using Prism.Mvvm;
using System.Reflection;

namespace AdamController.ViewModels
{
    public class MainWindowViewModel : BindableBase//BaseViewModel
    {


        #region Services

       

        #endregion

        #region Fields

        public string WindowTitle => $"Adam IDE {Assembly.GetExecutingAssembly().GetName().Version}";

        #endregion

        public MainWindowViewModel()
        {
            //ComunicateHelper.OnAdamTcpConnectedEvent += OnTcpConnected;
            //ComunicateHelper.OnAdamTcpDisconnectedEvent += OnTcpDisconnected;
            //ComunicateHelper.OnAdamTcpReconnected += OnTcpReconnected;
            //ComunicateHelper.OnAdamLogServerUdpReceivedEvent += ComunicateHelperOnAdamUdpReceived;
            

            //InitAction();

            //if (Settings.Default.AutoStartTcpConnect)
            //{
            //    ConnectButtonComand.Execute(null);
            //}
            //else
            //{
                //init fields if autorun off
                //TextOnConnectFlayotButton = mConnectButtonStatusDisconnected;
                //TextOnStatusConnectToolbar = mToolbarStatusClientDisconnected;

                //ConnectIcon = PackIconModernKind.Connect;
                //IconOnConnectFlayoutButton = PackIconMaterialKind.RobotDead;
            //}
        }

    }
}
