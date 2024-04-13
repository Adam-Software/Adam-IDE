using AdamController.Controls.CustomControls.Mvvm.FlyoutContainer;
using AdamController.Core.Properties;
using AdamController.Services;
using AdamController.Services.Interfaces;
using AdamController.WebApi.Client.v1;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using System.Windows;

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
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotificationDeliveryService;

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

            ConnectButtonComand = new (ConnectButton, ConnectButtonCanExecute);
            ClearNotificationsCommand = new DelegateCommand(ClearNotifications, ClearNotificationsCanExecute);

            Subscribe();
        }

        #endregion

        #region Navigation

        protected override void OnChanging(bool isOpening)
        {
            //need update status bar on opening
            if (isOpening)
            {
                Subscribe();
                SetStatusConnection(mCommunicationProvider.IsTcpClientConnected);
            }
                

            //if (!isOpening)
                //Unsubscribe();

            base.OnChanging(isOpening);
        }

        

        #endregion

        #region Private methods

        private void SetFlyoutParametrs()
        {
            Theme = FlyoutTheme.Inverse;
            Header = "Центр уведомлений";
            IsModal = false;
        }

        private void SetStatusConnection(bool connectionStatus)
        {
            if (connectionStatus)
            {
                // это должно быть не здесь
                _ = BaseApi.StopPythonExecute();

                TextOnConnectFlayotButton = cConnectButtonStatusConnected;
                //TextOnStatusConnectToolbar = mToolbarStatusClientConnected;

                //ConnectIcon = PackIconModernKind.Disconnect;
                IconOnConnectFlayoutButton = PackIconMaterialKind.Robot;
                //throw new NotImplementedException();
            }

            if(!connectionStatus)
            {
                //если центр уведомлений закрыт, обновляем счетчик уведомлений
                if (Settings.Default.IsMessageShowOnAbortMainConnection)
                {
                    mStatusBarNotificationDeliveryService.NotificationCounter++;

                    if (!IsOpen)
                        FailConnectMessageVisibility = Visibility.Visible;
                    
                }

                TextOnConnectFlayotButton = cConnectButtonStatusDisconnected;
                //TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

                //ConnectIcon = PackIconModernKind.Connect;
                IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
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
            mStatusBarNotificationDeliveryService.NotificationCounter = reconnectCounter;
            TextOnConnectFlayotButton = $"{cConnectButtonStatusReconnected} {reconnectCounter}";
            //TextOnStatusConnectToolbar = $"{mToolbarStatusClientReconnected} {reconnectCount}";

            //ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            IconOnConnectFlayoutButton = PackIconMaterialKind.RobotConfused;
        }

        //private int reconnectCount = 0;

        private void OnRaiseTcpServiceClientDisconnect(object sender)
        {
            SetStatusConnection(false); 
        }

        #endregion

        #region DelegateCommands methods

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
            mStatusBarNotificationDeliveryService.NotificationCounter = 0;

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
                bool isNewValue = SetProperty(ref failConnectMessageVisibility, value);
                
                if (isNewValue)
                {
                    if (FailConnectMessageVisibility == Visibility.Visible)
                        NoNewNotificationMessageVisibility = Visibility.Collapsed;

                    if (FailConnectMessageVisibility == Visibility.Collapsed)
                        NoNewNotificationMessageVisibility = Visibility.Visible;
                }
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

        #region Events TCP/IP clients OLD

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
