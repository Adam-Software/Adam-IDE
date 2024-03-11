using AdamController.Core.Mvvm;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdamController.Modules.StatusBar.ViewModels
{
    public class StatusBarViewModel : RegionViewModelBase
    {
        private const string mToolbarStatusClientDisconnected = "Робот Адам: отключен";
        private const string mToolbarStatusClientConnected = "Робот Адам: подключен";
        private const string mToolbarStatusClientReconnected = "Робот Адам: переподключение";

        public StatusBarViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
            
        }

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

        #region CompileLogStatusBar field

        private string compileLogStatusBar = "Лог робота";
        public string CompileLogStatusBar
        {
            get => compileLogStatusBar;
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

        #endregion

        #region AppStatusBar field

        private string appLogStatusBar = "Лог приложения";
        public string AppLogStatusBar
        {
            get => appLogStatusBar;
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

        #endregion

        private PackIconModernKind connectIcon = PackIconModernKind.Connect;
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


        private string textOnStatusConnectToolbar;
        public string TextOnStatusConnectToolbar
        {
            get => textOnStatusConnectToolbar;
            set
            {
                if (value == textOnStatusConnectToolbar) return;

                textOnStatusConnectToolbar = value;
                SetProperty(ref textOnStatusConnectToolbar, value);
            }
        }

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

        #region Open NotificationFlayouts

        private bool notificationFlayoutsIsOpen;
        public bool NotificationFlayoutsIsOpen
        {
            get { return notificationFlayoutsIsOpen; }
            set
            {
                if (value == notificationFlayoutsIsOpen) return;

                notificationFlayoutsIsOpen = value;
                SetProperty(ref notificationFlayoutsIsOpen, value);
            }
        }

        #endregion

        #region Commands

        private DelegateCommand<bool?> openNotificationPanel;
        public DelegateCommand<bool?> OpenNotificationPanel => openNotificationPanel ??= new DelegateCommand<bool?>(obj =>
        {
            NotificationFlayoutsIsOpen = !NotificationFlayoutsIsOpen;
        });

        #endregion
    }
}
