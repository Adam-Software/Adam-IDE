using AdamController.Services.AdamTcpClientDependency;
using System;
using System.Net.Sockets;
using System.Threading;

namespace AdamController.Services
{
    public class AdamTcpClient : NetCoreServer.TcpClient
    {
        #region DelegateAndEvent

        public delegate void OnTcpCientConnected();
        public event OnTcpCientConnected TcpCientConnected;

        public delegate void OnTcpCientSent(long sent, long pending);
        public event OnTcpCientSent TcpCientSent;

        public delegate void OnTcpClientDisconnect();
        public event OnTcpClientDisconnect TcpClientDisconnected;

        public delegate void OnTcpClientError(SocketError error);
        public event OnTcpClientError TcpClientError;

        public delegate void OnTcpClientReceived(byte[] buffer, long offset, long size);
        public event OnTcpClientReceived TcpClientReceived;

        public delegate void OnTcpClientReconnected(int reconnectCount);
        public event OnTcpClientReconnected TcpClientReconnected;

        #endregion

        #region Public field

        /// <summary>
        /// The number of reconnections when the connection is lost
        /// </summary>
        public int ReconnectCount { get;  private set; }

        /// <summary>
        /// Reconnection timeout
        /// </summary>
        public int ReconnectTimeout { get; private set; }

        #endregion

        #region Private variable

        private int mReconnectTimeout;
        private int mReconnectCount;
        private bool mStop;
        private bool mDisconnectAlreadyInvoke = false;
        private CancellationTokenSource mTokenSource;

        #endregion

        #region ~

        public AdamTcpClient(string address, int port, AdamTcpClientOption option) : base(address, port) 
        {
            ReconnectCount = option.ReconnectCount;
            ReconnectTimeout = option.ReconnectTimeout;
            
            RenewVariable(true);
            //it must be in renew variable method, but this called status wrong update
            //mReconnectCount = ReconnectCount;
        }

        #endregion

        public void DisconnectAndStop()
        {
            mTokenSource.Cancel();
            mStop = true;

            if (IsConnected)
            {
                _ = DisconnectAsync();
            }
            
            while (IsConnected)
            {
                _ = Thread.Yield();
            }
        }

        /// <summary>
        /// Need update varible on connected because clients in helper class static
        /// </summary>
        private void RenewVariable(bool updateReconnect)
        {
            mTokenSource = new CancellationTokenSource();
            mStop = false;
            mDisconnectAlreadyInvoke = false;
           
            mReconnectTimeout = ReconnectTimeout;

            if (!updateReconnect) return;
            //it must be in renew variable  in all method, but this called while connecting create inifinity update variable on reconnecting
            mReconnectCount = ReconnectCount;

        }

        protected override void OnDisconnected()
        {
            if (mStop) 
            {
                if (!mDisconnectAlreadyInvoke)
                {
                    mDisconnectAlreadyInvoke = true;
                    TcpClientDisconnected?.Invoke();
                }
                
                return; 
            }
            
            if (mReconnectCount == 0)
            {
                mStop = true;
                
                if (!mDisconnectAlreadyInvoke)
                {
                    mDisconnectAlreadyInvoke = true;
                    TcpClientDisconnected?.Invoke();
                }
                
                RenewVariable(true);

                //it must be in renew variable method, but this called status wrong update while reconnecting
                //mReconnectCount = ReconnectCount;

            }
            else
            {
                Reconnect(mTokenSource);
            }
        }

        private void Reconnect(CancellationTokenSource tokenSource)
        {          
            if (!tokenSource.IsCancellationRequested)
            {
                TcpClientReconnected?.Invoke(mReconnectCount--);
                _ = tokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(mReconnectTimeout));
                _ = ConnectAsync();
            }
            else
            {
                tokenSource.Dispose();
            }
        }

        protected override void OnConnected()
        {
            TcpCientConnected?.Invoke();
            RenewVariable(false);
            
            base.OnConnected();
        }

        protected override void OnSent(long sent, long pending)
        {
            TcpCientSent?.Invoke(sent, pending);
        }

        protected override void OnError(SocketError error)
        {
            TcpClientError?.Invoke(error);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            TcpClientReceived?.Invoke(buffer, offset, size);
        }
    }
}
