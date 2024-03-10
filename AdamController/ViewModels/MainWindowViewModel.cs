using AdamBlocklyLibrary.Enum;
using AdamController.Commands;
using AdamController.Core.DataSource;
using AdamController.Core.Helpers;
using AdamController.Core.Model;
using AdamController.Core.Properties;
using AdamController.Modules.ContentRegion.ViewModels;
using AdamController.Services;
using AdamController.WebApi.Client.v1;
using MahApps.Metro.IconPacks;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AdamController.ViewModels
{
    public class MainWindowViewModel : BindableBase//BaseViewModel
    {
        #region Const

        private const string mToolbarStatusClientDisconnected = "Робот Адам: отключен";
        private const string mToolbarStatusClientConnected = "Робот Адам: подключен";
        private const string mToolbarStatusClientReconnected = "Робот Адам: переподключение";

        private const string mConnectButtonStatusDisconnected = "Подключить";
        private const string mConnectButtonStatusConnected = "Отключить";
        private const string mConnectButtonStatusReconnected = "Подождите";

        #endregion

        #region Services

        private Services.ITestService mTestService;

        #endregion

        #region Fields

        public string WindowTitle => $"Adam IDE {Assembly.GetExecutingAssembly().GetName().Version}";

        #endregion

        public MainWindowViewModel(ITestService testService)
        {

            mTestService = testService;


            ComunicateHelper.OnAdamTcpConnectedEvent += OnTcpConnected;
            ComunicateHelper.OnAdamTcpDisconnectedEvent += OnTcpDisconnected;
            ComunicateHelper.OnAdamTcpReconnected += OnTcpReconnected;
            ComunicateHelper.OnAdamLogServerUdpReceivedEvent += ComunicateHelperOnAdamUdpReceived;
            

            InitAction();

            if (Settings.Default.AutoStartTcpConnect)
            {
                ConnectButtonComand.Execute(null);
            }
            else
            {
                //init fields if autorun off
                TextOnConnectFlayotButton = mConnectButtonStatusDisconnected;
                TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

                ConnectIcon = PackIconModernKind.Connect;
                IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
            }
        }

        private void ComunicateHelperOnAdamUdpReceived(string message)
        {
            try
            {
                SyslogMessage syslogMessage = SyslogParseMessage.Parse(message);
                CompileLogStatusBar = $"{syslogMessage.TimeStamp:T} {syslogMessage.Message}";
            }
            catch(Exception ex)
            {
                CompileLogStatusBar = $"Error reading udp log with exception {ex.Message}";
            }
        }

        #region SelectedPageIndex

        private int selectedPageIndex = 0;
        public int SelectedPageIndex
        {
            get => selectedPageIndex;
            set
            {
                if(value == selectedPageIndex)
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
            if(!NotificationFlayoutsIsOpen && Settings.Default.IsMessageShowOnAbortMainConnection)
            {
                BadgeCounter++;
                FailConnectMessageVisibility = Visibility.Visible;
            }

            TextOnConnectFlayotButton = mConnectButtonStatusDisconnected;
            TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;
            
            ConnectIcon = PackIconModernKind.Connect;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
        }

        private void OnTcpConnected()
        {
            _ = BaseApi.StopPythonExecute();

            TextOnConnectFlayotButton = mConnectButtonStatusConnected;
            TextOnStatusConnectToolbar = mToolbarStatusClientConnected;
            
            ConnectIcon = PackIconModernKind.Disconnect;
            IconOnConnectFlayoutButton = PackIconMaterialKind.Robot;
        }

        private void OnTcpReconnected(int reconnectCount)
        {
            TextOnConnectFlayotButton = $"{mConnectButtonStatusReconnected} {reconnectCount}";
            TextOnStatusConnectToolbar = $"{mToolbarStatusClientReconnected} {reconnectCount}";
            
            ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotConfused;
        }

        #endregion

        #region InitAction

        private void InitAction()
        {
            //move to page with selected index
            if(ScratchControlView.SetSelectedPageIndex == null)
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
                ScratchControlView.AppLogStatusBarAction = new Action<string>(log => AppLogStatusBar = log);
            }

            //send message to status complile log
            if (ScratchControlView.CompileLogStatusBarAction == null)
            {
                ScratchControlView.CompileLogStatusBarAction = new Action<string>(log => CompileLogStatusBar = log);
            }

            //start process ring
            if (ScratchControlView.ProgressRingStartAction == null)
            {
                ScratchControlView.ProgressRingStartAction = new Action<bool>(start => ProgressRingStart = start);
            }

            if(ScriptEditorControlView.AppLogStatusBarAction == null)
            {
                ScriptEditorControlView.AppLogStatusBarAction = new Action<string>(log => AppLogStatusBar = log);
            }            
        }

        #endregion

        #region AppStatusBar field

        private string appLogStatusBar = "Лог приложения";
        public string AppLogStatusBar
        {
            get => appLogStatusBar;
            set
            {
                if(value == appLogStatusBar)
                {
                    return;
                }

                appLogStatusBar = value;
                SetProperty(ref appLogStatusBar, value);
            }
        }

        #endregion

        #region CompileLogStatusBar field

        private string compileLogStatusBar = "Лог робота";
        public string CompileLogStatusBar
        {
            get => compileLogStatusBar;
            set
            {
                if(value == compileLogStatusBar)
                {
                    return;
                }

                compileLogStatusBar = value;
                SetProperty(ref compileLogStatusBar, value);
            }
        }

        #endregion

        #region ProgressRing field

        private bool progressRingStart = false;
        public bool ProgressRingStart
        {
            get => progressRingStart;
            set
            {
                if (value == progressRingStart) return;

                progressRingStart = value;
                SetProperty(ref progressRingStart, value);
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

        #region Open NotificationFlayouts

        private bool notificationFlayoutsIsOpen;
        public bool NotificationFlayoutsIsOpen
        {
            get { return notificationFlayoutsIsOpen; }
            set
            {
                if (value == notificationFlayoutsIsOpen) return;

                notificationFlayoutsIsOpen = value;
                SetProperty (ref notificationFlayoutsIsOpen, value);
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

        private int badgeCounter = 0;
        private int BadgeCounter
        {
            get => badgeCounter;
            set
            {
                if (value == badgeCounter) return;
                if (value == 0)
                {
                    badgeCounter = value;
                    NotificationBadge = "";
                    return;
                }

                badgeCounter = value;

                NotificationBadge = $"{BadgeCounter}";
            }
        }

        private string notificationBadge;
        public string NotificationBadge
        {
            get => notificationBadge;
            set
            {
                if (value == notificationBadge) return;

                notificationBadge = value;
                SetProperty(ref notificationBadge, value);
            }
        }

        private void ClearNotification()
        {
            BadgeCounter = 0;
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

        private string textOnStatusConnectToolbar;
        public string TextOnStatusConnectToolbar
        {
            get => textOnStatusConnectToolbar;
            set
            {
                if (value == textOnStatusConnectToolbar) return;

                textOnStatusConnectToolbar = value;
                SetProperty (ref textOnStatusConnectToolbar, value);
            }
        }

        private PackIconModernKind connectIcon;
        public PackIconModernKind ConnectIcon
        {
            get => connectIcon;
            set
            {
                if (value == connectIcon) return;

                connectIcon = value;
                SetProperty(ref connectIcon, value);
            }
        }

        private RelayCommand connectButtonComand;
        public RelayCommand ConnectButtonComand => connectButtonComand ??= new RelayCommand(async obj =>
        {
            bool isNotifyButton = (string)obj == "IsNotificationButtonCalling";
            if (isNotifyButton)
            {
                ClearNotification();
                NotificationFlayoutsIsOpen = false;
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

        private RelayCommand clearNotifications;
        public RelayCommand ClearNotifications => clearNotifications ??= new RelayCommand(obj =>
        {
             ClearNotification();
        });

        private RelayCommand closeNotificationFlayots;
        public RelayCommand CloseNotificationFlayots => closeNotificationFlayots ??= new RelayCommand(obj => 
        {
            NotificationFlayoutsIsOpen = false;
        });

        private RelayCommand<bool> openNotificationPanel;
        public RelayCommand<bool> OpenNotificationPanel => openNotificationPanel ??= new RelayCommand<bool>(obj =>
        {
            NotificationFlayoutsIsOpen = !NotificationFlayoutsIsOpen;
        });

        private RelayCommand<bool> changeGridColorToggleSwitchCommand;
        public RelayCommand<bool> ChangeGridColorToggleSwitchCommand => changeGridColorToggleSwitchCommand ??= new RelayCommand<bool>(obj => 
        {
            bool toogleSwitchState = obj;

            if (toogleSwitchState) return;

            SelectGridColorDependingSelectedTheme(SelectedBlocklyTheme.BlocklyTheme);
        });

        private RelayCommand<bool> changeSpacingToggleSwitchCommand;
        public RelayCommand<bool> ChangeSpacingToggleSwitchCommand => changeSpacingToggleSwitchCommand ??= new RelayCommand<bool>(obj =>
        {
            bool toogleSwitchState = obj;

            if (toogleSwitchState) return;

            Core.Properties.Settings.Default.BlocklyGridSpacing = 20;
        });

        private RelayCommand enableShowGridCommand;
        public RelayCommand EnableShowGridCommand => enableShowGridCommand ??= new RelayCommand(obj =>
        {
            Settings.Default.BlocklyShowGrid = true;
        });

        private RelayCommand<bool> changeBlocklyThemeToogleSwitchCommand;
        public RelayCommand<bool> ChangeBlocklyThemeToogleSwitchCommand => changeBlocklyThemeToogleSwitchCommand ??= new RelayCommand<bool>(obj =>
        {
            bool toogleSwitchState = obj;

            if (toogleSwitchState) return;

            if (Settings.Default.BaseTheme == "Dark")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Dark);
            }
            else if (Settings.Default.BaseTheme == "Light")
            {
                SelectedBlocklyTheme = BlocklyThemes.FirstOrDefault(x => x.BlocklyTheme == BlocklyTheme.Classic);
            }
        });

        private RelayCommand<bool> changeToolboxLanguageToggleSwitchCommand;
        public RelayCommand<bool> ChangeToolboxLanguageToggleSwitchCommand => changeToolboxLanguageToggleSwitchCommand ??= new RelayCommand<bool>(obj =>
        {
            bool toogleSwitchState = obj;

            if (toogleSwitchState) return;

            SelectedBlocklyToolboxLanguage = BlocklyLanguageCollection.FirstOrDefault(x => x.BlocklyLanguage == Core.Properties.Settings.Default.BlocklyWorkspaceLanguage);

        });

        #endregion

        #region IWindowParam

        //public override string WindowTitle => $"Adam IDE {Assembly.GetExecutingAssembly().GetName().Version}";
        //public override bool IsModal => false;
        //public override WindowState WindowState => WindowState.Maximized;

        //public override void OnClosed(Window window)
        //{
        //    ComunicateHelper.DisconnectAllAndDestroy();

        //    base.OnClosed(window);
        //}

        #endregion
    }
}
