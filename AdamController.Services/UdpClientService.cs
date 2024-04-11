using AdamController.Services.Interfaces;
using System.Net;

namespace AdamController.Services
{
    public class UdpClientService : NetCoreServer.UdpServer, IUdpClientService
    {
        public event UdpClientReceived RaiseUdpClientReceived;

        public UdpClientService(IPAddress address, int port) : base(address, port) { }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            OnRaiseUdpClientReceived(endpoint, buffer, offset, size);
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

        protected virtual void OnRaiseUdpClientReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            UdpClientReceived raiseEvent = RaiseUdpClientReceived;
            raiseEvent?.Invoke(this, endpoint, buffer, offset, size);
        }

        #endregion
    }
}
