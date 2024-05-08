
using AdamController.Controls.CustomControls.Services;
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
        private readonly IControlHelper mControlHelper;

        #endregion

        #region Var

        private readonly string mPathToSource;
        private readonly string mPath;

        #endregion

        public ScratchControlView(IWebViewProvider webViewProvider, IStatusBarNotificationDeliveryService statusBarNotification, IFolderManagmentService folderManagment, IControlHelper controlHelper)
        {
            InitializeComponent();
            InitializeWebViewCore();

            mWebViewProvider = webViewProvider;
            mStatusBarNotification = statusBarNotification;
            mControlHelper = controlHelper;

            mPathToSource = Path.Combine(folderManagment.CommonDirAppData, "BlocklySource");
            mPath = Path.Combine(Path.GetTempPath(), "AdamBrowser");

            WebView.CoreWebView2InitializationCompleted += WebViewCoreWebView2InitializationCompleted;
            WebView.NavigationCompleted += WebViewNavigationCompleted;
            WebView.WebMessageReceived += WebViewWebMessageReceived;

            mWebViewProvider.RaiseExecuteJavaScriptEvent += RaiseExecuteJavaScriptEvent;
            mWebViewProvider.RaiseExecuteReloadWebViewEvent += RaiseExecuteReloadWebViewEvent;

            TextResulEditor.TextChanged += TextResulEditorTextChanged;

            MainGrid.SizeChanged += MainGridSizeChanged;
            SourceEditor.SizeChanged += TextResulEditorSizeChanged;
        }

        private void TextResulEditorSizeChanged(object sender, SizeChangedEventArgs e)
        {
            mControlHelper.BlocklyColumnActualWidth = BlocklyColumn.ActualWidth;
        }

        private void MainGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            mControlHelper.MainGridActualWidth = MainGrid.ActualWidth;   
        }

        private void RaiseExecuteReloadWebViewEvent(object sender)
        {
            WebView?.CoreWebView2?.Reload();
        }

        private async Task<string> RaiseExecuteJavaScriptEvent(object sender, string script, bool deserializeResultToString = false)
        {
            string result = await WebView.ExecuteScriptAsync(script);

            if (deserializeResultToString)
            {
                string deserealizeString = JsonSerializer.Deserialize<string>(result);
                return deserealizeString;
            }

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
