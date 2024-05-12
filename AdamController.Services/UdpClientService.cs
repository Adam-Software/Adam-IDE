using AdamController.Services.Interfaces;
using AdamController.Services.UdpClientServiceDependency;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamController.Services
{
    public class UdpClientService : NetCoreServer.UdpServer, IUdpClientService
    {
        public event UdpClientMessageEnqueueEventHandler RaiseUdpClientMessageEnqueueEvent;

        private readonly QueueWithEvent<ReceivedData> mMessageQueue = new();
        public UdpClientService(IPAddress address, int port) : base(address, port) 
        {
            mMessageQueue.RaiseEnqueueEvent += RaiseEnqueueEvent; 
        }

        private void RaiseEnqueueEvent(object sender, System.EventArgs e)
        {
            var messages = mMessageQueue.Dequeue();

            OnRaiseUdpClientMessageEnqueueEvent(messages);
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            Task.Run(() => 
            {
                mMessageQueue.Enqueue(new(endpoint, buffer, offset, size));
                ReceiveAsync();
            });
        }

        protected override void OnStarted()
        {
            ReceiveAsync();
        }

        #region OnRaiseEvents

        protected virtual void OnRaiseUdpClientMessageEnqueueEvent(ReceivedData data)
        {
            UdpClientMessageEnqueueEventHandler raiseEvent = RaiseUdpClientMessageEnqueueEvent;
            raiseEvent?.Invoke(this, data);
        }

        #endregion
    }
}
