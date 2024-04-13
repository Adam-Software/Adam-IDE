using AdamController.Services.Interfaces;
using System.Net;

namespace AdamController.Services
{
    public class UdpClientService : NetCoreServer.UdpServer, IUdpClientService
    {
        public event UdpClientReceivedEventHandler RaiseUdpClientReceivedEvent;

        public UdpClientService(IPAddress address, int port) : base(address, port) { }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            OnRaiseUdpClientReceivedEvent(endpoint, buffer, offset, size);
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            ReceiveAsync();
        }

        protected override void OnStarted()
        {
            ReceiveAsync(); 
        }

        #region OnRaiseEvents

        protected virtual void OnRaiseUdpClientReceivedEvent(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            UdpClientReceivedEventHandler raiseEvent = RaiseUdpClientReceivedEvent;
            raiseEvent?.Invoke(this, endpoint, buffer, offset, size);
        }

        #endregion
    }
}
