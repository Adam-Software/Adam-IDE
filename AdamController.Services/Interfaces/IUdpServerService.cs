
using System;
using System.Net;

namespace AdamController.Services.Interfaces
{

    #region Delegate

    public delegate void UdpServerReceived(object sender, EndPoint endpoint, byte[] buffer, long offset, long size);

    #endregion

    public interface IUdpServerService : IDisposable
    {
        #region Events

        public event UdpServerReceived RaiseUdpServerReceived;

        #endregion

        #region Public fields

        public bool IsStarted { get; }

        #endregion

        #region Public methods

        public bool Start();

        public bool Stop();

        #endregion

    }
}
