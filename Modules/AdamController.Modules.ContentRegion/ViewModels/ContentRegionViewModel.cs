using AdamController.Core.Helpers;
using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ContentRegionViewModel : RegionViewModelBase
    {



        public ContentRegionViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            
        }

        #region Navigation

        #endregion

        #region OLD

        private void ComunicateHelperOnAdamUdpReceived(string message)
        {
            try
            {
                SyslogMessage syslogMessage = SyslogParseMessage.Parse(message);
                //CompileLogStatusBar = $"{syslogMessage.TimeStamp:T} {syslogMessage.Message}";
            }
            catch (Exception ex)
            {
                //CompileLogStatusBar = $"Error reading udp log with exception {ex.Message}";
            }
        }

        #region SelectedPageIndex

        //private int selectedPageIndex = 0;
        //public int SelectedPageIndex
        //{
            //get => selectedPageIndex;
            //set
            //{
                //if (value == selectedPageIndex)
                //{
                    //return;
                //}

                //selectedPageIndex = value;
                //GetSelectedPageIndex = value;

                //SetProperty(ref value, selectedPageIndex);
            //}
        //}


        //public static int GetSelectedPageIndex { get; private set; }

        #endregion

        #region Events TCP/IP clients

        private void OnTcpDisconnected()
        {
            //если центр уведомлений закрыт, обновляем счетчик уведомлений
            //if (!NotificationFlayoutsIsOpen && Settings.Default.IsMessageShowOnAbortMainConnection)
            //{
                //BadgeCounter++;
                //FailConnectMessageVisibility = Visibility.Visible;
            //}

            //TextOnConnectFlayotButton = mConnectButtonStatusDisconnected;
            //TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

            //ConnectIcon = PackIconModernKind.Connect;
            //IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
        }

        private void OnTcpConnected()
        {
            //_ = BaseApi.StopPythonExecute();

            //TextOnConnectFlayotButton = mConnectButtonStatusConnected;
            //TextOnStatusConnectToolbar = mToolbarStatusClientConnected;

            //ConnectIcon = PackIconModernKind.Disconnect;
            //IconOnConnectFlayoutButton = PackIconMaterialKind.Robot;
        }

        private void OnTcpReconnected(int reconnectCount)
        {
            //TextOnConnectFlayotButton = $"{mConnectButtonStatusReconnected} {reconnectCount}";
            //TextOnStatusConnectToolbar = $"{mToolbarStatusClientReconnected} {reconnectCount}";

            //ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            //IconOnConnectFlayoutButton = PackIconMaterialKind.RobotConfused;
        }

        #endregion

        #region InitAction

        private void InitAction()
        {
            //move to page with selected index
            //if (ScratchControlView.SetSelectedPageIndex == null)
            //{
            //    ScratchControlView.SetSelectedPageIndex = new Action<int>(index => SelectedPageIndex = index);
            //}

            //open flayout page
            //if (VisualSettingsControlView.OpenAdvancedBlocklySettings == null)
            //{
            //    VisualSettingsControlView.OpenAdvancedBlocklySettings = new Action<bool>(open => AdvancedBlocklySettingsFlayoutsIsOpen = open);
            //}

            //change toolbox lang settings
            //if (VisualSettingsControlView.SetToolboxLanguage == null)
            //{
            //    VisualSettingsControlView.SetToolboxLanguage = new Action<BlocklyLanguageModel>(model => SelectedBlocklyToolboxLanguage = model);
            //}

            //settings blockly theme
            //if (VisualSettingsControlView.SetBlocklyThemeAndGridColor == null)
            //{
            //    VisualSettingsControlView.SetBlocklyThemeAndGridColor = new Action<BlocklyThemeModel>
            //        (theme =>
            //        {
            //            SelectedBlocklyTheme = theme;
            //
             //           if (Settings.Default.ChangeGridColorSwitchToggleSwitchState) return;
             //
             //           SelectGridColorDependingSelectedTheme(theme.BlocklyTheme);
             //       });
            //}

            //if (VisualSettingsControlView.ChangeNotificationOpacity == null)
            //{
                //VisualSettingsControlView.ChangeNotificationOpacity = new Action<double>
                //    (opacity =>
                //    {
                //        Settings.Default.NotificationOpacity = opacity;
                //        NotificationOpacity = opacity;
                 //   });
            //}

            //send message to status app log
            //if (ScratchControlView.AppLogStatusBarAction == null)
            //{
                //ScratchControlView.AppLogStatusBarAction = new Action<string>(log => AppLogStatusBar = log);
            //}

            //send message to status complile log
            //if (ScratchControlView.CompileLogStatusBarAction == null)
            //{
                //ScratchControlView.CompileLogStatusBarAction = new Action<string>(log => CompileLogStatusBar = log);
            //}

            //start process ring
            //if (ScratchControlView.ProgressRingStartAction == null)
            //{
                //ScratchControlView.ProgressRingStartAction = new Action<bool>(start => ProgressRingStart = start);
            //}

            //if (ScriptEditorControlView.AppLogStatusBarAction == null)
            //{
                //ScriptEditorControlView.AppLogStatusBarAction = new Action<string>(log => AppLogStatusBar = log);
            //}
        }

        #endregion

        #endregion













    }
}
