using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace AdamController.Modules.ContentRegion.Views.HamburgerPage
{

    public partial class ComputerVisionControl : UserControl, IDisposable
    {
        private LibVLC mLibVLC;
        private MediaPlayer mMediaPlayer;

        public ComputerVisionControl()
        {
            InitializeComponent();

            VideoView.Loaded += VideoView_Loaded;
            Unloaded += UserControlUnloaded;
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            mLibVLC = new LibVLC(enableDebugLogs: false);
            mMediaPlayer = new MediaPlayer(mLibVLC);

            VideoView.MediaPlayer = mMediaPlayer;

            mMediaPlayer.EnableHardwareDecoding = true;
            mMediaPlayer.NetworkCaching = 1000;
            mMediaPlayer.Scale = 0.72f;
        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                mMediaPlayer.Dispose();
                mLibVLC.Dispose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ip = Core.Properties.Settings.Default.ServerIP;
            string port = Core.Properties.Settings.Default.VideoDataExchangePort;

            if (!VideoView.MediaPlayer.IsPlaying)
            {
                using var media = new Media(mLibVLC, new Uri($"http://{ip}:{port}/stream/0"));
                VideoView.MediaPlayer.Play(media);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //public string DownRightDirection { get; private set; } = "{\"move\":{\"x\": 0, \"y\": 1, \"z\": 0}}";

        /*private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ComunicateHelper.WebSocketSendTextMessage(DownRightDirection);
        }*/
    }
}
