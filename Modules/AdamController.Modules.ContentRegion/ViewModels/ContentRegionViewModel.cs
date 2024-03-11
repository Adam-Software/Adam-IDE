using AdamBlocklyLibrary.Enum;
using AdamController.Core.DataSource;
using AdamController.Core.Helpers;
using AdamController.Core.Model;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.WebApi.Client.v1;
using MahApps.Metro.IconPacks;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System;
using System.Linq;
using Prism.Commands;
using System.Windows.Threading;
using System.Security.AccessControl;

namespace AdamController.Modules.ContentRegion.ViewModels
{
    public class ContentRegionViewModel : RegionViewModelBase
    {

        #region Const

        private const string mConnectButtonStatusDisconnected = "Подключить";
        private const string mConnectButtonStatusConnected = "Отключить";
        private const string mConnectButtonStatusReconnected = "Подождите";

        #endregion

        public ContentRegionViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            
        }

        #region Navigation

        #endregion

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

        private int selectedPageIndex = 0;
        public int SelectedPageIndex
        {
            get => selectedPageIndex;
            set
            {
                if (value == selectedPageIndex)
                {
                    return;
                }

                selectedPageIndex = value;
                GetSelectedPageIndex = value;

                SetProperty(ref value, selectedPageIndex);
            }
        }


        public static int GetSelectedPageIndex { get; private set; }

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

            TextOnConnectFlayotButton = mConnectButtonStatusDisconnected;
            //TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

