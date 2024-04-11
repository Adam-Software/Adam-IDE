using AdamController.Services.Interfaces;
using NetCoreServer;
using System.Net;

namespace AdamController.Services
{
    public class AdamUdpServerService : UdpServer, IAdamUdpServerService
    {
        #region Events

        public event UdpServerReceived RaiseUdpServerReceived;

        #endregion

        #region ~

        public AdamUdpServerService(IPAddress address, int port) : base(address, port){}

        #endregion

        #region Private methods

        protected override void OnStarted()
        {
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            OnRaiseUdpServerReceived(endpoint, buffer, offset, size);
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            ReceiveAsync();
        }

        #endregion

        #region OnRaiseEvents

        protected virtual void OnRaiseUdpServerReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            UdpServerReceived raiseEvent = RaiseUdpServerReceived;
            raiseEvent?.Invoke(this, endpoint, buffer, offset, size);
        }

        #endregion
    }
}
