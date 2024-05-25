using AdamController.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AdamController.Modules.ContentRegion.Views
{

    public partial class ComputerVisionControlView : UserControl
    {
        #region Service

        private readonly IWebSocketClientService mWebSocketClient;

        #endregion
       
        public ComputerVisionControlView(IWebSocketClientService webSocketClient)
        {
            InitializeComponent();

            mWebSocketClient = webSocketClient;
            VideoView.Loaded += VideoView_Loaded;
            Unloaded += UserControlUnloaded;
        }

        private async void VideoView_Loaded(object sender, RoutedEventArgs e)
        {

            string ip = Core.Properties.Settings.Default.ServerIP;
            string port = Core.Properties.Settings.Default.VideoDataExchangePort;
            var uri = new Uri($"http://{ip}:{port}/stream/0.mjpeg");
            

            await VideoView.Open(uri);

            //mMediaPlayer.EnableHardwareDecoding = true;
            //mMediaPlayer.NetworkCaching = 1000;
            //mMediaPlayer.Scale = 0.72f;
        }

        private async void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            await VideoView.Close();
        }
        public string DownRightDirection { get; private set; } = "{\"move\":{\"x\": 0, \"y\": 1, \"z\": 0}}";

        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            mWebSocketClient.SendTextAsync(DownRightDirection);
        }
    }
}
