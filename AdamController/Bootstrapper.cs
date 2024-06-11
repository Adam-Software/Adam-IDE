using AdamStudio.Services.Interfaces;
using AdamStudio.Services;
using AdamStudio.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Windows;
using AdamStudio.Controls.CustomControls.Services;
using AdamStudio.Core.Properties;
using AdamStudio.Services.TcpClientDependency;
using Prism.Regions;
using System.Net;
using AdamStudio.Controls.CustomControls.RegionAdapters;
using MahApps.Metro.Controls;
using AdamStudio.Modules.ContentRegion;
using AdamStudio.Modules.FlayoutsRegion;
using AdamStudio.Modules.MenuRegion;
using AdamStudio.Modules.StatusBarRegion;
using Prism.Modularity;

namespace AdamStudio
{
    internal class Bootstrapper : PrismBootstrapper, IDisposable
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterSingleton<IFlyoutStateChecker, FlyoutStateChecker>();

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
                DryIoc.IContainer container = containerRegistry.GetContainer();
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

                TcpClientService client;

                if (!string.IsNullOrEmpty(ip))
                {
                    client = new(ip, port, option);
                }
                else
                {
                    client = new("127.0.0.1", port, option);
                }


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
                Uri uri;

                if (!string.IsNullOrEmpty(ip))
                {
                    uri = new($"ws://{ip}:{port}/adam-2.7/movement");
                }
                else
                {
                    uri = new($"ws://127.0.0.1:9001/adam-2.7/movement");
                }

                // debug, use only with debug server, which runs separately, not as a service
                //Uri uri = new($"ws://{Settings.Default.ServerIP}:9001/adam-2.7/movement");

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

                if (string.IsNullOrEmpty(ip))
                {
                    ip = "127.0.0.1";
                }

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
            containerRegistry.RegisterSingleton<IControlHelper>(containerRegistry =>
            {
                bool isVideoShowLastValue = Settings.Default.ShowVideo;
                return new ControlHelper(isVideoShowLastValue);
            });

            containerRegistry.RegisterSingleton<IVideoViewProvider, VideoViewProvider>();

            RegisterDialogs(containerRegistry);
        }

        private static void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            // used for call system dialog for open save/open/select file/folder (Microsoft.Win32 dialogs)
            containerRegistry.RegisterSingleton<ISystemDialogService, SystemDialogService>();

            //Dialog boxes are not used, but implemented
            //containerRegistry.RegisterDialog<SettingsView, SettingsViewModel>();
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

        public void Dispose()
        {
            IRegionManager regionManager = Container.Resolve<IRegionManager>();

            //Destroy() in module
            foreach (IRegion region in regionManager.Regions)
            {
                region.RemoveAll();
            }

            Container.Resolve<IThemeManagerService>().Dispose();
            Container.Resolve<IFileManagmentService>().Dispose();
            Container.Resolve<IFolderManagmentService>().Dispose();
            Container.Resolve<IAvalonEditService>().Dispose();

            Container.Resolve<ISubRegionChangeAwareService>().Dispose();
            Container.Resolve<IStatusBarNotificationDeliveryService>().Dispose();
            Container.Resolve<IWebViewProvider>().Dispose();
            Container.Resolve<ISystemDialogService>().Dispose();

            Container.Resolve<IPythonRemoteRunnerService>().Dispose();
            Container.Resolve<ICommunicationProviderService>().Dispose();
            Container.Resolve<IWebApiService>().Dispose();
            Container.Resolve<ICultureProvider>().Dispose();

            Container.Resolve<IControlHelper>().Dispose();
        }
    }
}
