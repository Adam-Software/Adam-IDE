#region system

using System;
using System.Windows;

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
using AdamController.Services.Interfaces;
using AdamController.Controls.CustomControls.Services;
using AdamController.Controls.CustomControls.RegionAdapters;

#endregion

#region mahapps

using MahApps.Metro.Controls;

#endregion

#region other

using AdamController.Core.Properties;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;
using AdamController.Services.TcpClientDependency;
using System.ComponentModel;
using AdamController.Core.Dialog.ViewModels;
using AdamController.Core.Dialog.Views;
using Microsoft.Win32;
using System.IO;

#endregion

namespace AdamController
{
    public partial class App : Application
    {

        private readonly Bootstrapper mBootstrapper = new();

        public App()
        {
            Subscribe();
            LoadSharedFFmpegLibrary();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            mBootstrapper.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            OnAppCrashOrExit();

            base.OnExit(e);
        }

        private void OnAppCrashOrExit()
        {
            Unsubscribe();
            mBootstrapper.Dispose();
            Current.Shutdown();
        }

        #region Subscribes

        private void Subscribe()
        {
            SubscribeUnhandledExceptionHandling();
            Settings.Default.PropertyChanged += OnPropertyChange;
        }

        private void Unsubscribe()
        {
            Settings.Default.PropertyChanged -= OnPropertyChange;
        }

        #endregion

        #region OnRaise event
        
        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            Settings.Default.Save();
        }

        #endregion

        #region Intercepting Unhandled Exception

        private void SubscribeUnhandledExceptionHandling()
        {
            // Catch exceptions from all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowUnhandledException(args.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException", false);

            // Catch exceptions from each AppDomain that uses a task scheduler for async operations.
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                ShowUnhandledException(args.Exception, "TaskScheduler.UnobservedTaskException", false);

            // Catch exceptions from a single specific UI dispatcher thread.
            Current.Dispatcher.UnhandledException += (sender, args) =>
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

        #region PrivateMethods

        private static void LoadSharedFFmpegLibrary()
        {
            var ffmpegPath = AppDomain.CurrentDomain.BaseDirectory;
            Unosquare.FFME.Library.FFmpegDirectory = ffmpegPath;
        }

        #endregion
    }
}
