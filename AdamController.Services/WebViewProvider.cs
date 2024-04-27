using AdamController.Services.Interfaces;
using AdamController.Services.WebViewProviderDependency;
using System.Threading.Tasks;

namespace AdamController.Services
{
    public class WebViewProvider : IWebViewProvider
    {
        #region Events

        /*event in view model*/
        public event WebViewNavigationCompleteEventHandler RaiseWebViewNavigationCompleteEvent;
        public event WebViewbMessageReceivedEventHandler RaiseWebViewMessageReceivedEvent;

        /*event in view */
        public event ExecuteJavaScriptEventHandler RaiseExecuteJavaScriptEvent;
        public event ExecuteReloadWebViewEventHandler RaiseExecuteReloadWebViewEvent;

        #endregion

        #region ~

        public WebViewProvider(){}

        #endregion

        #region Public methods

        public void WebViewMessageReceived(WebMessageJsonReceived receivedResult)
        {
            OnRaiseWebViewbMessageReceivedEvent(receivedResult);
        }

        public Task<string> ExecuteJavaScript(string script, bool deserializeResultToString = false)
        {
            return OnRaiseExecuteJavaScriptEvent(script, deserializeResultToString);
        }

        public void NavigationComplete()
        {
            OnRaiseWebViewNavigationCompleteEvent();
        }

        public virtual void ReloadWebView()
        {
            OnRaiseExecuteReloadWebViewEvent();
        }

        public void Dispose(){}

        #endregion

        #region OnRaise methods

        protected virtual void OnRaiseWebViewNavigationCompleteEvent()
        {
            WebViewNavigationCompleteEventHandler raiseEvent = RaiseWebViewNavigationCompleteEvent;
            raiseEvent?.Invoke(this);
        }

        protected virtual void OnRaiseWebViewbMessageReceivedEvent(WebMessageJsonReceived result)
        {
            WebViewbMessageReceivedEventHandler raiseEvent = RaiseWebViewMessageReceivedEvent;
            raiseEvent?.Invoke(this, result);
        }

        protected virtual Task<string> OnRaiseExecuteJavaScriptEvent(string script, bool deserializeResultToString)
        {
            ExecuteJavaScriptEventHandler raiseEvent = RaiseExecuteJavaScriptEvent;
            return raiseEvent?.Invoke(this, script, deserializeResultToString);
        }

        protected virtual void OnRaiseExecuteReloadWebViewEvent()
        {
            ExecuteReloadWebViewEventHandler raiseEvent = RaiseExecuteReloadWebViewEvent;
            raiseEvent?.Invoke(this);
        }

        #endregion
    }
}
