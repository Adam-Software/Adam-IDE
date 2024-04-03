using AdamController.Core;
using AdamController.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Reflection;
using System.Windows;

namespace AdamController.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Command

        public DelegateCommand<string> ShowRegionCommand { get; private set; }

        #endregion

        #region Services

        public IRegionManager RegionManager { get; }
        private ISubRegionChangeAwareService SubRegionChangeAwareService { get; }

        #endregion

        #region ~

        public MainWindowViewModel(IRegionManager regionManager, ISubRegionChangeAwareService subRegionChangeAwareService) 
        {
            RegionManager = regionManager;
            ShowRegionCommand = new DelegateCommand<string>(ShowRegion);
            SubRegionChangeAwareService = subRegionChangeAwareService;
            
            SubRegionChangeAwareService.RaiseSubRegionChangeEvent += RaiseSubRegionChangeEvent;
            Application.Current.MainWindow.Loaded += MainWindowLoaded;
        }

        #endregion

        #region Events

        /// <summary>
        /// Load default region at startup
        /// Need to call it after loading the main window
        /// </summary>
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            ShowRegionCommand.Execute(SubRegionNames.SubRegionScratch);
        }

        /// <summary>
        /// Changes the selected section in the hamburger menu
        /// </summary>
        private void RaiseSubRegionChangeEvent(object sender)
        {
            var changeRegionName = SubRegionChangeAwareService?.InsideRegionNavigationRequestName;
            ChangeSelectedIndexByRegionName(changeRegionName);
        }

        #endregion

        #region Fields

        /// <summary>
        /// -1 is not selected
        /// </summary>
        private int mHamburgerMenuSelectedIndex = -1;
        public int HamburgerMenuSelectedIndex 
        { 
            get { return mHamburgerMenuSelectedIndex; }
            set
            {
                if(mHamburgerMenuSelectedIndex == value)
                    return;

                SetProperty(ref mHamburgerMenuSelectedIndex, value);
            } 
        }

        /// <summary>
        /// -1 is not selected
        /// </summary>
        private int mHamburgerMenuSelectedOptionsIndex = -1;

        public int HamburgerMenuSelectedOptionsIndex
        {
            get { return mHamburgerMenuSelectedOptionsIndex; }

            set
            {
                if (mHamburgerMenuSelectedOptionsIndex == value)
                    return;

                SetProperty(ref mHamburgerMenuSelectedOptionsIndex, value);
            }
        }

        public string WindowTitle => $"Adam IDE {Assembly.GetExecutingAssembly().GetName().Version}";

        #endregion

        #region Methods

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

        #region Old

        //public MainWindowViewModel()
        //{
        //ComunicateHelper.OnAdamTcpConnectedEvent += OnTcpConnected;
        //ComunicateHelper.OnAdamTcpDisconnectedEvent += OnTcpDisconnected;
        //ComunicateHelper.OnAdamTcpReconnected += OnTcpReconnected;
        //ComunicateHelper.OnAdamLogServerUdpReceivedEvent += ComunicateHelperOnAdamUdpReceived;


        //InitAction();

        //if (Settings.Default.AutoStartTcpConnect)
        //{
        //    ConnectButtonComand.Execute(null);
        //}
        //else
        //{
        //init fields if autorun off
        //TextOnConnectFlayotButton = mConnectButtonStatusDisconnected;
        //TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

        //ConnectIcon = PackIconModernKind.Connect;
        //IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
        //}
        //}

        #endregion

    }
}
