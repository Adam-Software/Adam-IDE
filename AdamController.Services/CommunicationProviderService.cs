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
        public event AdamUdpClientReceived RaiseAdamUdpClientReceived;

        #endregion

        #region Services

        private readonly ITcpClientService mAdamTcpClientService;
        private readonly IUdpClientService mAdamUdpClientService;
        private readonly IUdpServerService mAadamUdpServerService;
        private readonly IWebSocketClientService mAdamWebSocketClientService;

        #endregion

        #region ~

        public CommunicationProviderService(ITcpClientService adamTcpClientService, IUdpClientService adamUdpClientService, 
            IUdpServerService adamUdpServerService, IWebSocketClientService adamWebSocketClientService)
        {
            mAdamTcpClientService = adamTcpClientService;
            mAdamUdpClientService = adamUdpClientService;
            mAadamUdpServerService = adamUdpServerService;
            mAdamWebSocketClientService = adamWebSocketClientService;

            Subscribe();
        }

        #endregion

        #region Public fields

        public bool IsTcpClientConnected { get; private set; }

        #endregion

        #region Public methods

        public void ConnectAllAsync()
        {
            _ = Task.Run(mAdamTcpClientService.ConnectAsync);
            _ = Task.Run(mAdamUdpClientService.Start);
            _ = Task.Run(mAadamUdpServerService.Start);
            _ = Task.Run(mAdamWebSocketClientService.ConnectAsync);
        }

        public void DisconnectAllAsync()
        {
            _ = Task.Run(mAdamTcpClientService.DisconnectAndStop);
            _ = Task.Run(mAdamUdpClientService.Stop);
            _ = Task.Run(mAadamUdpServerService.Stop);
            _ = Task.Run(mAdamWebSocketClientService.DisconnectAsync);
        }

        public void DisconnectAllAndDestroy() {}

        public void WebSocketSendTextMessage(string message)
        {
            Task.Run(() => mAdamWebSocketClientService.SendTextAsync(message));
        }

        public void Dispose()
        {
            DisconnectAllAsync();
            Unsubscribe();

            mAdamTcpClientService.Dispose();
            mAdamUdpClientService.Dispose();
            mAadamUdpServerService.Dispose();
            mAdamWebSocketClientService.Dispose();
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
            mAadamUdpServerService.RaiseUdpServerReceived += RaiseUdpServerReceived;
        }

        private void Unsubscribe()
        {
            mAdamTcpClientService.RaiseTcpClientReconnected -= RaiseTcpClientReconnected;
            mAdamTcpClientService.RaiseTcpCientConnected -= RaiseTcpCientConnected;
            mAdamTcpClientService.RaiseTcpClientDisconnected -= RaiseTcpClientDisconnected;
            mAdamUdpClientService.RaiseUdpClientReceived -= RaiseUdpClientReceived;
            mAadamUdpServerService.RaiseUdpServerReceived -= RaiseUdpServerReceived;
        }

        #endregion

        #region Event methods

        private void RaiseTcpCientConnected(object sender)
        {
            IsTcpClientConnected = true;

            OnRaiseAdamTcpCientConnected();

            mAdamUdpClientService.Start();
            mAadamUdpServerService.Start();
            mAdamWebSocketClientService.ConnectAsync();
        }

        private void RaiseTcpClientDisconnected(object sender)
        {
            IsTcpClientConnected = false;

            OnRaiseAdamTcpClientDisconnect();

            mAdamUdpClientService.Stop();
            mAadamUdpServerService.Stop();
            mAdamWebSocketClientService.DisconnectAsync();
        }

        private void RaiseTcpClientReconnected(object sender, int reconnectCount)
        {
            OnRaiseAdamTcpClientReconnected(reconnectCount);
        }

        private void RaiseUdpClientReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer);
            //string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseAdamUdpClientReceived(encodedMessage);
        }


        private void RaiseUdpServerReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer);
            //string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseAdamUdpServerReceived(encodedMessage);
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

        protected virtual void OnRaiseAdamUdpServerReceived(string message)
        {
            AdamUdpServerReceived raiseEvent = RaiseAdamUdpServerReceived;
            raiseEvent?.Invoke(this, message);
        }

        protected virtual void OnRaiseAdamUdpClientReceived(string message)
        {
            AdamUdpClientReceived raiseEvent = RaiseAdamUdpClientReceived;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}
