using System;
using System.Net;
using System.Net.Sockets;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void UdpClientReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size);

    #endregion

    public interface IUdpClientService :  IDisposable
    {
        #region Events

        public event UdpClientReceived RaiseUdpClientReceived;

        #endregion

        #region Public fields

        public bool IsStarted { get; }

        #endregion

        #region Public methods

        public bool Stop();

        public bool Start();

        #endregion
    }
}
