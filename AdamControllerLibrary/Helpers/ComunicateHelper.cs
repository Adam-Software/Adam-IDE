using AdamController.Core.AdamComunicate;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AdamController.Core.Helpers
{
    public sealed class ComunicateHelper
    {
        #region Declaration delegates and events

        public delegate void OnAdamTcpCientConnected();
        public static event OnAdamTcpCientConnected OnAdamTcpConnectedEvent;

        public delegate void OnAdamTcpClientDisconnect();
        public static event OnAdamTcpClientDisconnect OnAdamTcpDisconnectedEvent;

        public delegate void OnAdamTcpClientReconnected(int reconnectCount);
        public static event OnAdamTcpClientReconnected OnAdamTcpReconnected;

        public delegate void OnAdamUdpServerReceived(string message);
        public static event OnAdamUdpServerReceived OnAdamLogServerUdpReceivedEvent;

        public delegate void OnAdamUdpMessageReceived(string message);
        public static event OnAdamUdpMessageReceived OnAdamMessageReceivedEvent;

        #endregion

        public static bool TcpClientIsConnected => mAdamTcpClient != null && mAdamTcpClient.IsConnected;

        private static AdamTcpClient mAdamTcpClient;
        private static AdamUdpClient mAdamUdpMessageClient;
        private static AdamUdpServer mAdamUdpLogServer;
        private static AdamWebSocketClient mAdamWebSocketClient;

        #region ~ 

        static ComunicateHelper()
        {
            LazyInitializer();
        }

        private static void LazyInitializer()
        {
            if(mAdamTcpClient == null)
            {
                AdamTcpClientOption option = new()
                {
                    ReconnectCount = Settings.Default.ReconnectQtyComunicateTcpClient,
                    ReconnectTimeout = Settings.Default.ReconnectTimeoutComunicateTcpClient
                };

                mAdamTcpClient = new(Settings.Default.ServerIP, Settings.Default.TcpConnectStatePort, option);
            }

            mAdamUdpMessageClient ??= new(IPAddress.Any, int.Parse(Settings.Default.MessageDataExchangePort))
                {
                    OptionDualMode = true,
                    OptionReuseAddress = true
                };

            mAdamUdpLogServer ??= new(IPAddress.Any, Settings.Default.LogServerPort)
                {
                    OptionDualMode = true,
                    OptionReuseAddress = true
                };

#if DEBUG
            mAdamWebSocketClient ??= new AdamWebSocketClient(new Uri($"ws://{Settings.Default.ServerIP}:9001/adam-2.7/movement"));
#else
            mAdamWebSocketClient ??= new AdamWebSocketClient(new Uri($"ws://{Settings.Default.ServerIP}:{Settings.Default.SoketServerPort}/adam-2.7/movement")); 
#endif


            mAdamTcpClient.TcpCientConnected += TcpCientConnected;
            mAdamTcpClient.TcpClientDisconnected += TcpClientDisconnected;
            mAdamTcpClient.TcpClientError += TcpClientError;
            mAdamTcpClient.TcpClientReceived += TcpClientReceived;
            mAdamTcpClient.TcpClientReconnected += TcpClientReconnected;

            mAdamWebSocketClient.WebSocketConnectedEvent += WebSocketConnectedEvent;
            mAdamWebSocketClient.WebSocketClientReceivedEvent += WebSocketClientReceived;
            mAdamWebSocketClient.WebSocketClientDisconnectedEvent += WebSocketClientDisconnectedEvent;
            
            mAdamUdpMessageClient.UdpClientReceived += MessageClientUdpReceived;
            mAdamUdpLogServer.UdpServerReceived += UdpServerReceived;
        }

        private static void WebSocketClientDisconnectedEvent()
        {
            //throw new System.NotImplementedException();
        }

        private static void WebSocketClientReceived(string text)
        {
            //throw new System.NotImplementedException();
        }

        private static void WebSocketConnectedEvent()
        {
            //throw new System.NotImplementedException();
        }

        private static void MessageClientUdpReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnAdamMessageReceivedEvent?.Invoke(encodedMessage);
        }

        #endregion

        #region Udp Server events

        private static void UdpServerReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string encodedMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            OnAdamLogServerUdpReceivedEvent?.Invoke(encodedMessage);
        }

        #endregion

        #region TCP Client events

        private static void TcpClientReceived(byte[] buffer, long offset, long size) {}

        private static void TcpClientError(SocketError error) {}

        private static void TcpClientDisconnected()
        {
            OnAdamTcpDisconnectedEvent?.Invoke();

            if (mAdamUdpLogServer != null)
            {
                if (mAdamUdpLogServer.IsStarted)
                    mAdamUdpLogServer?.Stop();
            }

            if(mAdamWebSocketClient != null)
            {
                //if (mAdamWebSocketClient.IsRunning)
                mAdamWebSocketClient.DisconnectAsync();
            }
        }

        private static void TcpCientConnected()
        {
            OnAdamTcpConnectedEvent?.Invoke();
            
            if(!mAdamUdpLogServer.IsStarted)
                mAdamUdpLogServer?.Start();

            //if (!mAdamWebSocketClient.IsStarted)
            mAdamWebSocketClient.ConnectAsync();
        }

        private static void TcpClientReconnected(int reconnectCount)
        {
            OnAdamTcpReconnected?.Invoke(reconnectCount);
        }

        #endregion

        #region Tcp/Udp connect
        
        private static void Connect()
        {
            _ = Task.Run(() => mAdamTcpClient?.ConnectAsync());
            _ = Task.Run(() => mAdamUdpMessageClient?.Start());
            _ = Task.Run(() => mAdamWebSocketClient?.ConnectAsync());
        }

        public static void ConnectAllAsync()
        {
            _ = Task.Run(() => Connect());
        }

        #endregion

        # region Tcp/Upd disconnect 

        public static void DisconnectAll()
        {
            _ = Task.Run(()=> mAdamTcpClient?.DisconnectAndStop());
            _ = Task.Run(() => mAdamUdpMessageClient?.Stop());
            _ = Task.Run(() => mAdamWebSocketClient?.DisconnectAsync());
        }

        public static void DisconnectAllAndDestroy()
        {
            DisconnectAll();
            
            if(mAdamTcpClient != null)
            {
                mAdamTcpClient.TcpCientConnected -= TcpCientConnected;
                mAdamTcpClient.TcpClientDisconnected -= TcpClientDisconnected;
                mAdamTcpClient.TcpClientError -= TcpClientError;
                mAdamTcpClient.TcpClientReceived -= TcpClientReceived;
                mAdamTcpClient.TcpClientReconnected -= TcpClientReconnected;
                
                mAdamTcpClient = null;
            }

            if(mAdamUdpLogServer != null)
            {
                mAdamUdpLogServer.UdpServerReceived -= UdpServerReceived;
                mAdamUdpLogServer = null;
            }

            if(mAdamUdpMessageClient != null)
            {
                mAdamUdpMessageClient.UdpClientReceived -= MessageClientUdpReceived;
                mAdamUdpLogServer = null;
            }

            if(mAdamWebSocketClient != null)
            {
                mAdamWebSocketClient.WebSocketConnectedEvent -= WebSocketConnectedEvent;
                mAdamWebSocketClient.WebSocketClientReceivedEvent -= WebSocketClientReceived;
                mAdamWebSocketClient.WebSocketClientDisconnectedEvent -= WebSocketClientDisconnectedEvent;
                //mAdamWebSocketClient.Dispose();
            }
        }

        #endregion

        #region Tcp/Udp/WebSocket send message

        public static void SendTcpMessage(string message)
        {
            mAdamTcpClient.Send(message);
        }

        public static void SendTcpMessageAsync(string message)
        {
            _ = Task.Run(() => SendTcpMessage(message)); 
        }

        public static void WebSocketSendTextMessage(string message) 
        {
            Task.Run(() => mAdamWebSocketClient.SendTextAsync(message));
        }

        #endregion
    }
}
