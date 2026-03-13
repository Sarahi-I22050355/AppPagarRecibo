using System;
using System.Threading.Tasks;
using AppPagarRecibo.Services;

namespace AppPagarRecibo.ViewModels
{
    public class ReciboViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;

        private string _referencia;
        public string Referencia { get => _referencia; set => SetProperty(ref _referencia, value); }
        private string _codigo;
        public string Codigo { get => _codigo; set => SetProperty(ref _codigo, value); }
        private string _nombreAlumno;
        public string NombreAlumno { get => _nombreAlumno; set => SetProperty(ref _nombreAlumno, value); }
        private string _matricula;
        public string Matricula { get => _matricula; set => SetProperty(ref _matricula, value); }
        private string _carrera;
        public string Carrera { get => _carrera; set => SetProperty(ref _carrera, value); }
        private int _semestre;
        public int Semestre { get => _semestre; set => SetProperty(ref _semestre, value); }
        private string _concepto;
        public string Concepto { get => _concepto; set => SetProperty(ref _concepto, value); }
        private decimal _monto;
        public decimal Monto { get => _monto; set => SetProperty(ref _monto, value); }
        private decimal _descuento;
        public decimal Descuento { get => _descuento; set => SetProperty(ref _descuento, value); }
        private decimal _totalPagado;
        public decimal TotalPagado { get => _totalPagado; set => SetProperty(ref _totalPagado, value); }
        private string _tipoPago;
        public string TipoPago { get => _tipoPago; set => SetProperty(ref _tipoPago, value); }
        private bool _fueConTarjeta;
        public bool FueConTarjeta { get => _fueConTarjeta; set => SetProperty(ref _fueConTarjeta, value); }
        private bool _fueConSPEI;
        public bool FueConSPEI { get => _fueConSPEI; set => SetProperty(ref _fueConSPEI, value); }
        private string _tarjetaEnmascarada;
        public string TarjetaEnmascarada { get => _tarjetaEnmascarada; set => SetProperty(ref _tarjetaEnmascarada, value); }
        private string _bancoEmisor;
        public string BancoEmisor { get => _bancoEmisor; set => SetProperty(ref _bancoEmisor, value); }
        private string _bancoReceptor;
        public string BancoReceptor { get => _bancoReceptor; set => SetProperty(ref _bancoReceptor, value); }
        private string _cuentaDeposito;
        public string CuentaDeposito { get => _cuentaDeposito; set => SetProperty(ref _cuentaDeposito, value); }
        private string _periodo;
        public string Periodo { get => _periodo; set => SetProperty(ref _periodo, value); }
        private DateTime _fechaVencimiento;
        public DateTime FechaVencimiento { get => _fechaVencimiento; set => SetProperty(ref _fechaVencimiento, value); }
        private DateTime _fechaMovimiento;
        public DateTime FechaMovimiento { get => _fechaMovimiento; set => SetProperty(ref _fechaMovimiento, value); }

        public ReciboViewModel()
        {
            _db = App.DatabaseService;
        }

        public async Task CargarReciboAsync(int idTransaccion)
        {
            IsBusy = true;
            try
            {
                var tx = await _db.ObtenerTransaccion(idTransaccion);
                var usuario = await _db.ObtenerUsuarioPorId(tx.IdUsuario);
                var carrera = await _db.ObtenerCarrera(usuario.IdCarrera);
                var banco = await _db.ObtenerBancoPrincipal();
                var periodo = await _db.ObtenerPeriodoActual();

                Referencia = tx.Referencia;
                Codigo = tx.Codigo;
                NombreAlumno = usuario.Nombre;
                Matricula = usuario.Matricula;
                Carrera = carrera.Descripcion;
                Semestre = usuario.SemestreActual;
                Concepto = tx.Concepto;
                Monto = tx.Monto;
                Descuento = tx.MontoDescuento;
                TotalPagado = tx.MontoTotal;
                TipoPago = tx.TipoPago;
                FechaVencimiento = tx.FechaVencimiento;
                FechaMovimiento = tx.FechaMovimiento ?? DateTime.Now;
                Periodo = periodo?.Descripcion ?? "";

                if (tx.TipoPago == "DEBITO")
                {
                    FueConTarjeta = true;
                    FueConSPEI = false;
                    TarjetaEnmascarada = $"**** **** **** {tx.UltimosDigitos}";
                    BancoEmisor = tx.BancoEmisor;
                }
                else
                {
                    FueConTarjeta = false;
                    FueConSPEI = true;
                    BancoReceptor = banco?.Descripcion ?? "";
                    CuentaDeposito = banco?.CuentaDeposito ?? "";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
