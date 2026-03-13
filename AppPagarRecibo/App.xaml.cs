using AppPagarRecibo.Services;

namespace AppPagarRecibo;

public partial class App : Application
{
    public static DatabaseService DatabaseService { get; private set; }

    public App()
    {
        InitializeComponent();

        DatabaseService = new DatabaseService();
        Task.Run(async () => await DatabaseService.InicializarAsync()).Wait();

        MainPage = new AppShell();
    }
}
