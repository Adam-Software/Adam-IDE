using AdamController.Controls.CustomControls.Services;
using AdamController.Core;
using AdamController.Core.Mvvm;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.StatusBarRegion.ViewModels
{
    public class StatusBarViewModel : RegionViewModelBase
    {
        #region DelegateCommands

        public DelegateCommand OpenNotificationPanelDelegateCommand { get; }

        #endregion

        #region Services

        private readonly IFlyoutManager mFlyoutManager;

        #endregion

        #region Const

        private const string cTextOnStatusConnectToolbarDisconnected = "Робот Адам: отключен";
        private const string cTextOnStatusConnectToolbarConnected = "Робот Адам: подключен";
        private const string cTextOnStatusConnectToolbarReconnected = "Робот Адам: переподключение";

        private const string cCompileLogStatusBar = "Лог робота";
        private const string cAppLogStatusBar = "Лог приложения";

        #endregion

        #region ~

        public StatusBarViewModel(IRegionManager regionManager, IDialogService dialogService, IFlyoutManager flyoutManager) : base(regionManager, dialogService)
        {
            mFlyoutManager = flyoutManager;

            OpenNotificationPanelDelegateCommand = new DelegateCommand(OpenNotificationPanel, OpenNotificationPanelCanExecute);
        }

        #endregion

        #region public fields

        private bool progressRingStart;
        public bool ProgressRingStart
        {
            get { return progressRingStart; }
            set
            {
                if (value == progressRingStart) return;

                progressRingStart = value;
                SetProperty(ref progressRingStart, value);
            }
        }

        private string compileLogStatusBar = cCompileLogStatusBar;
        public string CompileLogStatusBar
        {
            get { return compileLogStatusBar; }
            set
            {
                if (value == compileLogStatusBar)
                {
                    return;
                }

                compileLogStatusBar = value;
                SetProperty(ref compileLogStatusBar, value);
            }
        }

        private string appLogStatusBar = cAppLogStatusBar;
        public string AppLogStatusBar
        {
            get { return appLogStatusBar; }
            set
            {
                if (value == appLogStatusBar)
                {
                    return;
                }

                appLogStatusBar = value;
                SetProperty(ref appLogStatusBar, value);
            }
        }

        private PackIconModernKind connectIcon = PackIconModernKind.Connect;
        public PackIconModernKind ConnectIcon
        {
            get { return connectIcon; }
            set
            {
                if (value == connectIcon) return;

                connectIcon = value;
                SetProperty(ref connectIcon, value);
            }
        }

        private string textOnStatusConnectToolbar = cTextOnStatusConnectToolbarDisconnected;
        public string TextOnStatusConnectToolbar
        {
            get { return textOnStatusConnectToolbar; }
            set
            {
                if (value == textOnStatusConnectToolbar) return;

                textOnStatusConnectToolbar = value;
                SetProperty(ref textOnStatusConnectToolbar, value);
            }
        }

        private string notificationBadge;
        public string NotificationBadge
        {
            get { return notificationBadge; }
            set
            {
                if (value == notificationBadge) return;

                notificationBadge = value;
                SetProperty(ref notificationBadge, value);
            }
        }

        #endregion

        #region Private fields

        private int badgeCounter = 0;
        private int BadgeCounter
        {
            get { return badgeCounter; }
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

        #endregion

        #region DelegateCommands methods

        private void OpenNotificationPanel()
        {
            mFlyoutManager.OpenFlyout(FlyoutNames.FlyoutNotification);
        }

        private bool OpenNotificationPanelCanExecute()
        {
            return true;
        }

        #endregion

    }
}
