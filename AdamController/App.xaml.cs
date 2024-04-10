#region system

using System;
using System.IO;
using System.Windows;
using System.Xml;

#endregion

#region prism

using Prism.Ioc;
using Prism.DryIoc;
using Prism.Modularity;
using Prism.Regions;
using DryIoc;

#endregion

#region innerhit

using AdamController.Views;
using AdamController.Modules.MenuRegion;
using AdamController.Modules.ContentRegion;
using AdamController.Core.Helpers;
using AdamController.Services;
using AdamController.Modules.StatusBarRegion;
using AdamController.Modules.FlayoutsRegion;
using AdamController.Core.Dialog.Views;
using AdamController.Core.Dialog.ViewModels;
using AdamController.Services.Interfaces;
using AdamController.Controls.CustomControls.Services;
using AdamController.Controls.CustomControls.RegionAdapters;

#endregion

#region mahapps

using MahApps.Metro.Controls;

#endregion

#region other

using ControlzEx.Theming;
using ICSharpCode.AvalonEdit.Highlighting;
using AdamController.Services.AdamTcpClientDependency;
using AdamController.Core.Properties;
using System.Diagnostics;
using System.Threading.Tasks;

#endregion

namespace AdamController
{
    public partial class App : PrismApplication
    {
        #region ~

        public App()
        {
            SetupUnhandledExceptionHandling();
        }

        #endregion

        protected override Window CreateShell()
        {
            MainWindow window = Container.Resolve<MainWindow>();
            return window;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoadHighlighting();

            _ = FolderHelper.CreateAppDataFolder();

            //TODO check theme before ChangeTheme
            _ = ThemeManager.Current.ChangeTheme(this, $"{Settings.Default.BaseTheme}.{Settings.Default.ThemeColorScheme}", false);

            string ip = Settings.Default.ServerIP;
            int port = Settings.Default.ApiPort;

            Uri DefaultUri = new($"http://{ip}:{port}");
            WebApi.Client.v1.BaseApi.SetApiClientUri(DefaultUri);

            string login = Settings.Default.ApiLogin;
            string password = Settings.Default.ApiPassword;

            WebApi.Client.v1.BaseApi.SetAuthenticationHeader(login, password);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISubRegionChangeAwareService>(containerRegistry =>
            {
                return new SubRegionChangeAwareService();
            });

            containerRegistry.RegisterSingleton<IFlyoutManager>(containerRegistry =>
            {
                IContainer container = containerRegistry.GetContainer();
                IRegionManager regionManager = containerRegistry.Resolve<IRegionManager>();

                return new FlyoutManager(container, regionManager);
            });

            containerRegistry.RegisterSingleton<IAdamTcpClientService>(containerRegistry =>
            {
                AdamTcpClientOption option = new()
                {
                    ReconnectCount = Settings.Default.ReconnectQtyComunicateTcpClient,
                    ReconnectTimeout = Settings.Default.ReconnectTimeoutComunicateTcpClient
                };

                string ip = Settings.Default.ServerIP;
                int port = Settings.Default.TcpConnectStatePort;

                AdamTcpClientService client = new(ip, port, option);
                return client;
            });

            containerRegistry.RegisterSingleton<ICommunicationProviderService>(containerRegistry =>
            {
                IAdamTcpClientService tcpClientService = containerRegistry.Resolve<IAdamTcpClientService>();
                CommunicationProviderService communicationProvider = new(tcpClientService);
                return communicationProvider;
            });

            RegisterDialogs(containerRegistry);
        }

        private static void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterDialog<NetworkTestView, NetworkTestViewModel>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping(typeof(FlyoutsControl), Container.Resolve<FlyoutsControlRegionAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MenuRegionModule>();
            moduleCatalog.AddModule<ContentRegionModule>();
            moduleCatalog.AddModule<StatusBarRegionModule>();
            moduleCatalog.AddModule<FlayoutsRegionModule>();
        }

        private static void LoadHighlighting()
        {
            try
            {
                using var stream = new MemoryStream(Resource.AdamPython);
                using var reader = new XmlTextReader(stream);
                HighlightingManager.Instance.RegisterHighlighting("AdamPython", Array.Empty<string>(),
                    ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, HighlightingManager.Instance));
            }
            catch
            {

            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            OnAppCrashOrExit();

            base.OnExit(e);
        }

        private void OnAppCrashOrExit()
        {
            SaveSettiings();
            DisposeServices();
            Current.Shutdown();
        }

        private void SaveSettiings()
        {
            Settings.Default.BaseTheme = ThemeManager.Current.DetectTheme(Current).BaseColorScheme;
            Settings.Default.ThemeColorScheme = ThemeManager.Current.DetectTheme(Current).ColorScheme;
            Settings.Default.Save();
        }

        private void DisposeServices()
        {
            Container.Resolve<ISubRegionChangeAwareService>().Dispose();
            Container.Resolve<IAdamTcpClientService>().Dispose();
            Container.Resolve<ICommunicationProviderService>().Dispose();
        }

        #region Intercepting Unhandled Exception

        private void SetupUnhandledExceptionHandling()
        {
            // Catch exceptions from all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowUnhandledException(args.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException", false);

            // Catch exceptions from each AppDomain that uses a task scheduler for async operations.
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                ShowUnhandledException(args.Exception, "TaskScheduler.UnobservedTaskException", false);

            // Catch exceptions from a single specific UI dispatcher thread.
            Dispatcher.UnhandledException += (sender, args) =>
            {
                // If we are debugging, let Visual Studio handle the exception and take us to the code that threw it.
                if (!Debugger.IsAttached)
                {
                    args.Handled = true;
                    ShowUnhandledException(args.Exception, "Dispatcher.UnhandledException", true);
                }
            };
        }

        private void ShowUnhandledException(Exception e, string unhandledExceptionType, bool promptUserForShutdown)
        {
            var messageBoxTitle = $"An unexpected error has occurred: {unhandledExceptionType}";
            var messageBoxMessage = $"The following exception occurred:\n\n{e}";
            var messageBoxButtons = MessageBoxButton.OK;

            if (promptUserForShutdown)
            {
                messageBoxMessage += "\n\nTo continue working, you need to exit the application. Can I do it now?";
                messageBoxButtons = MessageBoxButton.YesNo;
            }

            // Let the user decide if the app should die or not (if applicable).
            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons) == MessageBoxResult.Yes)
            {
                OnAppCrashOrExit();
            }
        }

        #endregion
    }
}
