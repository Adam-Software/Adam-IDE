using System;
using System.Net.Sockets;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void TcpCientConnected(object sender);
    public delegate void TcpCientSent(object sender, long sent, long pending);
    public delegate void TcpClientDisconnect(object sender);
    public delegate void TcpClientError(object sender, SocketError error);
    public delegate void TcpClientReceived(object sender, byte[] buffer, long offset, long size);
    public delegate void TcpClientReconnected(object sender, int reconnectCount);

    #endregion

    public interface ITcpClientService : IDisposable
    {
        
        #region Events

        public event TcpCientConnected RaiseTcpCientConnected;
        public event TcpCientSent RaiseTcpCientSent;
        public event TcpClientDisconnect RaiseTcpClientDisconnected;
        public event TcpClientError RaiseTcpClientError;
        public event TcpClientReceived RaiseTcpClientReceived;
        public event TcpClientReconnected RaiseTcpClientReconnected;

        #endregion

        /// <summary>
        /// The number of reconnections when the connection is lost
        /// </summary>
        public int ReconnectCount { get; }

        /// <summary>
        /// Reconnection timeout
        /// </summary>
        public int ReconnectTimeout { get; }

        public void DisconnectAndStop();

        /// <summary>
        /// This method is implemented in NetCoreServer.TcpClient
        /// </summary>
        public bool ConnectAsync();
    }
}
