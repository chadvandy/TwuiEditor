using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.ServiceProcess;
using System.Windows;
using TwUiEd.Core.Services;
using TwUiEd.Core.ViewModels.Windows;
using TwUiEd.Core.Views.Twui;
using TwUiEd.Core.Views.Windows;

namespace TwUiEd.Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();

            //InitializeComponent();

#pragma warning disable WPF0001
            ThemeMode = ThemeMode.Dark;

            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            // On initial startup, create the MainWindow view and its attached
            // viewmodel. It's a bit of an anti-pattern, but there's no clean way
            // to instantiate the viewmodel first when loading up the app that
            // I know about.

            // From this point forward, the application loads up using the viewmodel
            // first, and resource dictionaries (defined in MainWindow.xaml and App.xaml)
            // provide the link between a viewmodel and a view.
            var window = Services.GetRequiredService<MainWindow>();
            var vm = Services.GetRequiredService<MainWindowViewModel>();

            window.DataContext = vm;
            window.Show();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<ContentsViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
