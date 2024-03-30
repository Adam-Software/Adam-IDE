using AdamController.Core.Helpers;
using AdamController.Core.Mvvm;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class NotificationViewModel : RegionViewModelBase
    {

        #region Const

        private const string mConnectButtonStatusDisconnected = "Подключить";
        private const string mConnectButtonStatusConnected = "Отключить";
        private const string mConnectButtonStatusReconnected = "Подождите";

        #endregion

        public NotificationViewModel(IRegionManager regionManager, IDialogService dialogService) : base(regionManager, dialogService)
        {
        }

        #region Fields

        private bool mNotificationFlayoutsIsOpen;
        public bool NotificationFlayoutsIsOpen
        {
            get { return mNotificationFlayoutsIsOpen; }
            set
            {
                if (mNotificationFlayoutsIsOpen == value)
                    return;

                mNotificationFlayoutsIsOpen = value;

                SetProperty(ref mNotificationFlayoutsIsOpen, value);
            }
        }

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

        #region NotificationBadge

        private void ClearNotification()
        {
            //BadgeCounter = 0;
            FailConnectMessageVisibility = Visibility.Collapsed;
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

        #endregion
    }




}
