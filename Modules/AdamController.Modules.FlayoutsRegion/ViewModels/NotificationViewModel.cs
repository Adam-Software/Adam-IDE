using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.Helpers;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using AdamController.WebApi.Client.v1;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class NotificationViewModel : FlyoutBase
    {
        #region DelegateCommands

        public DelegateCommand ConnectButtonComand { get; }
        public DelegateCommand ClearNotificationsCommand { get;  }

        #endregion

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
            SetFlyoutParametr();

            mCommunicationProvider = communicationProvider;

            ConnectButtonComand = new (ConnectButton, ConnectButtonCanExecute);
            ClearNotificationsCommand = new DelegateCommand(ClearNotifications, ClearNotificationsCanExecute);
        }

        #endregion

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {
            //need update status bar on opening
            if(isOpening)
                Subscribe();

            if (!isOpening)
                Unsubscribe();

            base.OnChanging(isOpening);
        }

        #endregion

        #region Private methods

        private void SetFlyoutParametr()
        {
            Theme = FlyoutTheme.Inverse;
            Header = "Центр уведомлений";
            IsModal = false;
        }

        #endregion

        #region Subscription

        private void Subscribe()
        {
            mCommunicationProvider.RaiseTcpServiceCientConnected += OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientReconnected += OnRaiseTcpServiceClientReconnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnect += OnRaiseTcpServiceClientDisconnect;
            
        }

        private void Unsubscribe() 
        {
            mCommunicationProvider.RaiseTcpServiceCientConnected -= OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientReconnected -= OnRaiseTcpServiceClientReconnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnect -= OnRaiseTcpServiceClientDisconnect;
        }

        #endregion

        #region Event methods

        private void OnRaiseTcpServiceCientConnected(object sender)
        {
            // это должно быть не здесь
            _ = BaseApi.StopPythonExecute();

            TextOnConnectFlayotButton = cConnectButtonStatusConnected;
            //TextOnStatusConnectToolbar = mToolbarStatusClientConnected;

            //ConnectIcon = PackIconModernKind.Disconnect;
            IconOnConnectFlayoutButton = PackIconMaterialKind.Robot;
            //throw new NotImplementedException();
        }

        private void OnRaiseTcpServiceClientReconnected(object sender, int reconnectCounter)
        {
            TextOnConnectFlayotButton = $"{cConnectButtonStatusReconnected} {reconnectCount}";
            //TextOnStatusConnectToolbar = $"{mToolbarStatusClientReconnected} {reconnectCount}";

            //ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotConfused;
        }

        private int reconnectCount = 0;

        private void OnRaiseTcpServiceClientDisconnect(object sender)
        {
            //если центр уведомлений закрыт, обновляем счетчик уведомлений
            if (!IsOpen && Settings.Default.IsMessageShowOnAbortMainConnection)
            {
                //BadgeCounter++;
                FailConnectMessageVisibility = Visibility.Visible;
            }

            TextOnConnectFlayotButton = cConnectButtonStatusDisconnected;
            //TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

            //ConnectIcon = PackIconModernKind.Connect;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
        }

        #endregion

        #region Delegatecommand methods

        private void ConnectButton()
        {
            //bool isNotifyButton = false; //(string)obj == "IsNotificationButtonCalling";

            //if (isNotifyButton)
            //{
                //ClearNotifications();
                //NotificationFlayoutsIsOpen = false;
            //}

            //await Dispatcher.Yield(DispatcherPriority.Normal);

            if (mCommunicationProvider.IsTcpClientConnected)
            {
                mCommunicationProvider.DisconnectAllAsync();
                //return;
            }

            if (!mCommunicationProvider.IsTcpClientConnected)
            {
                mCommunicationProvider.ConnectAllAsync();
                //return;
            }
        }

        private bool ConnectButtonCanExecute()
        {
            return true;
        }

        /* #16 */
        private void ClearNotifications()
        {
            //BadgeCounter = 0;
            FailConnectMessageVisibility = Visibility.Collapsed;
        }

        private bool ClearNotificationsCanExecute()
        {
            return true;
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

        #region Connect/Disconnect button (Flayouts)

        private string textOnConnectFlayotButton = cConnectButtonStatusDisconnected;
        public string TextOnConnectFlayotButton
        {
            get => textOnConnectFlayotButton;
            set => SetProperty(ref textOnConnectFlayotButton, value);
        }

        private PackIconMaterialKind iconOnConnectFlayoutButton = PackIconMaterialKind.Robot;
        public PackIconMaterialKind IconOnConnectFlayoutButton
        {
            get => iconOnConnectFlayoutButton;
            set => SetProperty(ref iconOnConnectFlayoutButton, value);
            
        }

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
    }




}
