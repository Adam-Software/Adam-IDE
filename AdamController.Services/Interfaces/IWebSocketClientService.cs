using System;
using System.Threading.Tasks;

namespace AdamController.Services.Interfaces
{

    #region Delegate

    public delegate void WebSocketClientReceived(object sender,  string text);
    public delegate void WebSocketConnected(object sender);
    public delegate void WebSocketClientDisconnect(object sender);

    #endregion

    public interface IWebSocketClientService : IDisposable
    {
        #region Events

        public event WebSocketClientReceived RaiseWebSocketClientReceived;
        public event WebSocketConnected RaiseWebSocketConnected;
        public event WebSocketClientDisconnect RaiseWebSocketClientDisconnect;

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
