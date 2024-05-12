﻿using System;
using System.Net.Sockets;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void TcpCientConnectedEventHandler(object sender);
    public delegate void TcpCientSentEventHandler(object sender, long sent, long pending);
    public delegate void TcpClientDisconnectEventHandler(object sender);
    public delegate void TcpClientErrorEventHandler(object sender, SocketError error);
    public delegate void TcpClientReceivedEventHandler(object sender, byte[] buffer, long offset, long size);
    public delegate void TcpClientReconnectedEventHandler(object sender, int reconnectCount);

    #endregion

    public interface ITcpClientService : IDisposable
    {
        
        #region Events

        public event TcpCientConnectedEventHandler RaiseTcpCientConnectedEvent;
        public event TcpCientSentEventHandler RaiseTcpCientSentEvent;
        public event TcpClientDisconnectEventHandler RaiseTcpClientDisconnectedEvent;
        public event TcpClientErrorEventHandler RaiseTcpClientErrorEvent;
        public event TcpClientReceivedEventHandler RaiseTcpClientReceivedEvent;
        public event TcpClientReconnectedEventHandler RaiseTcpClientReconnectedEvent;

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
