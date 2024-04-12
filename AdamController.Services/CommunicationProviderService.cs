using AdamController.Services.Interfaces;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdamController.Services
{
    public class CommunicationProviderService : ICommunicationProviderService
    {
        #region Events

        public event TcpServiceCientConnected RaiseTcpServiceCientConnected;
        public event TcpServiceClientDisconnect RaiseTcpServiceClientDisconnect;
        public event TcpServiceClientReconnected RaiseTcpServiceClientReconnected;
        public event UdpServiceServerReceived RaiseUdpServiceServerReceived;
        public event UdpServiceClientReceived RaiseUdpServiceClientReceived;

        #endregion

        #region Services

        private readonly ITcpClientService mTcpClientService;
        private readonly IUdpClientService mUdpClientService;
        private readonly IUdpServerService mUdpServerService;
        private readonly IWebSocketClientService mWebSocketClientService;

        #endregion

        #region ~

        public CommunicationProviderService(ITcpClientService adamTcpClientService, IUdpClientService adamUdpClientService, 
            IUdpServerService adamUdpServerService, IWebSocketClientService adamWebSocketClientService)
        {
            mTcpClientService = adamTcpClientService;
            mUdpClientService = adamUdpClientService;
            mUdpServerService = adamUdpServerService;
            mWebSocketClientService = adamWebSocketClientService;

            Subscribe();
        }

        #endregion

        #region Public fields

        public bool IsTcpClientConnected { get; private set; }

        #endregion

        #region Public methods

        public void ConnectAllAsync()
        {
            _ = Task.Run(mTcpClientService.ConnectAsync);
            _ = Task.Run(mUdpClientService.Start);
            _ = Task.Run(mUdpServerService.Start);
            _ = Task.Run(mWebSocketClientService.ConnectAsync);
        }

        public void DisconnectAllAsync()
        {
            _ = Task.Run(mTcpClientService.DisconnectAndStop);
            _ = Task.Run(mUdpClientService.Stop);
            _ = Task.Run(mUdpServerService.Stop);
            _ = Task.Run(mWebSocketClientService.DisconnectAsync);
        }

        public void WebSocketSendTextMessage(string message)
        {
            Task.Run(() => mWebSocketClientService.SendTextAsync(message));
        }

        public void Dispose()
        {
            DisconnectAllAsync();
            Unsubscribe();

            mTcpClientService.Dispose();
            mUdpClientService.Dispose();
            mUdpServerService.Dispose();
            mWebSocketClientService.Dispose();
        }

        #endregion

        #region Private methods

        #endregion

        #region Subscriptions

        private void Subscribe()
        {
            mTcpClientService.RaiseTcpClientReconnected += RaiseServiceTcpClientReconnected;
            mTcpClientService.RaiseTcpCientConnected += RaiseServiceTcpCientConnected;
            mTcpClientService.RaiseTcpClientDisconnected += RaiseTcpClientDisconnected;
            mUdpClientService.RaiseUdpClientReceived += RaiseServiceUdpClientReceived;
            mUdpServerService.RaiseUdpServerReceived += RaiseServiceUdpServerReceived;
        }

        private void Unsubscribe()
        {
            mTcpClientService.RaiseTcpClientReconnected -= RaiseServiceTcpClientReconnected;
            mTcpClientService.RaiseTcpCientConnected -= RaiseServiceTcpCientConnected;
            mTcpClientService.RaiseTcpClientDisconnected -= RaiseTcpClientDisconnected;
            mUdpClientService.RaiseUdpClientReceived -= RaiseServiceUdpClientReceived;
            mUdpServerService.RaiseUdpServerReceived -= RaiseServiceUdpServerReceived;
        }

        #endregion

        #region Event methods

        private void RaiseServiceTcpCientConnected(object sender)
        {
            IsTcpClientConnected = true;

            OnRaiseTcpServiceCientConnected();

            mUdpClientService.Start();
            mUdpServerService.Start();
            mWebSocketClientService.ConnectAsync();
        }

        private void RaiseTcpClientDisconnected(object sender)
        {
            IsTcpClientConnected = false;

            OnRaiseTcpServiceClientDisconnect();

            mUdpClientService.Stop();
            mUdpServerService.Stop();
            mWebSocketClientService.DisconnectAsync();
        }

        private void RaiseServiceTcpClientReconnected(object sender, int reconnectCount)
        {
            OnRaiseTcpServiceClientReconnected(reconnectCount);
        }

        private void RaiseServiceUdpClientReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            //string encodedMessage = Encoding.UTF8.GetString(buffer);
            string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseUdpServiceClientReceived(encodedMessage);
        }


        private void RaiseServiceUdpServerReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            //string encodedMessage = Encoding.UTF8.GetString(buffer);
            string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseUdpServiceServerReceived(encodedMessage);
        }

        #endregion

        #region OnRaise events

        protected virtual void OnRaiseTcpServiceCientConnected()
        {
            TcpServiceCientConnected raiseEvent = RaiseTcpServiceCientConnected;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaiseTcpServiceClientDisconnect()
        {
            TcpServiceClientDisconnect raiseEvent = RaiseTcpServiceClientDisconnect;
            raiseEvent?.Invoke(this);
        }

        public virtual void OnRaiseTcpServiceClientReconnected(int reconnectCounter)
        {
            TcpServiceClientReconnected raiseEvent = RaiseTcpServiceClientReconnected;
            raiseEvent?.Invoke(this, reconnectCounter);
        }

        protected virtual void OnRaiseUdpServiceServerReceived(string message)
        {
            UdpServiceServerReceived raiseEvent = RaiseUdpServiceServerReceived;
            raiseEvent?.Invoke(this, message);
        }

        protected virtual void OnRaiseUdpServiceClientReceived(string message)
        {
            UdpServiceClientReceived raiseEvent = RaiseUdpServiceClientReceived;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}
