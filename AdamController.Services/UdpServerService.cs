using AdamStudio.Services.Interfaces;
using NetCoreServer;
using System.Net;

namespace AdamStudio.Services
{
    public class UdpServerService : UdpServer, IUdpServerService
    {
        #region Events

        public event UdpServerReceivedEventHandler RaiseUdpServerReceivedEvent;

        #endregion

        #region ~

        public UdpServerService(IPAddress address, int port) : base(address, port){}

        #endregion

        #region Private methods

        protected override void OnStarted()
        {
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            OnRaiseUdpServerReceivedEvent(endpoint, buffer, offset, size);
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            ReceiveAsync();
        }

        #endregion

        #region OnRaiseEvents

        protected virtual void OnRaiseUdpServerReceivedEvent(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            UdpServerReceivedEventHandler raiseEvent = RaiseUdpServerReceivedEvent;
            raiseEvent?.Invoke(this, endpoint, buffer, offset, size);
        }

        #endregion
    }
}
