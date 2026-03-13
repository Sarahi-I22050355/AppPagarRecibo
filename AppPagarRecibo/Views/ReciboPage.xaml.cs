using AppPagarRecibo.ViewModels;

namespace AppPagarRecibo.Views;

[QueryProperty(nameof(IdTransaccion), "idTransaccion")]
public partial class ReciboPage : ContentPage
{
    private readonly ReciboViewModel _vm;

    public string IdTransaccion { get; set; }

    public ReciboPage()
    {
        InitializeComponent();
        _vm = new ReciboViewModel();
        BindingContext = _vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (int.TryParse(IdTransaccion, out int id))
        {
            await _vm.CargarReciboAsync(id);
        }
    }

    private async void VolverInicio_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }
}
