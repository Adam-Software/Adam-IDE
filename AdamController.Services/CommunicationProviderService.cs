using AdamController.Services.Interfaces;
using System.Threading.Tasks;

namespace AdamController.Services
{
    public class CommunicationProviderService : ICommunicationProviderService
    {
        #region Events

        public event AdamTcpCientConnected RaiseAdamTcpCientConnected;
        public event AdamTcpClientDisconnect RaiseAdamTcpClientDisconnect;
        public event AdamTcpClientReconnected RaiseAdamTcpClientReconnected;
        public event AdamUdpServerReceived RaiseAdamUdpServerReceived;
        public event AdamUdpMessageReceived RaiseAdamUdpMessageReceived;

        #endregion

        #region Services

        private readonly IAdamTcpClientService mAdamTcpClientService;

        #endregion

        public CommunicationProviderService(IAdamTcpClientService adamTcpClientService)
        {
            mAdamTcpClientService = adamTcpClientService;

            Subscribe();
        }

        #region Public methods

        public void ConnectAllAsync()
        {
            mAdamTcpClientService.ConnectAsync();
            //_ = Task.Run();
            //_ = Task.Run(() => mAdamUdpMessageClient?.Start());
            //_ = Task.Run(() => mAdamWebSocketClient?.ConnectAsync());
        }

        public void DisconnectAllAsync()
        {
            _ = Task.Run(mAdamTcpClientService.DisconnectAndStop);
            //_ = Task.Run(() => mAdamUdpMessageClient?.Stop());
            //_ = Task.Run(() => mAdamWebSocketClient?.DisconnectAsync());
        }

        public void DisconnectAllAndDestroy() {}

        public void Dispose()
        {
            DisconnectAllAsync();
            Unsubscribe();
        }

        #endregion

        #region Private methods

        #endregion

        #region Subscriptions

        private void Subscribe()
        {
            mAdamTcpClientService.RaiseTcpClientReconnected += RaiseTcpClientReconnected;
            mAdamTcpClientService.RaiseTcpCientConnected += RaiseTcpCientConnected;
            mAdamTcpClientService.RaiseTcpClientDisconnected += RaiseTcpClientDisconnected;
        }

        private void Unsubscribe()
        {
            mAdamTcpClientService.RaiseTcpClientReconnected -= RaiseTcpClientReconnected;
            mAdamTcpClientService.RaiseTcpCientConnected -= RaiseTcpCientConnected;
            mAdamTcpClientService.RaiseTcpClientDisconnected -= RaiseTcpClientDisconnected;
        }

        #endregion

        #region Event methods

        private void RaiseTcpCientConnected(object sender)
        {
            //OnAdamTcpConnectedEvent?.Invoke();
            OnRaiseAdamTcpCientConnected();

            //if (!mAdamUdpLogServer.IsStarted)
            //    mAdamUdpLogServer?.Start();

            //if (!mAdamWebSocketClient.IsStarted)
            //mAdamWebSocketClient.ConnectAsync();
        }

        private void RaiseTcpClientDisconnected(object sender)
        {
            //OnAdamTcpDisconnectedEvent?.Invoke();
            OnRaiseAdamTcpClientDisconnect();

            //if (mAdamUdpLogServer != null)
            //{
            //    if (mAdamUdpLogServer.IsStarted)
            //        mAdamUdpLogServer?.Stop();
            //}

            //if (mAdamWebSocketClient != null)
            //{
                //if (mAdamWebSocketClient.IsRunning)
            //    mAdamWebSocketClient.DisconnectAsync();
            //}
        }

        private void RaiseTcpClientReconnected(object sender, int reconnectCount)
        {
            //OnAdamTcpReconnected?.Invoke(reconnectCount);
            OnRaiseAdamTcpClientReconnected(reconnectCount);
        }

        #endregion

        #region OnRaise events

        protected virtual void OnRaiseAdamTcpCientConnected()
        {
            AdamTcpCientConnected raiseEvent = RaiseAdamTcpCientConnected;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaiseAdamTcpClientDisconnect()
        {
            AdamTcpClientDisconnect raiseEvent = RaiseAdamTcpClientDisconnect;
            raiseEvent?.Invoke(this);
        }

        public virtual void OnRaiseAdamTcpClientReconnected(int reconnectCounter)
        {
            AdamTcpClientReconnected raiseEvent = RaiseAdamTcpClientReconnected;
            raiseEvent?.Invoke(this, reconnectCounter);
        }

        protected virtual void OnRaiseAdamUdpServerReceived()
        {
            AdamUdpServerReceived raiseEvent = RaiseAdamUdpServerReceived;
            raiseEvent?.Equals(this);
        }

        protected virtual void OnRaiseAdamUdpMessageReceived(string message)
        {
            AdamUdpMessageReceived raiseEvent = RaiseAdamUdpMessageReceived;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}
