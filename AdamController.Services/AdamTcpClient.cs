using AdamController.Services.AdamTcpClientDependency;
using System;
using System.Net.Sockets;
using System.Threading;

namespace AdamController.Services
{

    #region Events

    public delegate void TcpCientConnected(object sender);
    public delegate void TcpCientSent(object sender, long sent, long pending);
    public delegate void TcpClientDisconnect(object sender);
    public delegate void TcpClientError(object sender, SocketError error);
    public delegate void TcpClientReceived(object sender, byte[] buffer, long offset, long size);
    public delegate void TcpClientReconnected(object sender, int reconnectCount);

    #endregion

    public interface IAdamTcpClient
    {
        #region Events

        public event TcpCientConnected RaiseTcpCientConnected;
        public event TcpCientSent RaiseTcpCientSent;
        public event TcpClientDisconnect RaiseTcpClientDisconnected;
        public event TcpClientError RaiseTcpClientError;
        public event TcpClientReceived RaiseTcpClientReceived;
        public event TcpClientReconnected RaiseTcpClientReconnected;

        #endregion

        /// <summary>
        /// The number of reconnections when the connection is lost
        /// </summary>
        public int ReconnectCount { get; }

        /// <summary>
        /// Reconnection timeout
        /// </summary>
        public int ReconnectTimeout { get; }

        public void DisconnectAndStop();

    }

    public class AdamTcpClient : NetCoreServer.TcpClient, IAdamTcpClient
    {
        #region Events

        public event TcpCientConnected RaiseTcpCientConnected;
        public event TcpCientSent RaiseTcpCientSent;
        public event TcpClientDisconnect RaiseTcpClientDisconnected;
        public event TcpClientError RaiseTcpClientError;
        public event TcpClientReceived RaiseTcpClientReceived;
        public event TcpClientReconnected RaiseTcpClientReconnected;

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

        #region Public field

        /// <summary>
        /// The number of reconnections when the connection is lost
        /// </summary>
        public int ReconnectCount { get; }

        /// <summary>
        /// Reconnection timeout
        /// </summary>
        public int ReconnectTimeout { get; }

        #endregion

        #region Public methods

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

        #endregion

        #region Private methods

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
                    OnRaiseTcpClientDisconnected();
                }
                
                return; 
            }
            
            if (mReconnectCount == 0)
            {
                mStop = true;
                
                if (!mDisconnectAlreadyInvoke)
                {
                    mDisconnectAlreadyInvoke = true;
                    OnRaiseTcpClientDisconnected();
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
                OnRaiseTcpClientReconnected(mReconnectCount--);

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
            OnRaiseTcpCientConnected();
            RenewVariable(false);
            
            base.OnConnected();
        }

        protected override void OnSent(long sent, long pending)
        {
            OnRaiseTcpCientSent(sent, pending);
        }

        protected override void OnError(SocketError error)
        {
            OnRaiseTcpClientError(error);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            OnRaiseTcpClientReceived(buffer, offset, size);
            
        }

        #endregion

        #region RaiseEvents

        public virtual void OnRaiseTcpCientConnected()
        {
            TcpCientConnected raiseEvent = RaiseTcpCientConnected;
            raiseEvent?.Invoke(this);
        }

        public virtual void OnRaiseTcpCientSent(long sent, long pending)
        {
            TcpCientSent raiseEvent = RaiseTcpCientSent;
            raiseEvent?.Invoke(this, sent, pending);
        }

        public virtual void OnRaiseTcpClientDisconnected()
        {
            TcpClientDisconnect raiseEvent = RaiseTcpClientDisconnected;
            raiseEvent?.Invoke(this);
        }

        public virtual void OnRaiseTcpClientError(SocketError socketError)
        {
            TcpClientError raiseEvent = RaiseTcpClientError;
            raiseEvent?.Invoke(this, socketError);
        }

        public virtual void OnRaiseTcpClientReceived(byte[] buffer, long offset, long size)
        {
            TcpClientReceived raiseEvent = RaiseTcpClientReceived;
            raiseEvent?.Invoke(this, buffer, offset, size);
        }
        
        public virtual void OnRaiseTcpClientReconnected(int reconnectCount)
        {
            TcpClientReconnected raiseEvent = RaiseTcpClientReconnected;
            raiseEvent?.Invoke(this, reconnectCount);
        }

        #endregion
    }
}
