using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppPagarRecibo.Helpers;
using AppPagarRecibo.Services;

namespace AppPagarRecibo.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        private string _matricula;
        public string Matricula
        {
            get => _matricula;
            set => SetProperty(ref _matricula, value);
        }

        private string _clave;
        public string Clave
        {
            get => _clave;
            set => SetProperty(ref _clave, value);
        }

        private bool _bloqueoPermanente;
        public bool BloqueoPermanente
        {
            get => _bloqueoPermanente;
            set => SetProperty(ref _bloqueoPermanente, value);
        }

        private string _mensajeBloqueoPermanente;
        public string MensajeBloqueoPermanente
        {
            get => _mensajeBloqueoPermanente;
            set => SetProperty(ref _mensajeBloqueoPermanente, value);
        }

        private bool _bloqueoDocumentacion;
        public bool BloqueoDocumentacion
        {
            get => _bloqueoDocumentacion;
            set => SetProperty(ref _bloqueoDocumentacion, value);
        }

        private string _mensajeBloqueoDocumentacion;
        public string MensajeBloqueoDocumentacion
        {
            get => _mensajeBloqueoDocumentacion;
            set => SetProperty(ref _mensajeBloqueoDocumentacion, value);
        }

        private ObservableCollection<AdeudoDetalle> _documentosPendientes;
        public ObservableCollection<AdeudoDetalle> DocumentosPendientes
        {
            get => _documentosPendientes;
            set => SetProperty(ref _documentosPendientes, value);
        }

        private string _mensajeError;
        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        private bool _mostrarError;
        public bool MostrarError
        {
            get => _mostrarError;
            set => SetProperty(ref _mostrarError, value);
        }

        public ICommand IniciarSesionCommand { get; }

        public LoginViewModel()
        {
            _db = App.DatabaseService;
            DocumentosPendientes = new ObservableCollection<AdeudoDetalle>();
            IniciarSesionCommand = new Command(async () => await IniciarSesionAsync());
        }

        private void LimpiarEstado()
        {
            MostrarError = false;
            MensajeError = null;
            BloqueoPermanente = false;
            MensajeBloqueoPermanente = null;
            BloqueoDocumentacion = false;
            MensajeBloqueoDocumentacion = null;
            DocumentosPendientes.Clear();
        }

        private async Task IniciarSesionAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            LimpiarEstado();

            try
            {
                if (string.IsNullOrWhiteSpace(Matricula) || string.IsNullOrWhiteSpace(Clave))
                {
                    MensajeError = "Ingresa tu matrícula y contraseña.";
                    MostrarError = true;
                    return;
                }

                var usuario = await _db.ObtenerUsuarioPorCredenciales(Matricula.Trim(), Clave.Trim());
                if (usuario == null)
                {
                    MensajeError = "Matrícula o contraseña incorrecta.";
                    MostrarError = true;
                    return;
                }

                // NIVEL 1: Bloqueo permanente por materia especial reprobada
                var reprobadas = await _db.ObtenerOportunidadesReprobadas(usuario.Id);
                foreach (var rep in reprobadas)
                {
                    var tipo = await _db.ObtenerTipoOportunidad(rep.IdTipoOportunidad);
                    if (tipo.EsEspecial)
                    {
                        var materia = await _db.ObtenerAsignatura(rep.IdAsignatura);
                        BloqueoPermanente = true;
                        MensajeBloqueoPermanente =
                            $"ACCESO DENEGADO\n\nHas sido dado de baja por reprobar " +
                            $"\"{materia.Descripcion}\" en {tipo.Descripcion}.\n\n" +
                            "Acude a Control Escolar para más información.";
                        return;
                    }
                }

                // NIVEL 2: Bloqueo por documentación pendiente
                var adeudos = await _db.ObtenerAdeudosActivos(usuario.Id);
                if (adeudos.Any())
                {
                    BloqueoDocumentacion = true;
                    MensajeBloqueoDocumentacion = "Tienes documentación pendiente. No puedes generar tu recibo de pago hasta regularizarte.";
                    foreach (var adeudo in adeudos)
                    {
                        var tipoAdeudo = await _db.ObtenerTipoAdeudo(adeudo.IdTipoAdeudo);
                        DocumentosPendientes.Add(new AdeudoDetalle
                        {
                            TipoAdeudo = tipoAdeudo.Descripcion,
                            Observacion = adeudo.Observacion ?? ""
                        });
                    }
                    return;
                }

                // Sin bloqueos -> navegar al resumen
                await Shell.Current.GoToAsync($"resumen?idUsuario={usuario.Id}");
            }
            catch (Exception ex)
            {
                MensajeError = $"Error: {ex.Message}";
                MostrarError = true;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
