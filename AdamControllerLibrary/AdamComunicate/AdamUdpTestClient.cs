using System.Net;
using System.Net.Sockets;

namespace AdamController.Core.AdamComunicate
{
    public class AdamUdpTestClient : NetCoreServer.UdpClient
    {
        public long TotalBytes { get; private set; }
        public long TotalErrors { get; private set; }
        public string LastErrorMessage { get; private set; } = string.Empty;
        public byte[] mSendMessage;
        
        private readonly long mMessages;

        public AdamUdpTestClient(string address, int port, int messageQty, byte[] sendMessage) : base(address, port)
        {
            mMessages = messageQty;
            mSendMessage = sendMessage;
        }

        protected override void OnConnected()
        {
            ReceiveAsync();

            for (long i = mMessages; i > 0; --i)
            {
                SendMessage();
            }
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            TotalBytes += size;

            ReceiveAsync();
            SendMessage();
        }

        protected override void OnError(SocketError error)
        {
            LastErrorMessage = error.ToString();
            ++TotalErrors;
        }

        private void SendMessage()
        {
            Send(mSendMessage);
        }
    }
}
