using AdamController.Services.Interfaces;
using AdamController.Services.WebViewProviderDependency;
using System.Threading.Tasks;

namespace AdamController.Services
{
    public class WebViewProvider : IWebViewProvider
    {
        /*event in view model*/
        public event WebViewNavigationCompleteEventHandler RaiseWebViewNavigationCompleteEvent;
        public event WebViewbMessageReceivedEventHandler RaiseWebViewMessageReceivedEvent;

        /*event in view */
        public event ExecuteJavaScriptEventHandler RaiseExecuteJavaScriptEvent;
        public event ExecuteReloadWebViewEventHandler RaiseExecuteReloadWebViewEvent;

        public void Dispose()
        {
            
        }

        public void WebViewMessageReceived(WebMessageJsonReceived receivedResult)
        {
            OnRaiseWebViewbMessageReceivedEvent(receivedResult);
        }

        public Task<string> ExecuteJavaScript(string script)
        {
            return OnRaiseExecuteJavaScriptEvent(script);
        }

        public void NavigationComplete()
        {
            OnRaiseWebViewNavigationCompleteEvent();
        }

        public virtual void ReloadWebView()
        {
            OnRaiseExecuteReloadWebViewEvent();
        }

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

        protected virtual Task<string> OnRaiseExecuteJavaScriptEvent(string script)
        {
            ExecuteJavaScriptEventHandler raiseEvent = RaiseExecuteJavaScriptEvent;
            return raiseEvent?.Invoke(this, script);
        }

        protected virtual void OnRaiseExecuteReloadWebViewEvent()
        {
            ExecuteReloadWebViewEventHandler raiseEvent = RaiseExecuteReloadWebViewEvent;
            raiseEvent?.Invoke(this);
        }


    }
}
