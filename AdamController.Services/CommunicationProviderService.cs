using AdamController.Services.Interfaces;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdamController.Services
{
    public class CommunicationProviderService : ICommunicationProviderService
    {
        #region Events

        public event TcpCientConnected RaiseTcpCientConnected;
        public event TcpClientDisconnect RaiseTcpClientDisconnect;
        public event TcpClientReconnected RaiseTcpClientReconnected;
        public event UdpServerReceived RaiseUdpServerReceived;
        public event UdpClientReceived RaiseUdpClientReceived;

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

        //public void DisconnectAllAndDestroy() {}

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
            mTcpClientService.RaiseTcpClientReconnected += RaiseTcpClientReconnected;
            mTcpClientService.RaiseTcpCientConnected += RaiseTcpCientConnected;
            mTcpClientService.RaiseTcpClientDisconnected += RaiseTcpClientDisconnected;
            mUdpClientService.RaiseUdpClientReceived += RaiseUdpClientReceived;
            mUdpServerService.RaiseUdpServerReceived += RaiseUdpServerReceived;
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

            OnRaiseTcpCientConnected();

            mUdpClientService.Start();
            mUdpServerService.Start();
            mWebSocketClientService.ConnectAsync();
        }

        private void RaiseTcpClientDisconnected(object sender)
        {
            IsTcpClientConnected = false;

            OnRaiseTcpClientDisconnect();

            mUdpClientService.Stop();
            mUdpServerService.Stop();
            mWebSocketClientService.DisconnectAsync();
        }

        private void RaiseServiceTcpClientReconnected(object sender, int reconnectCount)
        {
            OnRaiseTcpClientReconnected(reconnectCount);
        }

        private void RaiseServiceUdpClientReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer);
            //string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseUdpClientReceived(encodedMessage);
        }


        private void RaiseServiceUdpServerReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer);
            //string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnRaiseUdpServerReceived(encodedMessage);
        }

        #endregion

        #region OnRaise events

        protected virtual void OnRaiseTcpCientConnected()
        {
            TcpCientConnected raiseEvent = RaiseTcpCientConnected;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaiseTcpClientDisconnect()
        {
            TcpClientDisconnect raiseEvent = RaiseTcpClientDisconnect;
            raiseEvent?.Invoke(this);
        }

        public virtual void OnRaiseTcpClientReconnected(int reconnectCounter)
        {
            TcpClientReconnected raiseEvent = RaiseTcpClientReconnected;
            raiseEvent?.Invoke(this, reconnectCounter);
        }

        protected virtual void OnRaiseUdpServerReceived(string message)
        {
            UdpServerReceived raiseEvent = RaiseUdpServerReceived;
            raiseEvent?.Invoke(this, message);
        }

        protected virtual void OnRaiseUdpClientReceived(string message)
        {
            UdpClientReceived raiseEvent = RaiseUdpClientReceived;
            raiseEvent?.Invoke(this, message);
        }

        #endregion
    }
}
