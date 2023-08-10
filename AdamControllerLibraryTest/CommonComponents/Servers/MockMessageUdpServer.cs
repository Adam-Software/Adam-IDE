using SimpleUdp;
using System;
using System.Threading;

namespace AdamControllerProjectsTest.CommonComponents.Servers
{
    public class MockMessageUdpServer
    {
        public event EventHandler<UpdServerEventArgs> DataReceived;
        private static readonly UdpEndpoint mMockServer = new("127.0.0.1", 5005);

        public MockMessageUdpServer()
        {
            mMockServer.DatagramReceived += DatagramReceived;
            //mMockServer.StartServer();

            Thread threadServerMessage = new(() => mMockServer.StartServer());
            threadServerMessage.Start();
            
        }


        private void DatagramReceived(object sender, Datagram e)
        {
            UpdServerEventArgs updServerEventArgs = new()
            {
                Data = e.Data
            };

            OnDataReceived(updServerEventArgs);
        }

        protected virtual void OnDataReceived(UpdServerEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }
    }

    public class UpdServerEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
