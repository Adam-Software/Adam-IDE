using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void TcpServiceCientConnectedEventHandler(object sender);
    public delegate void TcpServiceClientDisconnectEventHandler(object sender);
    public delegate void TcpServiceClientReconnectedEventHandler(object sender, int reconnectCounter);
    public delegate void UdpServiceServerReceivedEventHandler(object sender, string message);
    public delegate void UdpServiceClientReceivedEventHandler(object sender, string message);

    #endregion

    /// <summary>
    /// ComunicateHeleper functional
    /// </summary>
    public interface ICommunicationProviderService : IDisposable
    {
        #region Events

        public event TcpServiceCientConnectedEventHandler RaiseTcpServiceCientConnectedEvent;
        public event TcpServiceClientDisconnectEventHandler RaiseTcpServiceClientDisconnectEvent;
        public event TcpServiceClientReconnectedEventHandler RaiseTcpServiceClientReconnectedEvent;
        public event UdpServiceServerReceivedEventHandler RaiseUdpServiceServerReceivedEvent;
        public event UdpServiceClientReceivedEventHandler RaiseUdpServiceClientReceivedEvent;

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
