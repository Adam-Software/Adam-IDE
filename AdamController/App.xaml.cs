using AdamController.Helpers;
using AdamController.Properties;
using AdamController.ViewModels.HamburgerMenu;
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
            _ = ThemeManager.Current.ChangeTheme(this, $"{Settings.Default.BaseTheme}.{Settings.Default.ThemeColorScheme}", false);

            string ip = Settings.Default.ServerIP;
            int port = Settings.Default.ApiPort;

            Uri DefaultUri = new($"http://{ip}:{port}");
            WebApi.Client.v1.BaseApi.SetApiClientUri(DefaultUri);

            string login = Settings.Default.ApiLogin;
            string password = Settings.Default.ApiPassword;

            WebApi.Client.v1.BaseApi.SetAuthenticationHeader(login, password);

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<Services.ICustomerStore, Services.DbCustomerStore>();
            // register other needed services here
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
            Settings.Default.BaseTheme = ThemeManager.Current.DetectTheme(Current).BaseColorScheme;
            Settings.Default.ThemeColorScheme = ThemeManager.Current.DetectTheme(Current).ColorScheme;
            Settings.Default.Save();

            base.OnExit(e);
        }
    }
}
