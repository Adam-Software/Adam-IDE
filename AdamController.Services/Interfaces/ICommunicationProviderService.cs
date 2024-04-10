using System;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void AdamTcpCientConnected(object sender);
    public delegate void AdamTcpClientDisconnect(object sender);
    public delegate void AdamTcpClientReconnected(object sender, int reconnectCounter);
    public delegate void AdamUdpServerReceived(object sender, string message);
    public delegate void AdamUdpMessageReceived(object sender, string message);

    #endregion

    /// <summary>
    /// ComunicateHeleper functional
    /// </summary>
    public interface ICommunicationProviderService : IDisposable
    {
        #region Events

        public event AdamTcpCientConnected RaiseAdamTcpCientConnected;
        public event AdamTcpClientDisconnect RaiseAdamTcpClientDisconnect;
        public event AdamTcpClientReconnected RaiseAdamTcpClientReconnected;
        public event AdamUdpServerReceived RaiseAdamUdpServerReceived;
        public event AdamUdpMessageReceived RaiseAdamUdpMessageReceived;

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
        public void DisconnectAllAndDestroy();

        #endregion
    }
}
