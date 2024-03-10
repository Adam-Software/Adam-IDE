
using AdamController.Views;
using ControlzEx.Theming;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.IO;
using System.Windows;
using System.Xml;

#region prism

using Prism.Ioc;
using Prism.DryIoc;
using Prism.Modularity;
using MahApps.Metro.Controls;
using AdamController.Modules.MenuRegion;
using AdamController.Modules.ContentRegion;
using AdamController.Core.Helpers;
using AdamController.Services;
using AdamController.Modules.StatusBar;

#endregion

namespace AdamController
{
    public partial class App : PrismApplication
    {

        public App()
        {

        }

        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            return window;
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            LoadHighlighting();

            // side menu context, MUST inherit from MainViewModel
            // HamburgerMenuView : MainWindowView
            // and MainWindowView  MUST inherit from BaseViewModel
            // MainWindowView : BaseViewModel

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

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<ITestService, TestService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MenuRegionModule>();
            moduleCatalog.AddModule<ContentRegionModule>();
            moduleCatalog.AddModule<StatusBarModule>();
        }

        private static void LoadHighlighting()
        {
            try
            {
                using var stream = new MemoryStream(AdamController.Properties.Resources.AdamPython);
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
