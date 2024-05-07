using AdamController.Controls.CustomControls.Services;
using AdamController.Core;
using AdamController.Core.Mvvm;
using AdamController.Services.Interfaces;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using Prism.Regions;
using System;

namespace AdamController.Modules.StatusBarRegion.ViewModels
{
    public class StatusBarViewModel : RegionViewModelBase
    {
        #region DelegateCommands

        public DelegateCommand OpenNotificationPanelDelegateCommand { get; }

        #endregion

        #region Services

        private readonly IFlyoutManager mFlyoutManager;
        private readonly ICommunicationProviderService mCommunicationProviderService;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotificationDelivery;
        private readonly IFlyoutStateChecker mFlyoutState;
        private readonly ICultureProvider mCultureProvider;

        #endregion

        #region Const

        private string mTextOnStatusConnectToolbarDisconnected;
        private string mTextOnStatusConnectToolbarConnected;
        private string mTextOnStatusConnectToolbarReconnected;

        private string mCompileLogStatusBar;
        private string mAppLogStatusBar;

        #endregion

        #region ~

        public StatusBarViewModel(IRegionManager regionManager, IFlyoutManager flyoutManager, IFlyoutStateChecker flyoutState, ICommunicationProviderService communicationProviderService, 
                                    IStatusBarNotificationDeliveryService statusBarNotification, ICultureProvider cultureProvider) : base(regionManager)
        {
            mFlyoutManager = flyoutManager;
            mCommunicationProviderService = communicationProviderService;
            mStatusBarNotificationDelivery = statusBarNotification; 
            mFlyoutState = flyoutState;
            mCultureProvider = cultureProvider;

            OpenNotificationPanelDelegateCommand = new DelegateCommand(OpenNotificationPanel, OpenNotificationPanelCanExecute);

            LoadResource();
            LoadDefaultFieldValue();
        }

        #endregion

        #region DelegateCommands methods

        private void OpenNotificationPanel()
        {
            mFlyoutManager.OpenFlyout(FlyoutNames.FlyoutNotification);
        }

        private bool OpenNotificationPanelCanExecute()
        {
            return !mFlyoutState.IsNotificationFlyoutOpened;
        }

        #endregion

        #region Navigation

        public override void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            base.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Subscribe();

            base.OnNavigatedTo(navigationContext);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Unsubscribe();

            base.OnNavigatedFrom(navigationContext);
        }


        #endregion

        #region Public fields

        private bool progressRingStart;
        public bool ProgressRingStart
        {
            get { return progressRingStart; }
            set { SetProperty(ref progressRingStart, value); }
        }

        private string compileLogStatusBar; 
        public string CompileLogStatusBar
        {
            get { return compileLogStatusBar; }
            set { SetProperty(ref compileLogStatusBar, value); }
        }

        private string appLogStatusBar; 
        public string AppLogStatusBar
        {
            get { return appLogStatusBar; }
            set { SetProperty(ref appLogStatusBar, value); }
        }

        private PackIconModernKind connectIcon;
        public PackIconModernKind ConnectIcon
        {
            get { return connectIcon; }
            set { SetProperty(ref connectIcon, value); }
        }

        private string textOnStatusConnectToolbar;
        public string TextOnStatusConnectToolbar
        {
            get { return textOnStatusConnectToolbar; }
            set { SetProperty(ref textOnStatusConnectToolbar, value); }
        }

        private string notificationBadge;
        public string NotificationBadge
        {
            get { return notificationBadge; }
            set { SetProperty(ref notificationBadge, value); }
        }

        #endregion

        #region Private fields

        private int badgeCounter = 0;
        private int BadgeCounter
        {
            get { return badgeCounter; }
            set
            {
                var isNewValue = SetProperty(ref badgeCounter, value);

                if (isNewValue)
                    UpdateNotificationBagde();

                if (BadgeCounter == 0)
                    NotificationBadge = string.Empty;
            }
        }

        private bool? isConnected = false;
        public bool? IsConnected
        {
            get => isConnected;
            set
            {
                var isNewValue = SetProperty(ref isConnected, value);

                if (isNewValue)
                    UpdateStatusConnectToolbar();
            }

        }


        #endregion

        #region Private methods

        private void LoadDefaultFieldValue()
        {
            CompileLogStatusBar = mCompileLogStatusBar;
            AppLogStatusBar = mAppLogStatusBar;
            TextOnStatusConnectToolbar = mTextOnStatusConnectToolbarDisconnected;
            ConnectIcon = PackIconModernKind.Connect;

        }

        /// <summary>
        /// <code>BadgeCounter < 2</code>
        /// Restricts the notification counter so that it is not updated on repeated connections. 
        /// Only one notification will work. When the second one appears, they will need to be distinguished somehow.
        /// </summary>
        private void UpdateNotificationBagde()
        {
            if(BadgeCounter < 2)
                NotificationBadge = $"{BadgeCounter}";
        }