            //ConnectIcon = PackIconModernKind.Connect;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
        }

        private void OnTcpConnected()
        {
            _ = BaseApi.StopPythonExecute();

            TextOnConnectFlayotButton = mConnectButtonStatusConnected;
            //TextOnStatusConnectToolbar = mToolbarStatusClientConnected;

            //ConnectIcon = PackIconModernKind.Disconnect;
            IconOnConnectFlayoutButton = PackIconMaterialKind.Robot;
        }

        private void OnTcpReconnected(int reconnectCount)
        {
            TextOnConnectFlayotButton = $"{mConnectButtonStatusReconnected} {reconnectCount}";
            //TextOnStatusConnectToolbar = $"{mToolbarStatusClientReconnected} {reconnectCount}";

            //ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotConfused;
        }

        #endregion

        #region InitAction

        private void InitAction()
        {
            //move to page with selected index
            if (ScratchControlView.SetSelectedPageIndex == null)
            {
                ScratchControlView.SetSelectedPageIndex = new Action<int>(index => SelectedPageIndex = index);
            }

            //open flayout page
            if (VisualSettingsControlView.OpenAdvancedBlocklySettings == null)
            {
                VisualSettingsControlView.OpenAdvancedBlocklySettings = new Action<bool>(open => AdvancedBlocklySettingsFlayoutsIsOpen = open);
            }

            //change toolbox lang settings
            if (VisualSettingsControlView.SetToolboxLanguage == null)
            {
                VisualSettingsControlView.SetToolboxLanguage = new Action<BlocklyLanguageModel>(model => SelectedBlocklyToolboxLanguage = model);
            }

            //settings blockly theme
            if (VisualSettingsControlView.SetBlocklyThemeAndGridColor == null)
            {
                VisualSettingsControlView.SetBlocklyThemeAndGridColor = new Action<BlocklyThemeModel>
                    (theme =>
                    {
                        SelectedBlocklyTheme = theme;

                        if (Settings.Default.ChangeGridColorSwitchToggleSwitchState) return;

                        SelectGridColorDependingSelectedTheme(theme.BlocklyTheme);
                    });
            }

            if (VisualSettingsControlView.ChangeNotificationOpacity == null)
            {
                VisualSettingsControlView.ChangeNotificationOpacity = new Action<double>
                    (opacity =>
                    {
                        Settings.Default.NotificationOpacity = opacity;
                        NotificationOpacity = opacity;
                    });
            }

            //send message to status app log
            if (ScratchControlView.AppLogStatusBarAction == null)
            {
                //ScratchControlView.AppLogStatusBarAction = new Action<string>(log => AppLogStatusBar = log);
            }

            //send message to status complile log
            if (ScratchControlView.CompileLogStatusBarAction == null)
            {
                //ScratchControlView.CompileLogStatusBarAction = new Action<string>(log => CompileLogStatusBar = log);
            }

            //start process ring
            if (ScratchControlView.ProgressRingStartAction == null)
            {
                //ScratchControlView.ProgressRingStartAction = new Action<bool>(start => ProgressRingStart = start);
            }

            if (ScriptEditorControlView.AppLogStatusBarAction == null)
            {
                //ScriptEditorControlView.AppLogStatusBarAction = new Action<string>(log => AppLogStatusBar = log);
            }
        }

        #endregion

        #region BlocklyTheme Settings

        public static ObservableCollection<BlocklyThemeModel> BlocklyThemes { get; private set; } = ThemesCollection.BlocklyThemes;

        private BlocklyThemeModel selectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == Settings.Default.BlocklyTheme);
        public BlocklyThemeModel SelectedBlocklyTheme
        {
            get => selectedBlocklyTheme;
            set
            {
                if (value == selectedBlocklyTheme)
                {
                    return;
                }

                selectedBlocklyTheme = value;
                SetProperty(ref selectedBlocklyTheme, value);

                Settings.Default.BlocklyTheme = selectedBlocklyTheme.BlocklyTheme;

                if (Settings.Default.ChangeGridColorSwitchToggleSwitchState) return;
                SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
            }
        }

        #endregion

        #region BlocklyToolboxLanguage Settings

        public static ObservableCollection<BlocklyLanguageModel> BlocklyLanguageCollection { get; private set; } = LanguagesCollection.BlocklyLanguageCollection;

        private BlocklyLanguageModel selectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyToolboxLanguage);
        public BlocklyLanguageModel SelectedBlocklyToolboxLanguage
        {
            get => selectedBlocklyToolboxLanguage;
            set
            {
                if (value == selectedBlocklyToolboxLanguage)
                {
                    return;
                }

                selectedBlocklyToolboxLanguage = value;

                SetProperty(ref selectedBlocklyToolboxLanguage, value);

                Settings.Default.BlocklyToolboxLanguage = selectedBlocklyToolboxLanguage.BlocklyLanguage;
            }
        }

        #endregion

        #region BlocklyGridColour settings

        private Color? selectedBlocklyGridColour = MahApps.Metro.Controls.ColorHelper.ColorFromString(Settings.Default.BlocklyGridColour);
        public Color? SelectedBlocklyGridColour
        {
            get => selectedBlocklyGridColour;
            set
            {
                if (value == selectedBlocklyGridColour)
                {
                    return;
                }
                selectedBlocklyGridColour = value;

                SetProperty(ref selectedBlocklyGridColour, value);
                Settings.Default.BlocklyGridColour = selectedBlocklyGridColour.ToString();
            }
        }

        #endregion

        #region Opened flayout page

        #region Open BlocklySettingsFlayots

        private bool advancedBlocklySettingsFlayoutsIsOpen;
        public bool AdvancedBlocklySettingsFlayoutsIsOpen
        {
            get { return advancedBlocklySettingsFlayoutsIsOpen; }
            set
            {
                if (value == advancedBlocklySettingsFlayoutsIsOpen) return;

                advancedBlocklySettingsFlayoutsIsOpen = value;
                SetProperty(ref advancedBlocklySettingsFlayoutsIsOpen, value);
            }
        }

        #endregion



        #endregion

        #region NotificationMessage Visiblity

        private Visibility noNewNotificationMessageVisibility = Visibility.Visible;
        public Visibility NoNewNotificationMessageVisibility
        {
            get => noNewNotificationMessageVisibility;
            set
            {
                if (value == noNewNotificationMessageVisibility) return;

                noNewNotificationMessageVisibility = value;
                SetProperty(ref noNewNotificationMessageVisibility, value);

            }
        }

        private Visibility failConnectMessageVisibility = Visibility.Collapsed;
        public Visibility FailConnectMessageVisibility
        {
            get => failConnectMessageVisibility;
            set
            {
                if (value == failConnectMessageVisibility) return;

                if (value == Visibility.Visible)
                    NoNewNotificationMessageVisibility = Visibility.Collapsed;
                if (value == Visibility.Collapsed)
                    NoNewNotificationMessageVisibility = Visibility.Visible;

                failConnectMessageVisibility = value;
                SetProperty(ref failConnectMessageVisibility, value);
            }
        }

        #endregion

        #region NotificationBadge




        private void ClearNotification()
        {
            //BadgeCounter = 0;
            FailConnectMessageVisibility = Visibility.Collapsed;
        }

        #endregion

        #region NotificationOpacity

        private double notificationOpacity = Settings.Default.NotificationOpacity;
        public double NotificationOpacity
        {
            get => notificationOpacity;
            set
            {
                if (value == notificationOpacity) return;

                notificationOpacity = value;

                SetProperty(ref notificationOpacity, value);
            }
        }

        #endregion

        #region Connect/Disconnect button (Flayouts)

        private string textOnConnectFlayotButton;
        public string TextOnConnectFlayotButton
        {
            get => textOnConnectFlayotButton;
            set
            {
                if (value == textOnConnectFlayotButton) return;

                textOnConnectFlayotButton = value;
                SetProperty(ref textOnConnectFlayotButton, value);
            }
        }

        private PackIconMaterialKind iconOnConnectFlayoutButton;
        public PackIconMaterialKind IconOnConnectFlayoutButton
        {
            get => iconOnConnectFlayoutButton;
            set
            {
                if (value == iconOnConnectFlayoutButton) return;

                iconOnConnectFlayoutButton = value;
                SetProperty(ref iconOnConnectFlayoutButton, value);
            }
        }

        #endregion

        #region Connect/Disconnect toolbar


        private DelegateCommand connectButtonComand;
        public DelegateCommand ConnectButtonComand => connectButtonComand ??= new DelegateCommand(async () =>
        {
            bool isNotifyButton = false; //(string)obj == "IsNotificationButtonCalling";
            if (isNotifyButton)
            {
                ClearNotification();
                //NotificationFlayoutsIsOpen = false;
            }

            await Dispatcher.Yield(DispatcherPriority.Normal);


            if (ComunicateHelper.TcpClientIsConnected)
            {
                ComunicateHelper.DisconnectAll();
                return;
            }

            if (!ComunicateHelper.TcpClientIsConnected)
            {
                ComunicateHelper.ConnectAllAsync();
                return;
            }
        });

        #endregion

        #region SelectMainTheme

        private void SelectGridColorDependingSelectedTheme(BlocklyTheme theme)
        {
            switch (theme)
            {
                case BlocklyTheme.Dark:
                    {
                        SelectedBlocklyGridColour = Colors.White;
                        break;
                    }
                case BlocklyTheme.Classic:
                    {
                        SelectedBlocklyGridColour = Colors.Black;
                        break;
                    }
                default:
                    {
                        SelectedBlocklyGridColour = Colors.Black;
                        break;
                    }
            }
        }

        #endregion

        #region Commands

        private DelegateCommand clearNotifications;
        public DelegateCommand ClearNotifications => clearNotifications ??= new DelegateCommand(() =>
        {
            ClearNotification();
        });

        private DelegateCommand closeNotificationFlayots;
        public DelegateCommand CloseNotificationFlayots => closeNotificationFlayots ??= new DelegateCommand(() =>
        {
            //NotificationFlayoutsIsOpen = false;
        });



        private DelegateCommand<bool?> changeGridColorToggleSwitchCommand;
        public DelegateCommand<bool?> ChangeGridColorToggleSwitchCommand => changeGridColorToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
        });

        private DelegateCommand<bool?> changeSpacingToggleSwitchCommand;
        public DelegateCommand<bool?> ChangeSpacingToggleSwitchCommand => changeSpacingToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            Settings.Default.BlocklyGridSpacing = 20;
        });

        private DelegateCommand enableShowGridCommand;
        public DelegateCommand EnableShowGridCommand => enableShowGridCommand ??= new DelegateCommand(() =>
        {
            Settings.Default.BlocklyShowGrid = true;
        });

        private DelegateCommand<bool?> changeBlocklyThemeToogleSwitchCommand;
        public DelegateCommand<bool?> ChangeBlocklyThemeToogleSwitchCommand => changeBlocklyThemeToogleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            if (Settings.Default.BaseTheme == "Dark")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark);
            }
            else if (Settings.Default.BaseTheme == "Light")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic);
            }
        });

        private DelegateCommand<bool?> changeToolboxLanguageToggleSwitchCommand;
        public DelegateCommand<bool?> ChangeToolboxLanguageToggleSwitchCommand => changeToolboxLanguageToggleSwitchCommand ??= new DelegateCommand<bool?>(obj =>
        {
            bool? toogleSwitchState = obj;

            if (toogleSwitchState == true) return;

            SelectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Settings.Default.BlocklyWorkspaceLanguage);

        });

        #endregion

    }
}
