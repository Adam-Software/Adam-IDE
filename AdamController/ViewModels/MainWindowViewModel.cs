using AdamBlocklyLibrary.Enum;
using AdamController.Commands;
using AdamController.Core.DataSource;
using AdamController.Core.Helpers;
using AdamController.Core.Model;
using AdamController.Core.Properties;
using AdamController.Modules.ContentRegion.ViewModels;
using AdamController.Services;
using AdamController.WebApi.Client.v1;
using MahApps.Metro.IconPacks;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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
