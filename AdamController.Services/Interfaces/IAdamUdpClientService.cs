using System;
using System.Net;
using System.Net.Sockets;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void UdpClientReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size);

    #endregion

    public interface IAdamUdpClientService :  IDisposable
    {
        #region Events

        public event UdpClientReceived RaiseUdpClientReceived;

        #endregion

        public bool IsStarted { get; }

        public bool Stop();

        public bool Start();

        //public void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size);

        //public void OnSent(EndPoint endpoint, long sent);

        //public void OnError(SocketError error);
    }
}
