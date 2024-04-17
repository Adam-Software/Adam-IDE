using AdamController.Services.WebViewProviderDependency;
using System;
using System.Threading.Tasks;

namespace AdamController.Services.Interfaces
{
    #region Delegate

    /*event in view model*/
    public delegate void WebViewNavigationCompleteEventHandler(object sender);
    public delegate void WebViewbMessageReceivedEventHandler(object sender, WebMessageJsonReceived webMessageReceived);

    /*event in view */
    public delegate Task<string> ExecuteJavaScriptEventHandler(object sender, string script);
    public delegate void ExecuteReloadWebViewEventHandler(object sender);

    #endregion

    public interface IWebViewProvider : IDisposable
    {
        /*event in view model*/
        public event WebViewNavigationCompleteEventHandler RaiseWebViewNavigationCompleteEvent;
        public event WebViewbMessageReceivedEventHandler RaiseWebViewMessageReceivedEvent;

        /*event in view */
        public event ExecuteJavaScriptEventHandler RaiseExecuteJavaScriptEvent;
        public event ExecuteReloadWebViewEventHandler RaiseExecuteReloadWebViewEvent;

        public Task<string> ExecuteJavaScript(string script);

        /* in view model */
        public void ReloadWebView();
        public void NavigationComplete();
        public void WebViewMessageReceived(WebMessageJsonReceived receivedResult);
    }
}
