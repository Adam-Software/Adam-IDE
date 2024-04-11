using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.Helpers;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class NotificationViewModel : FlyoutBase
    {
        #region Services

        private readonly ICommunicationProviderService mCommunicationProvider;

        #endregion

        #region Const

        private const string cConnectButtonStatusDisconnected = "Подключить";
        private const string cConnectButtonStatusConnected = "Отключить";
        private const string cConnectButtonStatusReconnected = "Подождите";

        #endregion

        #region ~

        public NotificationViewModel(ICommunicationProviderService communicationProvider) 
        {
            mCommunicationProvider = communicationProvider;

            Theme = FlyoutTheme.Inverse;
            Header= "Центр уведомлений";
            IsModal = false;
        }

        #endregion

        #region NotificationMessage Visiblity

        private Visibility noNewNotificationMessageVisibility = Visibility.Visible;
        public Visibility NoNewNotificationMessageVisibility
        {
            get => noNewNotificationMessageVisibility;
            set => SetProperty(ref noNewNotificationMessageVisibility, value);
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
            set => SetProperty(ref notificationOpacity, value);
        }

        #endregion

        #region NotificationBadge
        /* #16 */
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
            set => SetProperty(ref textOnConnectFlayotButton, value);
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


            if (mCommunicationProvider.IsTcpClientConnected)
            {
                mCommunicationProvider.DisconnectAllAsync();
                return;
            }

            if (!mCommunicationProvider.IsTcpClientConnected)
            {
                mCommunicationProvider.ConnectAllAsync();
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

        #endregion
    }




}
