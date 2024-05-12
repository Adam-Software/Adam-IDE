
using System;
using System.Net;

namespace AdamController.Services.Interfaces
{

    #region Delegate

    public delegate void UdpServerReceivedEventHandler(object sender, EndPoint endpoint, byte[] buffer, long offset, long size);

    #endregion

    public interface IUdpServerService : IDisposable
    {
        #region Events

        public event UdpServerReceivedEventHandler RaiseUdpServerReceivedEvent;

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
