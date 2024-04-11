using NetCoreServer;
using System;
using System.Net;
using System.Net.Sockets;

namespace AdamController.Services
{
    [Obsolete]
    public class AdamUdpServer : UdpServer
    {
        public delegate void OnUdpServerReceived(EndPoint endpoint, byte[] buffer, long offset, long size);
        public event OnUdpServerReceived UdpServerReceived;

        public AdamUdpServer(IPAddress address, int port) : base(address, port){}

        protected override void OnStarted()
        {
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            UdpServerReceived?.Invoke(endpoint, buffer, offset, size);
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
