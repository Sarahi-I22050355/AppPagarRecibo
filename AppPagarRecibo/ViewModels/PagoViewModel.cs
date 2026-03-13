using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AppPagarRecibo.Models;
using AppPagarRecibo.Services;
using Microsoft.Maui.Controls;

namespace AppPagarRecibo.ViewModels
{
    public class PagoViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;
        private readonly SimulacionService _sim;
        private int _idUsuario;

        // Pestañas
        private bool _pagoConTarjeta = true;
        public bool PagoConTarjeta
        {
            get => _pagoConTarjeta;
            set => SetProperty(ref _pagoConTarjeta, value);
        }

        private bool _pagoConSPEI;
        public bool PagoConSPEI
        {
            get => _pagoConSPEI;
            set => SetProperty(ref _pagoConSPEI, value);
        }

        // TARJETA - TODOS estos campos SOLO viven en RAM, NUNCA en BD
        private string _titularTarjeta;
        public string TitularTarjeta
        {
            get => _titularTarjeta;
            set => SetProperty(ref _titularTarjeta, value);
        }

        private string _numeroTarjeta;
        public string NumeroTarjeta
        {
            get => _numeroTarjeta;
            set => SetProperty(ref _numeroTarjeta, value);
        }

        private string _fechaVencimientoTarjeta;
        public string FechaVencimientoTarjeta
        {
            get => _fechaVencimientoTarjeta;
            set => SetProperty(ref _fechaVencimientoTarjeta, value);
        }

        private string _cvv;
        public string CVV
        {
            get => _cvv;
            set => SetProperty(ref _cvv, value);
        }

        private string _bancoEmisor;
        public string BancoEmisor
        {
            get => _bancoEmisor;
            set => SetProperty(ref _bancoEmisor, value);
        }

        private ObservableCollection<string> _bancosDisponibles;
        public ObservableCollection<string> BancosDisponibles
        {
            get => _bancosDisponibles;
            set => SetProperty(ref _bancosDisponibles, value);
        }

        // SPEI
        private string _conceptoSPEI;
        public string ConceptoSPEI
        {
            get => _conceptoSPEI;
            set => SetProperty(ref _conceptoSPEI, value);
        }

        private string _clabe;
        public string CLABE
        {
            get => _clabe;
            set => SetProperty(ref _clabe, value);
        }

        private string _bancoReceptor;
        public string BancoReceptor
        {
            get => _bancoReceptor;
            set => SetProperty(ref _bancoReceptor, value);
        }

        private string _instrucciones;
        public string Instrucciones
        {
            get => _instrucciones;
            set => SetProperty(ref _instrucciones, value);
        }

        // Compartidos
        private string _referencia;
        public string Referencia
        {
            get => _referencia;
            set => SetProperty(ref _referencia, value);
        }

        private string _bancoNombre;
        public string BancoNombre
        {
            get => _bancoNombre;
            set => SetProperty(ref _bancoNombre, value);
        }

        private decimal _monto;
        public decimal Monto
        {
            get => _monto;
            set => SetProperty(ref _monto, value);
        }

        private decimal _montoDescuento;
        public decimal MontoDescuento
        {
            get => _montoDescuento;
            set => SetProperty(ref _montoDescuento, value);
        }

        private decimal _montoTotal;
        public decimal MontoTotal
        {
            get => _montoTotal;
            set => SetProperty(ref _montoTotal, value);
        }

