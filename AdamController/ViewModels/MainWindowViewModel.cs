using AdamController.Core;
using AdamController.Core.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Reflection;

namespace AdamController.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand<string> ShowRegionCommand { get; private set; }

        public IRegionManager RegionManager { get; }

        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService) 
        {
            RegionManager = regionManager;
            ShowRegionCommand = new DelegateCommand<string>(ShowRegion);

            System.Windows.Application.Current.MainWindow.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowRegionCommand.Execute(SubRegionNames.SubRegionScratch);
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


        #region Services



        #endregion

        #region Fields

        public string WindowTitle => $"Adam IDE {Assembly.GetExecutingAssembly().GetName().Version}";

        #endregion

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

    }
}
