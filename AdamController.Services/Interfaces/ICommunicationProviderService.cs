using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void TcpCientConnected(object sender);
    public delegate void TcpClientDisconnect(object sender);
    public delegate void TcpClientReconnected(object sender, int reconnectCounter);
    public delegate void UdpServerReceived(object sender, string message);
    public delegate void UdpClientReceived(object sender, string message);

    #endregion

    /// <summary>
    /// ComunicateHeleper functional
    /// </summary>
    public interface ICommunicationProviderService : IDisposable
    {
        #region Events

        public event TcpCientConnected RaiseTcpCientConnected;
        public event TcpClientDisconnect RaiseTcpClientDisconnect;
        public event TcpClientReconnected RaiseTcpClientReconnected;
        public event UdpServerReceived RaiseUdpServerReceived;
        public event UdpClientReceived RaiseUdpClientReceived;



        #endregion

        #region Public fields

        public bool IsTcpClientConnected { get; }

        #endregion

        #region Public methods

        public void ConnectAllAsync();
        public void DisconnectAllAsync();


        /// <summary>
        /// Left for backward compatibility
        /// This is where events were unsubscribed, and instances of client classes were destroyed
        /// This is now implemented in Dispose services
        /// All calls to this method should be replaced with calls to DisconnectAllAsync()
        /// </summary>
        //public void DisconnectAllAndDestroy();

        public void WebSocketSendTextMessage(string message);

        #endregion
    }
}
