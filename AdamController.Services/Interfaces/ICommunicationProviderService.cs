using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void TcpServiceCientConnected(object sender);
    public delegate void TcpServiceClientDisconnect(object sender);
    public delegate void TcpServiceClientReconnected(object sender, int reconnectCounter);
    public delegate void UdpServiceServerReceived(object sender, string message);
    public delegate void UdpServiceClientReceived(object sender, string message);

    #endregion

    /// <summary>
    /// ComunicateHeleper functional
    /// </summary>
    public interface ICommunicationProviderService : IDisposable
    {
        #region Events

        public event TcpServiceCientConnected RaiseTcpServiceCientConnected;
        public event TcpServiceClientDisconnect RaiseTcpServiceClientDisconnect;
        public event TcpServiceClientReconnected RaiseTcpServiceClientReconnected;
        public event UdpServiceServerReceived RaiseUdpServiceServerReceived;
        public event UdpServiceClientReceived RaiseUdpServiceClientReceived;



        #endregion

        #region Public fields

        public bool IsTcpClientConnected { get; }

        #endregion

        #region Public methods

        public void ConnectAllAsync();
        public void DisconnectAllAsync();
        public void WebSocketSendTextMessage(string message);

        #endregion
    }
}
