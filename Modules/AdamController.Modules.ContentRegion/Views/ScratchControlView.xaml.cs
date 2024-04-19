
using AdamController.Core.Helpers;
using AdamController.Core.Properties;
using AdamController.Services.Interfaces;
using AdamController.Services.WebViewProviderDependency;
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AdamController.Modules.ContentRegion.Views
{
    public partial class ScratchControlView : UserControl
    {
        #region Services

        private readonly IWebViewProvider mWebViewProvider;
        private readonly IStatusBarNotificationDeliveryService mStatusBarNotification;

        #endregion

        private readonly string mPathToSource = Path.Combine(FolderHelper.CommonDirAppData, "BlocklySource");
        private readonly string mPath = Path.Combine(Path.GetTempPath(), "AdamBrowser");


        public ScratchControlView(IWebViewProvider webViewProvider, IStatusBarNotificationDeliveryService statusBarNotification)
        {
            InitializeComponent();
            InitializeWebViewCore();

            mWebViewProvider = webViewProvider;
            mStatusBarNotification = statusBarNotification;

            WebView.CoreWebView2InitializationCompleted += WebViewCoreWebView2InitializationCompleted;
            WebView.NavigationCompleted += WebViewNavigationCompleted;
            WebView.WebMessageReceived += WebViewWebMessageReceived;

            mWebViewProvider.RaiseExecuteJavaScriptEvent += RaiseExecuteJavaScriptEvent;
            mWebViewProvider.RaiseExecuteReloadWebViewEvent += RaiseExecuteReloadWebViewEvent;

            TextResulEditor.TextChanged += TextResulEditorTextChanged;
        }

        private void RaiseExecuteReloadWebViewEvent(object sender)
        {
            WebView?.CoreWebView2?.Reload();
        }

        private async Task<string> RaiseExecuteJavaScriptEvent(object sender, string script)
        {
            string result = await WebView.ExecuteScriptAsync(script);
            return result;
        }

        private void WebViewNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            mWebViewProvider.NavigationComplete();
        }

        private void TextResulEditorTextChanged(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(TextResulEditor.ScrollToEnd));
        }

        private async void InitializeWebViewCore()
        {
            CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: mPath);
            await WebView.EnsureCoreWebView2Async(env);
        }

        private void WebViewCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            //unused options?
            //WebView.CoreWebView2.Settings.AreDevToolsEnabled = true; 
            //WebView.CoreWebView2.Settings.AreHostObjectsAllowed = true;

            WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = !Settings.Default.DontShowBrowserMenuInBlockly;
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("localhost", mPathToSource, CoreWebView2HostResourceAccessKind.Allow);
            WebView.CoreWebView2.Navigate("https://localhost/index.html");
        }
        
        private void WebViewWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            JsonSerializerOptions options = new()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            WebMessageJsonReceived receivedResult;

            try
            {
                string receivedString = e.TryGetWebMessageAsString();
                receivedResult = JsonSerializer.Deserialize<WebMessageJsonReceived>(receivedString, options);
            }
            catch
            {
                mStatusBarNotification.AppLogMessage = "Error reading blokly code";
                receivedResult = new WebMessageJsonReceived { Action = string.Empty, Data = string.Empty };
            }

            mWebViewProvider.WebViewMessageReceived(receivedResult);
        }
    }
}
