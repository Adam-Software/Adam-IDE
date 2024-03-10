using System;
using System.Threading.Tasks;
using Websocket.Client;

namespace AdamController.Services
{
    public class AdamWebSocketClient : IDisposable
    {

        public delegate void OnWebSocketClientReceived(string text);
        public event OnWebSocketClientReceived WebSocketClientReceivedEvent;

        public delegate void OnWebSocketConnected();
        public event OnWebSocketConnected WebSocketConnectedEvent;

        public delegate void OnWebSocketClientDisconnect();
        public event OnWebSocketClientDisconnect WebSocketClientDisconnectedEvent;

        private readonly WebsocketClient mWebsocketClient;
        public AdamWebSocketClient(Uri url)
        {
            mWebsocketClient = new(url)
            {
                ReconnectTimeout = null
            };

            mWebsocketClient.MessageReceived.Subscribe(message =>
            {
                WebSocketClientReceivedEvent?.Invoke(message.Text);
            });

            mWebsocketClient.DisconnectionHappened.Subscribe(eventHappened => 
            {
                WebSocketClientDisconnectedEvent?.Invoke();
            });

            mWebsocketClient.ReconnectionHappened.Subscribe(eventHappened =>
            {
                WebSocketConnectedEvent?.Invoke();
            });
        }

        public bool IsStarted => mWebsocketClient.IsStarted;

        public bool IsRunning => mWebsocketClient.IsRunning;

        public Task ConnectAsync()
        {
            return mWebsocketClient.StartOrFail();
        }

        public Task<bool> DisconnectAsync()
        {
            return mWebsocketClient.StopOrFail(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Nomal close");
        }

        public Task SendTextAsync(string text)
        {
            if(!string.IsNullOrEmpty(text))
            {
                Task task = mWebsocketClient.SendInstant(text);
                return task;
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            mWebsocketClient.Dispose();
        }
    }
}



 
