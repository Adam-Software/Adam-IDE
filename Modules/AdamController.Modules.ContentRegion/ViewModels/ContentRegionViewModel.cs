using AdamController.Core;
using AdamController.Core.Extensions;
using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using AdamController.Modules.ContentRegion.Views;
using AdamController.Services.Interfaces;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ContentRegionViewModel : RegionViewModelBase
    {
        private ISubRegionChangeAwareService RegionChangeAwareService { get; }

        public ContentRegionViewModel(IRegionManager regionManager, IDialogService dialogService, ISubRegionChangeAwareService regionChangeAwareService) : base(regionManager, dialogService)
        {
            RegionChangeAwareService = regionChangeAwareService;
        }

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (navigationContext.NavigationService.Region.Name == RegionNames.ContentRegion)
            {
                string insideRegionName = navigationContext.Uri.OriginalString;

                RegionChangeAwareService.InsideRegionNavigationRequestName = insideRegionName;
                SubRegionsRequestNavigate(insideRegionName, navigationContext.Parameters);
            }

            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion

        #region Private methods

        private void SubRegionsRequestNavigate(string uri, NavigationParameters parameters)
        {
            if (string.IsNullOrEmpty(uri))
                return;

            
            switch (uri)
            {
                case SubRegionNames.SubRegionScratch:
                    
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(ScratchControlView), parameters);
                    break;

                case SubRegionNames.SubRegionScriptEditor:
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(ScriptEditorControlView), parameters);
                    break;

                case SubRegionNames.SubRegionComputerVisionControl:
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(ComputerVisionControlView), parameters);
                    break;

                case SubRegionNames.SubRegionVisualSettings:
                    RegionManager.RequestNavigate(SubRegionNames.InsideConentRegion, nameof(VisualSettingsControlView), parameters);
                    break;

            }
        }

        #endregion

        #region OLD

        private void ComunicateHelperOnAdamUdpReceived(string message)
        {
            try
            {
                SyslogMessage syslogMessage = message.Parse();
                // SyslogParseMessage.Parse(message);
                //CompileLogStatusBar = $"{syslogMessage.TimeStamp:T} {syslogMessage.Message}";
            }
            catch (Exception ex)
            {
                //CompileLogStatusBar = $"Error reading udp log with exception {ex.Message}";
            }
        }

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
