using System.Net;
using System.Net.Sockets;

namespace AdamController.Core.Converters
{
    public class AdamUdpClient : NetCoreServer.UdpServer
    {
        #region DelegateAndEvent

        public delegate void OnUdpClientReceived(EndPoint endpoint, byte[] buffer, long offset, long size);
        public event OnUdpClientReceived UdpClientReceived;

        #endregion

        public AdamUdpClient(IPAddress address, int port) : base(address, port) { }

        protected override void OnStarted()
        {
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            UdpClientReceived?.Invoke(endpoint, buffer, offset, size);
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            base.OnError(error);
        }
    }
}