        private DateTime _fechaVencimiento;
        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set => SetProperty(ref _fechaVencimiento, value);
        }

        // Simulación
        private bool _verificandoPago;
        public bool VerificandoPago
        {
            get => _verificandoPago;
            set => SetProperty(ref _verificandoPago, value);
        }

        private string _mensajeEstado;
        public string MensajeEstado
        {
            get => _mensajeEstado;
            set => SetProperty(ref _mensajeEstado, value);
        }

        // Comandos
        public ICommand SeleccionarTarjetaCommand { get; }
        public ICommand SeleccionarSPEICommand { get; }
        public ICommand PagarConTarjetaCommand { get; }
        public ICommand ConfirmarSPEICommand { get; }
        public ICommand CopiarCLABECommand { get; }
        public ICommand CopiarReferenciaCommand { get; }
        public ICommand CopiarConceptoCommand { get; }

        public PagoViewModel()
        {
            _db = App.DatabaseService;
            _sim = new SimulacionService();

            BancosDisponibles = new ObservableCollection<string>
            {
                "BBVA", "Banorte", "Santander", "HSBC",
                "Scotiabank", "Citibanamex", "Banco Azteca",
                "BanCoppel", "Inbursa", "Otro"
            };

            SeleccionarTarjetaCommand = new Command(() => { PagoConTarjeta = true; PagoConSPEI = false; });
            SeleccionarSPEICommand = new Command(() => { PagoConTarjeta = false; PagoConSPEI = true; });
            PagarConTarjetaCommand = new Command(async () => await PagarConTarjetaAsync());
            ConfirmarSPEICommand = new Command(async () => await ConfirmarSPEIAsync());
            CopiarCLABECommand = new Command(async () => await Clipboard.SetTextAsync(CLABE));
            CopiarReferenciaCommand = new Command(async () => await Clipboard.SetTextAsync(Referencia));
            CopiarConceptoCommand = new Command(async () => await Clipboard.SetTextAsync(ConceptoSPEI));
        }

        public async Task CargarDatosPagoAsync(int idUsuario, decimal monto, decimal descuento, decimal total)
        {
            _idUsuario = idUsuario;
            Monto = monto;
            MontoDescuento = descuento;
            MontoTotal = total;
            FechaVencimiento = DateTime.Now.AddDays(5);
            Referencia = _sim.GenerarReferencia();

            var usuario = await _db.ObtenerUsuarioPorId(idUsuario);
            ConceptoSPEI = _sim.GenerarConceptoSPEI(usuario.Nombre, usuario.Matricula);

            var banco = await _db.ObtenerBancoPrincipal();
            if (banco != null)
            {
                BancoNombre = banco.Descripcion;
                CLABE = banco.CLABE;
                BancoReceptor = banco.BancoReceptor;
                Instrucciones = banco.InstruccionesSPEI;
            }
        }

        private async Task PagarConTarjetaAsync()
        {
            if (IsBusy) return;

            // Mostrar alertas visuales nativas si faltan datos en la RAM
            if (string.IsNullOrWhiteSpace(TitularTarjeta))
            {
                await Shell.Current.DisplayAlert("Aviso", "Ingresa el nombre del titular de la tarjeta.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(NumeroTarjeta) || NumeroTarjeta.Replace(" ", "").Length < 16)
            {
                await Shell.Current.DisplayAlert("Aviso", "Ingresa un número de tarjeta válido (16 dígitos).", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(FechaVencimientoTarjeta) || FechaVencimientoTarjeta.Length < 5)
            {
                await Shell.Current.DisplayAlert("Aviso", "Ingresa la fecha de vencimiento (MM/AA).", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CVV) || CVV.Length < 3)
            {
                await Shell.Current.DisplayAlert("Aviso", "Ingresa el CVV (3 dígitos).", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(BancoEmisor))
            {
                await Shell.Current.DisplayAlert("Aviso", "Selecciona tu banco.", "OK");
                return;
            }

            IsBusy = true;
            VerificandoPago = true;

            try
            {
                // Extraer últimos 4 ANTES de limpiar
                string ultimos4 = _sim.ExtraerUltimosDigitos(NumeroTarjeta);

                MensajeEstado = "Conectando con el banco...";
                await Task.Delay(1500);
                MensajeEstado = "Procesando pago...";
                await Task.Delay(1500);
                MensajeEstado = "Confirmando transacción...";
                await Task.Delay(1000);

                // SEGURIDAD: Limpiar TODOS los datos sensibles de la RAM
                TitularTarjeta = null;
                NumeroTarjeta = null;
                FechaVencimientoTarjeta = null;
                CVV = null;

                string codigo = _sim.GenerarCodigoConfirmacion();

                var transaccion = new Transaccion
                {
                    IdUsuario = _idUsuario,
                    Referencia = Referencia,
                    Codigo = codigo,
                    Concepto = "INSCRIPCIÓN DEL SEMESTRE Y CUOTA DE MANTENIMIENTO",
                    TipoPago = "DEBITO",
                    UltimosDigitos = ultimos4,
                    BancoEmisor = BancoEmisor,
                    ConceptoSPEI = null,
                    Monto = Monto,
                    MontoDescuento = MontoDescuento,
                    MontoTotal = MontoTotal,
                    IdBancoReceptor = 1,
                    FechaVencimiento = FechaVencimiento,
                    FechaMovimiento = DateTime.Now,
                    Pagado = true,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioCrea = _idUsuario
                };

                int idTx = await _db.GuardarTransaccion(transaccion);
                await CrearInscripcion(idTx);
                await Shell.Current.GoToAsync($"recibo?idTransaccion={idTx}");
            }
            finally
            {
                IsBusy = false;
                VerificandoPago = false;
                MensajeEstado = null;
            }
        }

        private async Task ConfirmarSPEIAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            VerificandoPago = true;

            try
            {
                MensajeEstado = "Conectando con el banco...";
                await Task.Delay(1500);
                MensajeEstado = "Verificando referencia...";
                await Task.Delay(1500);
                MensajeEstado = "Confirmando pago...";
                await Task.Delay(1000);

                string codigo = _sim.GenerarCodigoConfirmacion();

                var transaccion = new Transaccion
                {
                    IdUsuario = _idUsuario,
                    Referencia = Referencia,
                    Codigo = codigo,
                    Concepto = "INSCRIPCIÓN DEL SEMESTRE Y CUOTA DE MANTENIMIENTO",
                    TipoPago = "SPEI",
                    ConceptoSPEI = ConceptoSPEI,
                    UltimosDigitos = null,
                    BancoEmisor = null,
                    Monto = Monto,
                    MontoDescuento = MontoDescuento,
                    MontoTotal = MontoTotal,
                    IdBancoReceptor = 1,
                    FechaVencimiento = FechaVencimiento,
                    FechaMovimiento = DateTime.Now,
                    Pagado = true,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioCrea = _idUsuario
                };

                int idTx = await _db.GuardarTransaccion(transaccion);
                await CrearInscripcion(idTx);
                await Shell.Current.GoToAsync($"recibo?idTransaccion={idTx}");
            }
            finally
            {
                IsBusy = false;
                VerificandoPago = false;
                MensajeEstado = null;
            }
        }

        private async Task CrearInscripcion(int idTransaccion)
        {
            var periodo = await _db.ObtenerPeriodoActual();
            if (periodo != null)
            {
                await _db.GuardarInscripcion(new UsuarioPeriodoSemestral
                {
                    IdUsuario = _idUsuario,
                    IdPeriodoSemestral = periodo.Id,
                    IdTransaccion = idTransaccion,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioCrea = _idUsuario
                });
            }
        }
    }
}