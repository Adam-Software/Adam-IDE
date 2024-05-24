using AdamController.Services.Interfaces;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using Unosquare.FFME.Playlists;

namespace AdamController.Modules.ContentRegion.Views
{

    public partial class ComputerVisionControlView : UserControl, IDisposable
    {
        #region Service

        private readonly IWebSocketClientService mWebSocketClient;

        #endregion

        //private LibVLC mLibVLC;
        //private MediaPlayer mMediaPlayer;

        public ComputerVisionControlView(IWebSocketClientService webSocketClient)
        {
            
            InitializeComponent();

            mWebSocketClient = webSocketClient;
            VideoView.Loaded += VideoView_Loaded;
            //Unloaded += UserControlUnloaded;
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var userDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var uri = new Uri($"{userDir}{Path.DirectorySeparatorChar}test.avi");
                VideoView.Open(uri);
                //mLibVLC = new LibVLC(enableDebugLogs: false);
                //mMediaPlayer = new MediaPlayer(mLibVLC);

                //VideoView.MediaPlayer = mMediaPlayer;

                //mMediaPlayer.EnableHardwareDecoding = true;
                //mMediaPlayer.NetworkCaching = 1000;
                //mMediaPlayer.Scale = 0.72f;
            }
            catch 
            {
                //fix for
                //LibVLCSharp.Shared.VLCException: "Failed to load required native libraries
            }

        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //mMediaPlayer?.Dispose();
                //mLibVLC?.Dispose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ip = Core.Properties.Settings.Default.ServerIP;
            string port = Core.Properties.Settings.Default.VideoDataExchangePort;
            //var uri = new Uri($"http://{ip}:{port}/stream/0.mjpeg");

            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var uri = new Uri($"{userDir}{Path.DirectorySeparatorChar}test.avi");
            /*HttpClient client = new HttpClient();
            HttpResponseMessage test = await client.GetAsync(uri);
            var con = test.Content;
            Stream str = con.ReadAsStream();*/

            if (VideoView == null)
                return;

            Play(uri);
            //VideoView.Source = uri;
            //var test = VideoView.HasVideo;

            //await VideoView.Play();
            
            /*if (!VideoView.MediaPlayer.IsPlaying)
            {
                using var media = new Media(mLibVLC, new Uri($"http://{ip}:{port}/stream/0"));
                VideoView.MediaPlayer.Play(media);
            }*/
        }

        private void Play(Uri uri)
        {
            try
            {
                VideoView.Play();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string DownRightDirection { get; private set; } = "{\"move\":{\"x\": 0, \"y\": 1, \"z\": 0}}";

        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            mWebSocketClient.SendTextAsync(DownRightDirection);
            //ComunicateHelper.WebSocketSendTextMessage(DownRightDirection);
        }
    }
}
