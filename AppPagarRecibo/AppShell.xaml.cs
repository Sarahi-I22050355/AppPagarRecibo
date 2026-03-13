using AppPagarRecibo.Views;

namespace AppPagarRecibo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("resumen", typeof(ResumenAlumnoPage));
        Routing.RegisterRoute("pago", typeof(PagoPage));
        Routing.RegisterRoute("recibo", typeof(ReciboPage));
    }
}
