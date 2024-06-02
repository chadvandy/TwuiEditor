using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.ServiceProcess;
using System.Windows;
using TwUiEd.Core.Services;
using TwUiEd.Core.Views.Global;
using TwUiEd.Core.Views.Twui;

namespace TwUiEd.Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Me => (App)Current;
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();

            InitializeComponent();
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            var window = Services.GetService<MainWindow>();

            window.Show();
        }

        private static IServiceProvider ConfigureServices()
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
