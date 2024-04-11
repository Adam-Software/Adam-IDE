using AdamController.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Websocket.Client;

namespace AdamController.Services
{
    public class AdamWebSocketClientService : IAdamWebSocketClientService
    {
        #region Events

        public event WebSocketClientReceived RaiseWebSocketClientReceived;
        public event WebSocketConnected RaiseWebSocketConnected;
        public event WebSocketClientDisconnect RaiseWebSocketClientDisconnect;

        #endregion

        #region var 

        private readonly WebsocketClient mWebsocketClient;

        #endregion

        #region ~

        public AdamWebSocketClientService(Uri url)
        {
            mWebsocketClient = new(url)
            {
                ReconnectTimeout = null
            };

            Subscribe();
        }

        #endregion

        #region Public field

        public bool IsStarted => mWebsocketClient.IsStarted;

        public bool IsRunning => mWebsocketClient.IsRunning;

        #endregion

        #region Public method

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

        #endregion

        #region Subscriptions

        private void Subscribe()
        {
            mWebsocketClient.MessageReceived.Subscribe(message =>
            {
                OnRaiseWebSocketClientReceived(message.Text);
            });

            mWebsocketClient.DisconnectionHappened.Subscribe(eventHappened =>
            {
                OnRaiseWebSocketClientDisconnect();
            });

            mWebsocketClient.ReconnectionHappened.Subscribe(eventHappened =>
            {
                OnRaiseWebSocketConnected();
            });
        }

        #endregion

        #region OnRaiseEvents

        protected virtual void OnRaiseWebSocketClientReceived(string text)
        {
            WebSocketClientReceived raiseEvent = RaiseWebSocketClientReceived;
            raiseEvent?.Invoke(this, text);
        }

        protected virtual void OnRaiseWebSocketConnected()
        {
            WebSocketConnected raiseEvent = RaiseWebSocketConnected;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaiseWebSocketClientDisconnect()
        {
            WebSocketClientDisconnect raiseEvent = RaiseWebSocketClientDisconnect;
            raiseEvent?.Invoke(this);
        }

        #endregion
    }
}



 
