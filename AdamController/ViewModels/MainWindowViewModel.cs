using AdamController.Core;
using AdamController.Core.Mvvm;
using AdamController.Services.Interfaces;
using AdamController.WebApi.Client.v1;
using Prism.Commands;
using Prism.Regions;
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

        #endregion

        #region ~

        public MainWindowViewModel(IRegionManager regionManager, ISubRegionChangeAwareService subRegionChangeAwareService, 
                                    IStatusBarNotificationDeliveryService statusBarNotification, ICommunicationProviderService communicationProviderService) 
        {
            RegionManager = regionManager;
            mSubRegionChangeAwareService = subRegionChangeAwareService;
            mStatusBarNotification = statusBarNotification;
            mCommunicationProviderService = communicationProviderService;

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
                case SubRegionNames.SubRegionScriptEditor:
                    HamburgerMenuSelectedIndex = 1;
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    HamburgerMenuSelectedIndex = 2;
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
                case SubRegionNames.SubRegionScriptEditor:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionScriptEditor);
                    break;
                case SubRegionNames.SubRegionComputerVisionControl:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionComputerVisionControl);
                    break;
                case SubRegionNames.SubRegionVisualSettings:
                    RegionManager.RequestNavigate(RegionNames.ContentRegion, SubRegionNames.SubRegionVisualSettings);
                    break;
            }
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
            Application.Current.MainWindow.Loaded += MainWindowLoaded;
        }

        /// <summary>
        /// #20
        /// </summary>
        private void Unsubscribe()
        {
            mSubRegionChangeAwareService.RaiseSubRegionChangeEvent -= RaiseSubRegionChangeEvent;
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent -= RaiseTcpServiceCientConnectedEvent;
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
            ShowRegionCommand.Execute(SubRegionNames.SubRegionScratch);

            mStatusBarNotification.CompileLogMessage = "Загрузка приложения завершена";
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
            _ = BaseApi.StopPythonExecute();
        }

        #endregion

    }
}
