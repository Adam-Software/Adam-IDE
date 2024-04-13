using System;
using System.Threading.Tasks;

namespace AdamController.Services.Interfaces
{

    #region Delegate

    public delegate void WebSocketClientReceivedEventHandler(object sender,  string text);
    public delegate void WebSocketConnectedEventHandler(object sender);
    public delegate void WebSocketClientDisconnectEventHandler(object sender);

    #endregion

    public interface IWebSocketClientService : IDisposable
    {
        #region Events

        public event WebSocketClientReceivedEventHandler RaiseWebSocketClientReceivedEvent;
        public event WebSocketConnectedEventHandler RaiseWebSocketConnectedEvent;
        public event WebSocketClientDisconnectEventHandler RaiseWebSocketClientDisconnectEvent;

        #endregion

        #region Public field

        public bool IsStarted { get; }

        public bool IsRunning { get; }

        #endregion

        #region Public method

        public Task ConnectAsync();

        public Task<bool> DisconnectAsync();

        public Task SendTextAsync(string text);

        #endregion
    }
}
