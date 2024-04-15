﻿using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using System.Windows;

namespace AdamController.Modules.FlayoutsRegion.ViewModels
{
    public class NotificationViewModel : FlyoutBase
    {
        #region DelegateCommands

        public DelegateCommand ConnectButtonDelegateCommand { get; private set; }
        public DelegateCommand ReconnectNotificationButtonDelegateCommand { get; private set; }
        public DelegateCommand ResetNotificationsDelegateCommand { get; private set; }

        #endregion

        #region Services

        private ICommunicationProviderService mCommunicationProvider;
        private IStatusBarNotificationDeliveryService mStatusBarNotificationDeliveryService;

        #endregion

        #region Const

        private const string cConnectButtonStatusDisconnected = "Подключить";
        private const string cConnectButtonStatusConnected = "Отключить";
        private const string cConnectButtonStatusReconnected = "Подождите";

        #endregion

        #region ~

        public NotificationViewModel(ICommunicationProviderService communicationProvider, IStatusBarNotificationDeliveryService statusBarNotificationDelivery) 
        {
            SetFlyoutParametrs();
            
            mCommunicationProvider = communicationProvider;
            mStatusBarNotificationDeliveryService = statusBarNotificationDelivery;

            ConnectButtonDelegateCommand = new (ConnectButton, ConnectButtonCanExecute);
            ReconnectNotificationButtonDelegateCommand = new (ReconnectNotificationButton, ReconnectNotificationButtonCanExecute);
            ResetNotificationsDelegateCommand = new (ResetNotifications, ResetNotificationsCanExecute);
        }

        #endregion

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {
            
            if (isOpening)
            {
                Subscribe();

                SetStatusConnection(mCommunicationProvider.IsTcpClientConnected);
            }
                

            if (!isOpening)
            {
                Unsubscribe();

                ConnectButtonDelegateCommand = null;
                ReconnectNotificationButtonDelegateCommand = null;
                ResetNotificationsDelegateCommand = null;
            }
                

            base.OnChanging(isOpening);
        }

        #endregion

        #region Public field

        private Visibility noNewNotificationMessageVisibility = Visibility.Visible;
        public Visibility NoNewNotificationMessageVisibility
        {
            get => noNewNotificationMessageVisibility;
            set => SetProperty(ref noNewNotificationMessageVisibility, value);
        }

        private Visibility failConnectNotificationVisibility = Visibility.Collapsed;
        public Visibility FailConnectNotificationVisibility
        {
            get => failConnectNotificationVisibility;
            set
            {
                bool isNewValue = SetProperty(ref failConnectNotificationVisibility, value);

                if (isNewValue)
                {
                    if (FailConnectNotificationVisibility == Visibility.Visible)
                        NoNewNotificationMessageVisibility = Visibility.Collapsed;

                    if (FailConnectNotificationVisibility == Visibility.Collapsed)
                        NoNewNotificationMessageVisibility = Visibility.Visible;
                }
            }
        }

        private string contentConnectButton = cConnectButtonStatusDisconnected;
        public string ContentConnectButton
        {
            get => contentConnectButton;
            set => SetProperty(ref contentConnectButton, value);
        }

        private PackIconMaterialKind iconConnectButton = PackIconMaterialKind.Robot;
        public PackIconMaterialKind IconConnectButton
        {
            get => iconConnectButton;
            set => SetProperty(ref iconConnectButton, value);

        }

        #endregion

        #region Private methods

        private void SetFlyoutParametrs()
        {
            Theme = FlyoutTheme.Inverse;
            Header = "Центр уведомлений";
            IsModal = false;
        }

        /// <summary>
        /// Controls the display of the connection status
        /// </summary>
        /// <param name="connectionStatus">true is connected, false disconetcted, null reconected</param>
        /// <param name="reconnectCounter"></param>
        private void SetStatusConnection(bool? connectionStatus, int reconnectCounter = 0)
        {
            if (connectionStatus == true)
            {
                ContentConnectButton = cConnectButtonStatusConnected;
                IconConnectButton = PackIconMaterialKind.Robot;
            }

            if(connectionStatus == false)
            {
                if (Settings.Default.IsMessageShowOnAbortMainConnection)
                {
                    if (!IsOpen)
                        FailConnectNotificationVisibility = Visibility.Visible;       
                }

                ContentConnectButton = cConnectButtonStatusDisconnected;
                IconConnectButton = PackIconMaterialKind.RobotDead;
            }

            if(connectionStatus == null)
            {
                ContentConnectButton = $"{cConnectButtonStatusReconnected} {reconnectCounter}";
                IconConnectButton = PackIconMaterialKind.RobotConfused;
            }
        }

        #endregion

        #region Subscription

        private void Subscribe()
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent += OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientReconnectedEvent += OnRaiseTcpServiceClientReconnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent += OnRaiseTcpServiceClientDisconnect;   
        }

        private void Unsubscribe() 
        {
            mCommunicationProvider.RaiseTcpServiceCientConnectedEvent -= OnRaiseTcpServiceCientConnected;
            mCommunicationProvider.RaiseTcpServiceClientReconnectedEvent -= OnRaiseTcpServiceClientReconnected;
            mCommunicationProvider.RaiseTcpServiceClientDisconnectEvent -= OnRaiseTcpServiceClientDisconnect;
        }

        #endregion

        #region Event methods

        private void OnRaiseTcpServiceCientConnected(object sender)
        {
            SetStatusConnection(true);   
        }

        private void OnRaiseTcpServiceClientReconnected(object sender, int reconnectCounter)
        {
            SetStatusConnection(null, reconnectCounter);
        }

        private void OnRaiseTcpServiceClientDisconnect(object sender)
        {
            SetStatusConnection(false); 
        }

        #endregion

        #region DelegateCommands methods

        private void ConnectButton()
        {
            bool isConnected = mCommunicationProvider.IsTcpClientConnected;

            if (isConnected)
            {
                mCommunicationProvider.DisconnectAllAsync();
                return;
            }
            
            mCommunicationProvider.ConnectAllAsync();
        }

        private bool ConnectButtonCanExecute()
        {
            return true;
        }

        private void ResetNotifications()
        {
            mStatusBarNotificationDeliveryService.ResetNotificationCounter();
            FailConnectNotificationVisibility = Visibility.Collapsed;
        }

        private bool ResetNotificationsCanExecute()
        {
            return true;
        }

        private void ReconnectNotificationButton()
        {
            bool isConnected = mCommunicationProvider.IsTcpClientConnected;

            if (!isConnected)
            {
                mCommunicationProvider.ConnectAllAsync();
                ResetNotifications();
            }
        }

        private bool ReconnectNotificationButtonCanExecute()
        {
            return true;
        }

        #endregion
    }




}
