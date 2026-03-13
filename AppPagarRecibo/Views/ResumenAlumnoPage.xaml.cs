using AppPagarRecibo.ViewModels;

namespace AppPagarRecibo.Views;

[QueryProperty(nameof(IdUsuario), "idUsuario")]
public partial class ResumenAlumnoPage : ContentPage
{
    private readonly ResumenAlumnoViewModel _vm;

    public string IdUsuario { get; set; }

    public ResumenAlumnoPage()
    {
        InitializeComponent();
        _vm = new ResumenAlumnoViewModel();
        BindingContext = _vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (int.TryParse(IdUsuario, out int id))
        {
            await _vm.CargarDatosAlumnoAsync(id);
        }
    }
}
