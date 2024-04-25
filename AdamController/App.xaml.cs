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
using AdamController.Core.Properties;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;
using AdamController.Services.TcpClientDependency;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using AdamController.Core;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICultureProvider, CultureProvider>();
            containerRegistry.RegisterSingleton<IFileManagmentService, FileManagmentService>();
            containerRegistry.RegisterSingleton<IFolderManagmentService, FolderManagmentService>();

            containerRegistry.RegisterSingleton<IAvalonEditService>(containerRegistry =>
            {
                IFileManagmentService fileManagment = containerRegistry.Resolve<IFileManagmentService>();
                AvalonEditService avalonService = new(fileManagment);
                return avalonService;
            });

            containerRegistry.RegisterSingleton<IWebViewProvider, WebViewProvider>();
            containerRegistry.RegisterSingleton<ISubRegionChangeAwareService, SubRegionChangeAwareService>();
            containerRegistry.RegisterSingleton<IStatusBarNotificationDeliveryService, StatusBarNotificationDeliveryService>();

            containerRegistry.RegisterSingleton<IFlyoutManager>(containerRegistry =>
            {
                IContainer container = containerRegistry.GetContainer();
                IRegionManager regionManager = containerRegistry.Resolve<IRegionManager>();

                return new FlyoutManager(container, regionManager);
            });

            containerRegistry.RegisterSingleton<ITcpClientService>(() =>
            {
                TcpClientOption option = new()
                {
                    ReconnectCount = Settings.Default.ReconnectQtyComunicateTcpClient,
                    ReconnectTimeout = Settings.Default.ReconnectTimeoutComunicateTcpClient
                };

                string ip = Settings.Default.ServerIP;
                int port = Settings.Default.TcpConnectStatePort;

                TcpClientService client = new(ip, port, option);
                return client;
            });

            containerRegistry.RegisterSingleton<IUdpClientService>(() =>
            {
                IPAddress ip = IPAddress.Any;
                int port = int.Parse(Settings.Default.MessageDataExchangePort);

                UdpClientService client = new(ip, port)
                {
                    OptionDualMode = true,
                    OptionReuseAddress = true
                };

                return client;
            });

            containerRegistry.RegisterSingleton<IUdpServerService>(() =>
            {
                IPAddress ip = IPAddress.Any;
                int port = Settings.Default.LogServerPort;

                UdpServerService server = new(ip, port)
                {
                    OptionDualMode = true,
                    OptionReuseAddress = true
                };

                return server;
            });

            containerRegistry.RegisterSingleton<IWebSocketClientService>(() =>
            {
                string ip = Settings.Default.ServerIP;
                int port = Settings.Default.SoketServerPort;

                // debug, use only with debug server, which runs separately, not as a service
                Uri uri = new($"ws://{Settings.Default.ServerIP}:9001/adam-2.7/movement");

                // work in production, connect to socket-server run as service
                // Uri uri = new($"ws://{ip}:{port}/adam-2.7/movement");

                WebSocketClientService client = new(uri);
                return client;
            });

            containerRegistry.RegisterSingleton<IWebApiService>(() =>
            {
                string ip = Settings.Default.ServerIP;
                int port = Settings.Default.ApiPort;
                string login = Settings.Default.ApiLogin;
                string password = Settings.Default.ApiPassword;

                WebApiService client = new(ip, port, login, password);
                return client;

            });

            containerRegistry.RegisterSingleton<ICommunicationProviderService>(containerRegistry =>
            {
                ITcpClientService tcpClientService = containerRegistry.Resolve<ITcpClientService>();
                IUdpClientService udpClientService = containerRegistry.Resolve<IUdpClientService>();
                IUdpServerService udpServerService = containerRegistry.Resolve<IUdpServerService>();
                IWebSocketClientService socketClientService = containerRegistry.Resolve<IWebSocketClientService>();
                
                CommunicationProviderService communicationProvider = new(tcpClientService, udpClientService, udpServerService, socketClientService);

                return communicationProvider;
            });

            containerRegistry.RegisterSingleton<IPythonRemoteRunnerService>(containerRegistry =>
            {
                IUdpClientService udpClient = containerRegistry.Resolve<IUdpClientService>();

                PythonRemoteRunnerService remoteRunnerService = new(udpClient);
                return remoteRunnerService;
            });

            containerRegistry.RegisterSingleton<IThemeManagerService, ThemeManagerService>();

            RegisterDialogs(containerRegistry);
        }

        private static void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SettingsView, SettingsViewModel>();

            //The old dialog call type integrated into the service
            containerRegistry.RegisterSingleton<IDialogManagerService>(containerRegistry =>
            {
                var app = Current;
                return new DialogManager(app);
            });
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
            Settings.Default.Save();
        }

        /// <summary>
        /// The priority of resource release is important!
        /// FirstRun/LastStop
        /// </summary>
        private void DisposeServices()
        {
            Container.Resolve<IThemeManagerService>().Dispose();
            Container.Resolve<IFileManagmentService>().Dispose();
            Container.Resolve<IFolderManagmentService>().Dispose();
            Container.Resolve<IAvalonEditService>().Dispose();

            Container.Resolve<ISubRegionChangeAwareService>().Dispose();
            Container.Resolve<IStatusBarNotificationDeliveryService>().Dispose();
            Container.Resolve<IWebViewProvider>().Dispose();
            Container.Resolve<IDialogManagerService>().Dispose();

            Container.Resolve<IPythonRemoteRunnerService>().Dispose();
            Container.Resolve<ICommunicationProviderService>().Dispose();
            Container.Resolve<IWebApiService>().Dispose();
            Container.Resolve<ICultureProvider>().Dispose();
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
            if (e.HResult == -2146233088)
            {
                // This message disables an error about the inability to connect to the websocket server.
                // As a temporary measure. Service errors should be handled in the services themselves
                if (e.InnerException.Source == "Websocket.Client")
                    return;
            }
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
