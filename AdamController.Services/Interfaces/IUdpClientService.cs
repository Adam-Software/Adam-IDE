using AdamController.Services.UdpClientServiceDependency;
using System;
using System.Net;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    public delegate void UdpClientMessageEnqueueEventHandler(object sender, ReceivedData data);

    #endregion

    public interface IUdpClientService :  IDisposable
    {
        #region Events

        public event UdpClientMessageEnqueueEventHandler RaiseUdpClientMessageEnqueueEvent;

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
