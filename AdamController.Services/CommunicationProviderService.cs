using AdamController.Services.Interfaces;
using System.Net;
using System.Text;
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
        private readonly IAdamUdpClientService mAdamUdpClientService;

        #endregion

        public CommunicationProviderService(IAdamTcpClientService adamTcpClientService, IAdamUdpClientService adamUdpClientService)
        {
            mAdamTcpClientService = adamTcpClientService;
            mAdamUdpClientService = adamUdpClientService;

            Subscribe();
        }

        #region Public methods

        public void ConnectAllAsync()
        {
            
            _ = Task.Run(mAdamTcpClientService.ConnectAsync);
            _ = Task.Run(mAdamUdpClientService.Start);
            //_ = Task.Run(() => mAdamWebSocketClient?.ConnectAsync());
        }

        public void DisconnectAllAsync()
        {
            _ = Task.Run(mAdamTcpClientService.DisconnectAndStop);
            _ = Task.Run(mAdamUdpClientService.Stop);
            //_ = Task.Run(() => mAdamWebSocketClient?.DisconnectAsync());
        }

        public void DisconnectAllAndDestroy() {}

        public void Dispose()
        {
            DisconnectAllAsync();
            Unsubscribe();

            mAdamTcpClientService.Dispose();
            mAdamUdpClientService.Dispose();
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

            mAdamUdpClientService.RaiseUdpClientReceived += RaiseUdpClientReceived;
        }

        private void Unsubscribe()
        {
            mAdamTcpClientService.RaiseTcpClientReconnected -= RaiseTcpClientReconnected;
            mAdamTcpClientService.RaiseTcpCientConnected -= RaiseTcpCientConnected;
            mAdamTcpClientService.RaiseTcpClientDisconnected -= RaiseTcpClientDisconnected;

            mAdamUdpClientService.RaiseUdpClientReceived -= RaiseUdpClientReceived;
        }

        #endregion

        #region Event methods

        private void RaiseTcpCientConnected(object sender)
        {
            //OnAdamTcpConnectedEvent?.Invoke();
            OnRaiseAdamTcpCientConnected();

            //if (!mAdamUdpLogServer.IsStarted)
            //    mAdamUdpLogServer?.Start();

            //if(!mAdamUdpClientService.IsStarted)
            mAdamUdpClientService.Start();

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

            //if(mAdamUdpClientService.IsStarted)
            mAdamUdpClientService.Stop();

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

        private void RaiseUdpClientReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseAdamUdpMessageReceived(encodedMessage);
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
