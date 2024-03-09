using System.Net.Sockets;

namespace AdamController.Core.Converters
{
    public class AdamTestTcpClient : NetCoreServer.TcpClient
    {
        public long TotalBytes { get; private set; }
        public long TotalErrors { get; private set; }
        public string LastErrorMessage { get; private set; } = string.Empty;

        private long mReceived = 0;
        private readonly long mMessageQty;
        private readonly byte[] mSendMessage;

        public AdamTestTcpClient(string address, int port, int messageQty, byte[] sendMessage) : base(address, port)
        {
            mMessageQty = messageQty;
            mSendMessage = sendMessage;
        }

        protected override void OnConnected()
        {
            for (long i = mMessageQty; i > 0; --i)
            {
                SendMessage();
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            mReceived += size;
            while (mReceived >= mSendMessage.Length)
            {
                SendMessage();
                mReceived -= mSendMessage.Length;
            }

            TotalBytes += size;
        }

        protected override void OnError(SocketError error)
        {
            LastErrorMessage = error.ToString();
            ++TotalErrors;
        }

        private void SendMessage()
        {
            _ = SendAsync(mSendMessage);
        }
    }

   
}
