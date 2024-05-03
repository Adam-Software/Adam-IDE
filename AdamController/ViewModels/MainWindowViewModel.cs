using AdamController.Core;
using AdamController.Core.Extensions;
using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace AdamController.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region DelegateCommands

        public DelegateCommand<string> ShowRegionCommand { get; private set; }

        #endregion

        #region Services

        public IRegionManager RegionManager { get; }
        private readonly ISubRegionChangeAwareService mSubRegionChangeAwareService;
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
                    IAvalonEditService avalonEditService, IThemeManagerService themeManager, ICultureProvider cultureProvider) 
        {
            RegionManager = regionManager;
            mWebApiService = webApiService;
            mSubRegionChangeAwareService = subRegionChangeAwareService;
            mStatusBarNotification = statusBarNotification;
            mCommunicationProviderService = communicationProviderService;
            mFolderManagment = folderManagment;
            mAvalonEditService = avalonEditService;
            mThemeManager = themeManager;
            mCultureProvider = cultureProvider;

            ShowRegionCommand = new DelegateCommand<string>(ShowRegion);            
            Subscribe();
        }

        #endregion

        #region Public fields

        public string WindowTitle => $"Adam IDE {Assembly.GetExecutingAssembly().GetName().Version}";

        /// <summary>
        /// -1 is not selected
        /// </summary>
        private int hamburgerMenuSelectedIndex = -1;
        public int HamburgerMenuSelectedIndex 
        { 
            get { return hamburgerMenuSelectedIndex; }
            set
            {
                SetProperty(ref hamburgerMenuSelectedIndex, value);
            } 
        }

        /// <summary>
        /// -1 is not selected
        /// </summary>
        private int hamburgerMenuSelectedOptionsIndex = -1;

        public int HamburgerMenuSelectedOptionsIndex
        {
            get { return hamburgerMenuSelectedOptionsIndex; }

            set
            {
                SetProperty(ref hamburgerMenuSelectedOptionsIndex, value);
            }
        }

        #endregion

        #region Private methods

        private void ChangeSelectedIndexByRegionName(string subRegionName)
        {
            switch (subRegionName)
            {
                case SubRegionNames.SubRegionScratch:
                    HamburgerMenuSelectedIndex = 0;
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    HamburgerMenuSelectedIndex = 1;
                    break;
                case SubRegionNames.SubRegionVisualSettings:
                    HamburgerMenuSelectedOptionsIndex = 0;
                    break;
            }
        }

        private void ShowRegion(string subRegionName)
        {
            switch (subRegionName)
            {
                case SubRegionNames.SubRegionScratch:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScratch);
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionComputerVisionControl);
                    break;
                case SubRegionNames.SubRegionVisualSettings:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionVisualSettings);
                    break;
            }
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
            mSubRegionChangeAwareService.RaiseSubRegionChangeEvent += RaiseSubRegionChangeEvent;
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent += RaiseTcpServiceCientConnectedEvent;
            mCommunicationProviderService.RaiseUdpServiceServerReceivedEvent += RaiseUdpServiceServerReceivedEvent;
            Application.Current.MainWindow.Loaded += MainWindowLoaded;
        }

        /// <summary>
        /// #20
        /// </summary>
        private void Unsubscribe()
        {
            mSubRegionChangeAwareService.RaiseSubRegionChangeEvent -= RaiseSubRegionChangeEvent;
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent -= RaiseTcpServiceCientConnectedEvent;
            mCommunicationProviderService.RaiseUdpServiceServerReceivedEvent -= RaiseUdpServiceServerReceivedEvent;
            Application.Current.MainWindow.Loaded -= MainWindowLoaded;
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
            mStatusBarNotification.CompileLogMessage = "Загрузка приложения завершена";
            

            if (Settings.Default.AutoStartTcpConnect)
                mCommunicationProviderService.ConnectAllAsync();
        }

        /// <summary>
        /// Changes the selected section in the hamburger menu
        /// </summary>
        private void RaiseSubRegionChangeEvent(object sender)
        {
            var changeRegionName = mSubRegionChangeAwareService.InsideRegionNavigationRequestName;
            ChangeSelectedIndexByRegionName(changeRegionName);
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