        private void LoadResource()
        {
            mTextOnStatusConnectToolbarDisconnected = mCultureProvider.FindResource("StatusBarViewModel.TextOnStatusConnectToolbarDisconnected");
            mTextOnStatusConnectToolbarConnected = mCultureProvider.FindResource("StatusBarViewModel.TextOnStatusConnectToolbarConnected");
            mTextOnStatusConnectToolbarReconnected = mCultureProvider.FindResource("StatusBarViewModel.TextOnStatusConnectToolbarReconnected");

            mCompileLogStatusBar = mCultureProvider.FindResource("StatusBarViewModel.CompileLogStatusBar");
            mAppLogStatusBar = mCultureProvider.FindResource("StatusBarViewModel.AppLogStatusBar");
        }

        private void UpdateStatusConnectToolbar()
        {
            if(IsConnected == true)
            {
                TextOnStatusConnectToolbar = mTextOnStatusConnectToolbarConnected;
                ConnectIcon = PackIconModernKind.Disconnect;
            }

            if (IsConnected == false)
            {
                TextOnStatusConnectToolbar = mTextOnStatusConnectToolbarDisconnected;
                ConnectIcon = PackIconModernKind.Connect;
            }
        }

        #endregion

        #region Subscribes

        private void Subscribe()
        {
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent += RaiseAdamTcpCientConnectedEvent;
            mCommunicationProviderService.RaiseTcpServiceClientDisconnectEvent += RaiseAdamTcpClientDisconnectEvent;
            mCommunicationProviderService.RaiseTcpServiceClientReconnectedEvent += RaiseTcpServiceClientReconnectedEvent;

            mStatusBarNotificationDelivery.RaiseChangeProgressRingStateEvent += RaiseChangeProgressRingStateEvent;
            mStatusBarNotificationDelivery.RaiseNewCompileLogMessageEvent += RaiseNewCompileLogMessageEvent;
            mStatusBarNotificationDelivery.RaiseNewAppLogMessageEvent += RaiseNewAppLogMessageEvent;
            mStatusBarNotificationDelivery.RaiseUpdateNotificationCounterEvent += RaiseUpdateNotificationCounterEvent;

            mFlyoutState.IsNotificationFlyoutOpenedStateChangeEvent += IsOpenedStateChangeEvent;

            mCultureProvider.RaiseCurrentAppCultureLoadOrChangeEvent += RaiseCurrentAppCultureLoadOrChangeEvent;
        }

        private void Unsubscribe() 
        {
            mCommunicationProviderService.RaiseTcpServiceCientConnectedEvent -= RaiseAdamTcpCientConnectedEvent;
            mCommunicationProviderService.RaiseTcpServiceClientDisconnectEvent -= RaiseAdamTcpClientDisconnectEvent;
            mCommunicationProviderService.RaiseTcpServiceClientReconnectedEvent -= RaiseTcpServiceClientReconnectedEvent;

            mStatusBarNotificationDelivery.RaiseChangeProgressRingStateEvent -= RaiseChangeProgressRingStateEvent;
            mStatusBarNotificationDelivery.RaiseNewCompileLogMessageEvent -= RaiseNewCompileLogMessageEvent;
            mStatusBarNotificationDelivery.RaiseNewAppLogMessageEvent -= RaiseNewAppLogMessageEvent;

            mFlyoutState.IsNotificationFlyoutOpenedStateChangeEvent -= IsOpenedStateChangeEvent;

            mCultureProvider.RaiseCurrentAppCultureLoadOrChangeEvent -= RaiseCurrentAppCultureLoadOrChangeEvent;
        }

        #endregion

        #region Event methods

        private void RaiseAdamTcpCientConnectedEvent(object sender)
        {
            IsConnected = true;
        }

        private void RaiseTcpServiceClientReconnectedEvent(object sender, int reconnectCounter)
        {
            mStatusBarNotificationDelivery.NotificationCounter++;
            ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            TextOnStatusConnectToolbar = $"{mTextOnStatusConnectToolbarReconnected} {reconnectCounter}";
        }

        private void RaiseAdamTcpClientDisconnectEvent(object sender, bool isUserRequest)
        {
           
            IsConnected = false;
            
            if(!isUserRequest)
                mStatusBarNotificationDelivery.NotificationCounter++;
        }

        private void RaiseChangeProgressRingStateEvent(object sender, bool newState)
        {
            ProgressRingStart = newState;
        }

        private void RaiseNewCompileLogMessageEvent(object sender, string message)
        {
            CompileLogStatusBar = message;
        }

        private void RaiseNewAppLogMessageEvent(object sender, string message)
        {
            AppLogStatusBar = message;
        }

        private void RaiseUpdateNotificationCounterEvent(object sender, int counter)
        {
            BadgeCounter = counter;
        }

        private void IsOpenedStateChangeEvent(object sender)
        {
            OpenNotificationPanelDelegateCommand.RaiseCanExecuteChanged();
        }

        private void RaiseCurrentAppCultureLoadOrChangeEvent(object sender)
        {
            LoadResource();
            UpdateStatusConnectToolbar();
        }

        #endregion

        
    }
}
