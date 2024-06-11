using AdamStudio.Controls.CustomControls.Services;
using AdamStudio.Controls.Enums;
using AdamStudio.Core;
using AdamStudio.Core.Extensions;
using AdamStudio.Core.Model;
using AdamStudio.Core.Mvvm;
using AdamStudio.Services.Interfaces;
using AdamStudio.Core.Properties;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace AdamStudio.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region DelegateCommands
        
        public DelegateCommand<string> ShowRegionCommand { get; }
        public DelegateCommand<string> MoveSplitterDelegateCommand { get; }
        public DelegateCommand SwitchToVideoDelegateCommand { get; }
        public DelegateCommand SwitchToSettingsViewDelegateCommand { get; }

        #endregion

        #region Services

        public ISubRegionChangeAwareService SubRegionChangeAwareService { get; }
        public IControlHelper ControlHelper { get; }

        private readonly IRegionManager mRegionManager;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotification;
        private readonly ICommunicationProviderService mCommunicationProviderService;
        private readonly IFolderManagmentService mFolderManagment;
        private readonly IWebApiService mWebApiService;
        private readonly IAvalonEditService mAvalonEditService;
        private readonly IThemeManagerService mThemeManager;
        private readonly ICultureProvider mCultureProvider;

        #endregion

        #region ~

        public MainWindowViewModel(IRegionManager regionManager, ISubRegionChangeAwareService subRegionChangeAwareService, IStatusBarNotificationDeliveryService statusBarNotification, 
                    ICommunicationProviderService communicationProviderService, IFolderManagmentService folderManagment, IWebApiService webApiService, 
                    IAvalonEditService avalonEditService, IThemeManagerService themeManager, ICultureProvider cultureProvider, 
                    IControlHelper controlHelper) 
        {
            mRegionManager = regionManager;
            mWebApiService = webApiService;
            SubRegionChangeAwareService = subRegionChangeAwareService;
            mStatusBarNotification = statusBarNotification;
            mCommunicationProviderService = communicationProviderService;
            mFolderManagment = folderManagment;
            mAvalonEditService = avalonEditService;
            mThemeManager = themeManager;
            mCultureProvider = cultureProvider;
            ControlHelper = controlHelper;

            ShowRegionCommand = new DelegateCommand<string>(ShowRegion);
            MoveSplitterDelegateCommand = new DelegateCommand<string>(MoveSplitter, MoveSplitterCanExecute);

            SwitchToVideoDelegateCommand = new DelegateCommand(SwitchToVideo, SwitchToVideoCanExecute);
            SwitchToSettingsViewDelegateCommand = new DelegateCommand(SwitchToSettingsView, SwitchToSettingsViewCanExecute);

            Subscribe();
        }

        #endregion

        #region Public fields

        public string WindowTitle => $"AdamStudio {Assembly.GetExecutingAssembly().GetName().Version}";


        #endregion

        #region DelegateCommands methods

        private void MoveSplitter(string commandArg)
        {
            BlocklyViewMode currentViewMode = ControlHelper.CurrentBlocklyViewMode;

            if (commandArg == "Left")
            {
                if (currentViewMode == BlocklyViewMode.FullScreen)
                    ControlHelper.CurrentBlocklyViewMode = BlocklyViewMode.MiddleScreen;

                if (currentViewMode == BlocklyViewMode.MiddleScreen)
                    ControlHelper.CurrentBlocklyViewMode = BlocklyViewMode.Hidden;
            }

            if (commandArg == "Right")
            {
                if (currentViewMode == BlocklyViewMode.Hidden)
                    ControlHelper.CurrentBlocklyViewMode = BlocklyViewMode.MiddleScreen;

                if (currentViewMode == BlocklyViewMode.MiddleScreen)
                    ControlHelper.CurrentBlocklyViewMode = BlocklyViewMode.FullScreen;
            }
        }

        private bool MoveSplitterCanExecute(string arg)
        {
            var regionName = SubRegionChangeAwareService.InsideRegionNavigationRequestName;
            return regionName == SubRegionNames.SubRegionScratch;
        }

        private void SwitchToVideo()
        {
            if (ControlHelper.IsShowVideo)
            {
                ControlHelper.IsShowVideo = false;
                return;
            }
                
            ControlHelper.IsShowVideo = true;
            return;
        }

        private bool SwitchToVideoCanExecute()
        {
            var regionName = SubRegionChangeAwareService.InsideRegionNavigationRequestName;
            return regionName == SubRegionNames.SubRegionScratch;
        }

        private void SwitchToSettingsView()
        {
            
            var regionName = SubRegionChangeAwareService.InsideRegionNavigationRequestName;
            
            if (regionName == SubRegionNames.SubRegionScratch)
            {
                ShowRegion(SubRegionNames.SubRegionVisualSettings);
                return;
            }
            
            if(regionName == SubRegionNames.SubRegionVisualSettings)
            {
                ShowRegion(SubRegionNames.SubRegionScratch);
                return;
            }    
        }

        private bool SwitchToSettingsViewCanExecute()
        {
            return true;
        }

        #endregion

        #region Private methods

        private void ShowRegion(string subRegionName)
        {
            switch (subRegionName)
            {
                case SubRegionNames.SubRegionScratch:
                    mRegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScratch);
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    mRegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionComputerVisionControl);
                    break;
                case SubRegionNames.SubRegionVisualSettings:
                    mRegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionVisualSettings);
                    break;
            }

            MoveSplitterDelegateCommand.RaiseCanExecuteChanged();
            SwitchToVideoDelegateCommand.RaiseCanExecuteChanged(); 
        }

        /// <summary>
        /// starts when the application is first launched
        /// </summary>
        private void SaveFolderPathToSettings()
        {
            if (string.IsNullOrEmpty(Settings.Default.SavedUserWorkspaceFolderPath))
            {
                Settings.Default.SavedUserWorkspaceFolderPath = mFolderManagment.SavedWorkspaceDocumentsDir;
            }

            if (string.IsNullOrEmpty(Settings.Default.SavedUserScriptsFolderPath))
            {
                Settings.Default.SavedUserScriptsFolderPath = mFolderManagment.SavedUserScriptsDocumentsDir;
            }
        }

        private void ParseSyslogMessage(string message)
        {
            try
            {
                SyslogMessageModel syslogMessage = message.Parse();
                mStatusBarNotification.CompileLogMessage = $"{syslogMessage.TimeStamp:T} {syslogMessage.Message}";   
            }
            catch
            {
                // If you couldn't read the message, it's okay, no one needs to know about it.
            }
        }

        /// <summary>
        /// Register highlighting for AvalonEdit. You need to call before loading the regions
        /// </summary>
        private void LoadCustomAvalonEditHighlighting()
        {
            mAvalonEditService.RegisterHighlighting(HighlightingName.AdamPython, Resource.AdamPython);
        }

        private void LoadAppTheme()
        {
            var appThemeName = Settings.Default.AppThemeName;
            mThemeManager.ChangeAppTheme(appThemeName);
        }

        private void LoadDefaultCultureInfo()
        {
            CultureInfo lastLoadLanguage = mCultureProvider.SupportAppCultures.FirstOrDefault(x => x.Name == Settings.Default.AppLanguage);
            lastLoadLanguage ??= mCultureProvider.SupportAppCultures.FirstOrDefault();

            mCultureProvider.ChangeAppCulture(lastLoadLanguage);
        }

        #endregion

        #region Subscriptions

        /// <summary>
        /// #20
        /// </summary>
        private void Subscribe()
        {
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent += RaiseTcpServiceCientConnectedEvent;
            mCommunicationProviderService.RaiseUdpServiceServerReceivedEvent += RaiseUdpServiceServerReceivedEvent;

            Application.Current.MainWindow.Loaded += MainWindowLoaded;
            Application.Current.MainWindow.Closed += MainWindowClosed; 
        }

        /// <summary>
        /// #20
        /// </summary>
        private void Unsubscribe()
        {
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent -= RaiseTcpServiceCientConnectedEvent;
            mCommunicationProviderService.RaiseUdpServiceServerReceivedEvent -= RaiseUdpServiceServerReceivedEvent;
        }

        #endregion

        #region Event methods
      
        /// <summary>
        /// Load default region at startup
        /// Need to call it after loading the main window
        /// </summary>
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadDefaultCultureInfo();

            if (Settings.Default.CreateUserDirrectory)
            {
                mFolderManagment.CreateAppDataFolder();
                SaveFolderPathToSettings();
            }
                

            LoadCustomAvalonEditHighlighting();
            LoadAppTheme();

            ShowRegionCommand.Execute(SubRegionNames.SubRegionScratch);

            if (Settings.Default.AutoStartTcpConnect)
                mCommunicationProviderService.ConnectAllAsync();
        }


        private void MainWindowClosed(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        /// <summary>
        /// It is not clear where to put this, so it will not get lost here.
        /// 
        /// Stops a remotely executed script that may have been executing before the connection was lost.
        /// </summary>
        private void RaiseTcpServiceCientConnectedEvent(object sender)
        {
            mWebApiService.StopPythonExecute();
        }

        private void RaiseUdpServiceServerReceivedEvent(object sender, string message)
        {
            ParseSyslogMessage(message);
        }

        #endregion
    }
}
