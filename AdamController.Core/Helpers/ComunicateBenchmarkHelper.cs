
using AdamController.Core.Properties;
using AdamController.Services;
using AdamController.Services.AdamTcpClientDependency;
using System.Threading;

namespace AdamController.Core.Helpers
{
    
    public sealed class ComunicateBenchmarkHelper
    {
        #region Singelton

        private static readonly object @lock = new();
        private static ComunicateBenchmarkHelper instance = null;

        public static ComunicateBenchmarkHelper Instance
        {
            get
            {
                if (instance != null)
                {
                    LazyInitializer();
                    return instance;
                }

                Monitor.Enter(@lock);
                ComunicateBenchmarkHelper temp = new();
                
                _ = Interlocked.Exchange(ref instance, temp);
                Monitor.Exit(@lock);
                return instance;
            }
        }

        #endregion

        #region Declaration delegates and events

        public delegate void OnTcpCientConnected();
        public static event OnTcpCientConnected OnTcpConnected;

        public delegate void OnTcpClientDisconnect();
        public static event OnTcpClientDisconnect OnTcpDisconnected;

        public delegate void OnTcpClientReconnected(int reconnectCount);
        public static event OnTcpClientReconnected OnTcpReconnected;

        #endregion

        public bool TcpClientIsConnected => mAdamTcpClient != null && mAdamTcpClient.IsConnected;

        private static AdamTcpClientService mAdamTcpClient;

        #region ~

        static ComunicateBenchmarkHelper()
        {
            LazyInitializer();
        }

        private static void LazyInitializer()
        {
            if(mAdamTcpClient == null)
            {
                AdamTcpClientOption option = new()
                {
                    ReconnectCount = Settings.Default.ReconnectQtyBenchmarkComunicateTcpClient,
                    ReconnectTimeout = Settings.Default.ReconnectTimeoutBenchmarkComunicateTcpClient
                };

                mAdamTcpClient = new(Settings.Default.BenchmarkTestServerIp, Settings.Default.BenchmarkTcpConnectStatePort, option);
            }
            
            mAdamTcpClient.RaiseTcpCientConnected += TcpCientConnected;
            mAdamTcpClient.RaiseTcpClientDisconnected += TcpClientDisconnected;
            mAdamTcpClient.RaiseTcpClientReconnected += TcpClientReconnected;
        }

        #endregion

        #region Client events

        private static void TcpClientDisconnected(object sender)
        {
            OnTcpDisconnected?.Invoke();
        }

        private static void TcpCientConnected(object sender)
        {
            OnTcpConnected?.Invoke();
        }

        private static void TcpClientReconnected(object sender, int reconnectCount)
        {
            OnTcpReconnected?.Invoke(reconnectCount);
        }

        #endregion

        #region Tcp connect

        public void Connect()
        {
            _ = mAdamTcpClient?.ConnectAsync();
        }

        #endregion

        # region Tcp disconnect 

        public void Disconnect()
        {
            mAdamTcpClient?.DisconnectAndStop();
        }

        public void DisconnectAndDestroy()
        {
            Disconnect();
            
            if(mAdamTcpClient != null)
            {
                mAdamTcpClient.RaiseTcpCientConnected -= TcpCientConnected;
                mAdamTcpClient.RaiseTcpClientDisconnected -= TcpClientDisconnected;
                mAdamTcpClient.RaiseTcpClientReconnected -= TcpClientReconnected;
                mAdamTcpClient = null;
            }
        }

        #endregion
    }
}
