using AppPagarRecibo.ViewModels;

namespace AppPagarRecibo.Views;

[QueryProperty(nameof(IdUsuario), "idUsuario")]
[QueryProperty(nameof(MontoStr), "monto")]
[QueryProperty(nameof(DescuentoStr), "descuento")]
[QueryProperty(nameof(TotalStr), "total")]
public partial class PagoPage : ContentPage
{
    private readonly PagoViewModel _vm;

    public string IdUsuario { get; set; }
    public string MontoStr { get; set; }
    public string DescuentoStr { get; set; }
    public string TotalStr { get; set; }

    public PagoPage()
    {
        InitializeComponent();
        _vm = new PagoViewModel();
        BindingContext = _vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (int.TryParse(IdUsuario, out int id) &&
            decimal.TryParse(MontoStr, out decimal monto) &&
            decimal.TryParse(DescuentoStr, out decimal desc) &&
            decimal.TryParse(TotalStr, out decimal total))
        {
            await _vm.CargarDatosPagoAsync(id, monto, desc, total);
        }
    }
}
