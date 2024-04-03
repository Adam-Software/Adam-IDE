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

#endregion

namespace AdamController
{
    public partial class App : PrismApplication
    {
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
            _ = ThemeManager.Current.ChangeTheme(this, $"{Core.Properties.Settings.Default.BaseTheme}.{Core.Properties.Settings.Default.ThemeColorScheme}", false);

            string ip = Core.Properties.Settings.Default.ServerIP;
            int port = Core.Properties.Settings.Default.ApiPort;

            Uri DefaultUri = new($"http://{ip}:{port}");
            WebApi.Client.v1.BaseApi.SetApiClientUri(DefaultUri);

            string login = Core.Properties.Settings.Default.ApiLogin;
            string password = Core.Properties.Settings.Default.ApiPassword;

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

            //here must be ip/port
            containerRegistry.RegisterSingleton<IAdamTcpClientService, AdamTcpClientService>();

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
                using var stream = new MemoryStream(Core.Properties.Resource.AdamPython);
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
            Core.Properties.Settings.Default.BaseTheme = ThemeManager.Current.DetectTheme(Current).BaseColorScheme;
            Core.Properties.Settings.Default.ThemeColorScheme = ThemeManager.Current.DetectTheme(Current).ColorScheme;
            Core.Properties.Settings.Default.Save();

            base.OnExit(e);
        }
    }
}
