using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppPagarRecibo.Helpers;
using AppPagarRecibo.Services;

namespace AppPagarRecibo.ViewModels
{
    public class ResumenAlumnoViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;
        private int _idUsuario;

        private string _nombre;
        public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }

        private string _matricula;
        public string Matricula { get => _matricula; set => SetProperty(ref _matricula, value); }

        private string _carrera;
        public string Carrera { get => _carrera; set => SetProperty(ref _carrera, value); }

        private int _semestreActual;
        public int SemestreActual { get => _semestreActual; set => SetProperty(ref _semestreActual, value); }

        private string _tipoUsuario;
        public string TipoUsuario { get => _tipoUsuario; set => SetProperty(ref _tipoUsuario, value); }

        private string _periodo;
        public string Periodo { get => _periodo; set => SetProperty(ref _periodo, value); }

        private string _becaDescripcion;
        public string BecaDescripcion { get => _becaDescripcion; set => SetProperty(ref _becaDescripcion, value); }

        private decimal _becaPorcentaje;
        public decimal BecaPorcentaje { get => _becaPorcentaje; set => SetProperty(ref _becaPorcentaje, value); }

        private bool _tieneBeca;
        public bool TieneBeca { get => _tieneBeca; set => SetProperty(ref _tieneBeca, value); }

        private ObservableCollection<OportunidadDetalle> _oportunidadesConCostoExtra;
        public ObservableCollection<OportunidadDetalle> OportunidadesConCostoExtra
        {
            get => _oportunidadesConCostoExtra;
            set => SetProperty(ref _oportunidadesConCostoExtra, value);
        }

        private bool _tieneOportunidadesConCosto;
        public bool TieneOportunidadesConCosto { get => _tieneOportunidadesConCosto; set => SetProperty(ref _tieneOportunidadesConCosto, value); }

        private decimal _costoBase;
        public decimal CostoBase { get => _costoBase; set => SetProperty(ref _costoBase, value); }

        private decimal _costoOportunidades;
        public decimal CostoOportunidades { get => _costoOportunidades; set => SetProperty(ref _costoOportunidades, value); }

        private decimal _descuentoBeca;
        public decimal DescuentoBeca { get => _descuentoBeca; set => SetProperty(ref _descuentoBeca, value); }

        private decimal _totalAPagar;
        public decimal TotalAPagar { get => _totalAPagar; set => SetProperty(ref _totalAPagar, value); }

        public ICommand ContinuarPagoCommand { get; }

        public ResumenAlumnoViewModel()
        {
            _db = App.DatabaseService;
            OportunidadesConCostoExtra = new ObservableCollection<OportunidadDetalle>();
            ContinuarPagoCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(
                    $"pago?idUsuario={_idUsuario}" +
                    $"&monto={CostoBase + CostoOportunidades}" +
                    $"&descuento={DescuentoBeca}" +
                    $"&total={TotalAPagar}");
            });
        }

        public async Task CargarDatosAlumnoAsync(int idUsuario)
        {
            _idUsuario = idUsuario;
            IsBusy = true;

            try
            {
                var usuario = await _db.ObtenerUsuarioPorId(idUsuario);
                var tipoUsr = await _db.ObtenerTipoUsuario(usuario.IdTipoUsuario);
                var carrera = await _db.ObtenerCarrera(usuario.IdCarrera);
                var periodo = await _db.ObtenerPeriodoActual();

                Nombre = usuario.Nombre;
                Matricula = usuario.Matricula;
                Carrera = carrera.Descripcion;
                SemestreActual = usuario.SemestreActual;
                TipoUsuario = tipoUsr.Descripcion;
                Periodo = periodo?.Descripcion ?? "Sin periodo activo";

                // Beca
                if (usuario.IdBeca.HasValue)
                {
                    var beca = await _db.ObtenerBeca(usuario.IdBeca.Value);
                    TieneBeca = true;
                    BecaDescripcion = beca.Descripcion;
                    BecaPorcentaje = beca.Porcentaje;
                }
                else
                {
                    TieneBeca = false;
                    BecaDescripcion = "Sin beca";
                    BecaPorcentaje = 0;
                }

                // Oportunidades con costo extra (pendientes, Aprobado = null)
                OportunidadesConCostoExtra.Clear();
                var pendientes = await _db.ObtenerOportunidadesPendientes(idUsuario);
                decimal totalCostoExtra = 0;

                foreach (var op in pendientes)
                {
                    var tipoOp = await _db.ObtenerTipoOportunidad(op.IdTipoOportunidad);
                    if (tipoOp.GeneraCostoExtra)
                    {
                        var asig = await _db.ObtenerAsignatura(op.IdAsignatura);
                        OportunidadesConCostoExtra.Add(new OportunidadDetalle
                        {
                            ClaveAsignatura = asig.Clave,
                            NombreAsignatura = asig.Descripcion,
                            Semestre = asig.Semestre,
                            TipoOportunidad = tipoOp.Descripcion,
                            ClaveOportunidad = tipoOp.Clave,
                            EsEspecial = tipoOp.EsEspecial,
                            CostoExtra = op.CostoExtra
                        });
                        totalCostoExtra += op.CostoExtra;
                    }
                }

                TieneOportunidadesConCosto = OportunidadesConCostoExtra.Count > 0;

                // Cálculo de montos
                CostoBase = 2300.00m; // Monto base simulado (como en el recibo real del TEC)
                CostoOportunidades = totalCostoExtra;
                decimal subtotal = CostoBase + CostoOportunidades;
                DescuentoBeca = subtotal * (BecaPorcentaje / 100m);
                TotalAPagar = subtotal - DescuentoBeca;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
