using AdamController.Core.Helpers;
using AdamController.WebApi.Client.v1;
using AdamController.WebApi.Client.v1.RequestModel;
using MahApps.Metro.IconPacks;
using MessageDialogManagerLib;
using NetCoreServer;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.Core.Dialog.ViewModels
{
    public class NetworkTestViewModel : BindableBase, IDialogAware
    {
        #region Const

        private const string mClientDisconnected = "Статус тестового сервера: отключен";
        private const string mClientConnected = "Статус тестового сервера: подключен";
        private const string mClientReconnected = "Статус тестового сервера: переподключение";

        private const string mTcpTestClientStatusNotRunning = "TCP Round-Trip тест: не запущен";
        private const string mTcpTestClientStatusRunning = "TCP Round-Trip тест: запущен";
        private const string mUdpTestClientStatusNotRunning = "UDP Round-Trip тест: не запущен";
        private const string mUdpTestClientStatusRunning = "UDP Round-Trip тест: запущен";

        #endregion

        private readonly ComunicateBenchmarkHelper mComunicateTestHelper;
        private readonly IMessageDialogManager IDialogManager;

        #region ~

        public NetworkTestViewModel()
        {
            SendApiComunicateCommand(ServerCommand.Start);

            mComunicateTestHelper = ComunicateBenchmarkHelper.Instance;
            IDialogManager = new MessageDialogManagerMahapps(Application.Current);

            ComunicateBenchmarkHelper.OnTcpConnected += OnTcpConnected;
            ComunicateBenchmarkHelper.OnTcpDisconnected += OnTcpDisconnected;
            ComunicateBenchmarkHelper.OnTcpReconnected += OnTcpReconnected;

            ClearTcpResultField();
            TcpStatusBarManager(false);

            ClearUdpResultField();
            UdpStatusBarManager(false);

            if (Properties.Settings.Default.AutoStartTestTcpConnect)
            {
                ConnectButtonComand.Execute();
            }
            else
            {
                //init fields if autorun off
                OnTcpDisconnected();
            }
        }

        #endregion

        #region Navigation

        public string Title => throw new NotImplementedException();

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        #endregion

        #region UI behavior

        //added to activate the reset button, after publishing the results, but does not work
        //activates the window when disconnect, reconnect, connect
        //if you fix the background of the connect button when the window is out of focus, you can delete it
        //can be removed during refactoring
        private bool networkWindowActivated;
        public bool NetworkWindowActivated
        {
            get => networkWindowActivated;
            set
            {
                if (value == networkWindowActivated) return;
                networkWindowActivated = value;
                
                SetProperty(ref networkWindowActivated, value);
            }
        }

        #endregion

        #region Tcp/ip event test client method

        private void OnTcpDisconnected()
        {
            IsTcpClientConnected = false;
            TextOnStatusConnectButton = mClientDisconnected;
            ReconnectProgressRing = false;
            ConnectIcon = PackIconModernKind.Connect;
            NetworkWindowActivated = true;
        }

        private void OnTcpConnected()
        {
            IsTcpClientConnected = true;
            TextOnStatusConnectButton = mClientConnected;
            ReconnectProgressRing = false;
            ConnectIcon = PackIconModernKind.Disconnect;
            NetworkWindowActivated = true;
        }

        private void OnTcpReconnected(int reconnectCount)
        {
            IsTcpClientConnected = null;
            TextOnStatusConnectButton = $"{mClientReconnected} {reconnectCount}";
            ReconnectProgressRing = true;
            ConnectIcon = PackIconModernKind.TransitConnectionDeparture;
            NetworkWindowActivated = true;
        }

        #endregion

        #region IsTcpClientConnected

        private bool? isTcpClientConnected;
        /// <summary>
        /// The field determines the existence of a connection to the test server
        /// null - the state of the reconnect
        /// true - connection
        /// false - disconnection
        /// </summary>
        public bool? IsTcpClientConnected
        {
            get => isTcpClientConnected;
            set
            {
                if (value == isTcpClientConnected) return;

                isTcpClientConnected = value;

                SetProperty(ref isTcpClientConnected, value);
            }
        }

        #endregion

        #region Publish result counter

        #region TCP

        private int publishTcpResultCount;
        public int PublishTcpResultCount
        {
            get => publishTcpResultCount;
            set
            {
                if (value == publishTcpResultCount) return;
                publishTcpResultCount = value;

                SetProperty(ref publishTcpResultCount, value);
            }
        }

        #endregion

        #region UDP

        private int publishUdpResultCount;
        public int PublishUdpResultCount
        {
            get => publishUdpResultCount;
            set
            {
                if (value == publishUdpResultCount) return;
                publishUdpResultCount = value;

                SetProperty(ref publishTcpResultCount, value);
            }
        }

        #endregion

        #endregion

        #region Start test time field

        #region TCP

        private string startTcpTestTime;
        public string StartTcpTestTime
        {
            get => startTcpTestTime;
            set
            {
                if (value == startTcpTestTime) return;

                startTcpTestTime = value;

                SetProperty(ref startTcpTestTime, value);
            }
        }

        #endregion

        #region UDP

        private string startUdpTestTime;
        public string StartUdpTestTime
        {
            get => startUdpTestTime;
            set
            {
                if (value == startUdpTestTime) return;

                startUdpTestTime = value;

                SetProperty (ref startUdpTestTime, value);
            }
        }

        #endregion

        #endregion

        #region Finish test time field

        #region TCP 

        private string finishTcpTestTime;
        public string FinishTcpTestTime
        {
            get => finishTcpTestTime;
            set
            {
                if (value == finishTcpTestTime) return;

                finishTcpTestTime = value;

                SetProperty(ref finishTcpTestTime, value);
            }
        }

        #endregion

        #region UDP

        private string finishUdpTestTime;
        public string FinishUdpTestTime
        {
            get => finishUdpTestTime;
            set
            {
                if (value == finishUdpTestTime) return;

                finishUdpTestTime = value;

                SetProperty(ref finishUdpTestTime, value);
            }
        }

        #endregion

        #region TotalTimeTest field

        #region TCP

        private string totalTimeTcpTest;
        public string TotalTimeTcpTest
        {
            get => totalTimeTcpTest;
            private set
            {
                if (value == totalTimeTcpTest) return;

                totalTimeTcpTest = value;
                
                SetProperty(ref totalTimeTcpTest, value);
            }
        }

        #endregion

        #region UDP

        private string totalTimeUdpTest;
        public string TotalTimeUdpTest
        {
            get => totalTimeUdpTest;
            private set
            {
                if (value == totalTimeUdpTest) return;

                totalTimeUdpTest = value;
                
                SetProperty (ref totalTimeUdpTest, value);
            }
        }

        #endregion

        #endregion

        #endregion

        #region DataCounter field

        #region TCP

        private string dataTcpCounter;
        public string DataTcpCounter
        {
            get => dataTcpCounter;
            set
            {
                if (value == dataTcpCounter) return;
                dataTcpCounter = value;

                SetProperty(ref dataTcpCounter, value);
            }
        }

        #endregion

        #region UDP

        private string dataUdpCounter;
        public string DataUdpCounter
        {
            get => dataUdpCounter;
            set
            {
                if (value == dataUdpCounter) return;

                dataUdpCounter = value;

                SetProperty(ref dataUdpCounter, value);
            }
        }

        #endregion

        #endregion

        #region MessageCounter field

        #region TCP

        private string messageTcpCounter;
        public string MessageTcpCounter
        {
            get => messageTcpCounter;
            set
            {
                if (value == messageTcpCounter) return;

                messageTcpCounter = value;

                SetProperty(ref messageTcpCounter, value);
            }
        }

        #endregion

        #region UDP

        private string messageUdpCounter;
        public string MessageUdpCounter
        {
            get => messageUdpCounter;
            set
            {
                if (value == messageUdpCounter) return;

                messageUdpCounter = value;
                
                SetProperty(ref messageUdpCounter, value);
            }
        }

        #endregion

        #endregion

        #region DataThroughput field

        #region TCP

        private string dataTcpThroughput;
        public string DataTcpThroughput
        {
            get => dataTcpThroughput;
            private set
            {
                if (value == dataTcpThroughput) return;
                dataTcpThroughput = value;

                SetProperty (ref dataTcpThroughput, value);
            }
        }

        #endregion

        #region UDP

        private string dataUdpThroughput;
        public string DataUdpThroughput
        {
            get => dataUdpThroughput;
            private set
            {
                if (value == dataUdpThroughput) return;
                dataUdpThroughput = value;

                SetProperty(ref dataUdpThroughput, value);
            }
        }

        #endregion

        #endregion

        #region MessageLatency field

        #region TCP

        private string messageTcpLatency;
        public string MessageTcpLatency
        {
            get => messageTcpLatency;
            private set
            {
                if (value == messageTcpLatency) return;
                messageTcpLatency = value;

                SetProperty(ref messageTcpLatency, value);
            }
        }

        #endregion

        #region UDP

        private string messageUdpLatency;
        public string MessageUdpLatency
        {
            get => messageUdpLatency;
            private set
            {
                if (value == messageUdpLatency) return;
                messageUdpLatency = value;

                SetProperty(ref messageUdpLatency, value);
            }
        }

        #endregion

        #endregion

        #region MessageThroughput field

        #region TCP

        private string messageTcpThroughput;
        public string MessageTcpThroughput
        {
            get => messageTcpThroughput;
            private set
            {
                if (value == messageTcpThroughput) return;
                messageTcpThroughput = value;

                SetProperty(ref messageTcpThroughput, value);
            }
        }

        #endregion

        #region UDP

        private string messageUdpThroughput;
        public string MessageUdpThroughput
        {
            get => messageUdpThroughput;
            private set
            {
                if (value == messageUdpThroughput) return;
                messageUdpThroughput = value;

                SetProperty(ref messageUdpThroughput, value);
            }
        }

        #endregion

        #endregion

        #region Error counter field

        #region TCP

        private string errorTcpCounter;
        public string ErrorTcpCounter
        {
            get => errorTcpCounter;
            set
            {
                if (value == errorTcpCounter) return;
                errorTcpCounter = value;

                SetProperty(ref errorTcpCounter, value);
            }
        }

        #endregion

        #region UDP

        private string errorUdpCounter;
        public string ErrorUdpCounter
        {
            get => errorUdpCounter;
            set
            {
                if (value == errorUdpCounter) return;
                errorUdpCounter = value;

                SetProperty(ref errorUdpCounter, value);
            }
        }

        #endregion

        #endregion

        #region Error message

        #region TCP

        private string errorTcpMessage;
        public string ErrorTcpMessage
        {
            get => errorTcpMessage;
            set
            {
                if (value == errorTcpMessage) return;
                errorTcpMessage = value;

                SetProperty(ref errorTcpMessage, value);
            }
        }

        #endregion

        #region UDP

        private string errorUdpMessage;
        public string ErrorUdpMessage
        {
            get => errorUdpMessage;
            set
            {
                if (value == errorUdpMessage) return;
                errorUdpMessage = value;

                SetProperty(ref errorUdpMessage, value);
            }
        }

        #endregion

        #endregion

        #region ProgressRing field

        #region TCP

        private bool progressRingTcp = false;
        public bool ProgressRingTcp
        {
            get => progressRingTcp;
            set
            {
                if (value == progressRingTcp) return;

                progressRingTcp = value;
                
                SetProperty(ref progressRingTcp, value);
            }
        }

        #endregion

        #region UDP

        private bool progressRingUdp = false;
        public bool ProgressRingUdp
        {
            get => progressRingUdp;
            set
            {
                if (value == progressRingUdp) return;

                progressRingUdp = value;
                
                SetProperty(ref progressRingUdp, value);
            }
        }

        #endregion

        #region ReconnectProgressRing

        private bool reconnectProgressRing;
        public bool ReconnectProgressRing
        {
            get => reconnectProgressRing;
            set
            {
                if (value == reconnectProgressRing) return;

                reconnectProgressRing = value;
                
                SetProperty (ref reconnectProgressRing, value);
            }
        }

        #endregion

        #endregion

        #region SelectedTabIndex

        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                if (value == selectedTabIndex)
                    return;

                selectedTabIndex = value;
                
                SetProperty(ref selectedTabIndex, value);
            }
        }

        #endregion

        #region Environment tabs fields

        public int ProcessorCount { get; } = Environment.ProcessorCount;
        public bool Is64BitOperatingSystem { get; } = Environment.Is64BitOperatingSystem;
        public bool Is64BitProcess { get; } = Environment.Is64BitProcess;
        public string MachineName { get; } = Environment.MachineName;
        public string OSVersion { get; } = Environment.OSVersion.ToString();
        public string OSDescription { get; } = RuntimeInformation.OSDescription;
        public string UserName { get; } = Environment.UserName;
        public int TickCount { get; } = Environment.TickCount;
        public long WorkingSet { get; } = Environment.WorkingSet;
     

        #endregion

        #region Main Connect/Disconnect client toolbar

        private string textOnStatusConnectButton = mClientDisconnected;
        public string TextOnStatusConnectButton
        {
            get => textOnStatusConnectButton;
            set
            {
                if (value == textOnStatusConnectButton) return;

                textOnStatusConnectButton = value;
                SetProperty(ref textOnStatusConnectButton, value);
            }
        }

        private PackIconModernKind connectIcon = PackIconModernKind.Connect;
        public PackIconModernKind ConnectIcon
        {
            get => connectIcon;
            set
            {
                if (value == connectIcon) return;

                connectIcon = value;
                SetProperty(ref connectIcon, value);
            }
        }

        #endregion

        #region Status TCP test toolbar

        private string textOnTcpStatusTest = mTcpTestClientStatusRunning;
        public string TextOnTcpStatusTest
        {
            get => textOnTcpStatusTest;
            set
            {
                if (value == textOnTcpStatusTest) return;

                textOnTcpStatusTest = value;

                SetProperty(ref textOnTcpStatusTest, value);
            }
        }

        private PackIconMaterialKind tcpTestIcon = PackIconMaterialKind.TestTubeOff;
        public PackIconMaterialKind TcpTestIcon
        {
            get => tcpTestIcon;
            set
            {
                if (value == tcpTestIcon) return;

                tcpTestIcon = value;
                
                SetProperty(ref tcpTestIcon, value);
            }
        }

        #endregion

        #region Status UDP test toolbar

        private string textOnUdpStatusTest = mUdpTestClientStatusNotRunning;
        public string TextOnUdpStatusTest
        {
            get => textOnUdpStatusTest;
            set
            {
                if (value == textOnUdpStatusTest) return;

                textOnUdpStatusTest = value;

                SetProperty(ref textOnUdpStatusTest, value);
            }
        }

        private PackIconMaterialKind udpTestIcon = PackIconMaterialKind.TestTubeOff;
        public PackIconMaterialKind UdpTestIcon
        {
            get => udpTestIcon;
            set
            {
                if (value == udpTestIcon) return;

                udpTestIcon = value;

                SetProperty(ref udpTestIcon, value);
            }
        }

        #endregion

        #region Param managment

        #region TCP

        private bool serverIpTcpBoxIsEnabled;
        /// <summary>
        /// The state of this element (on/off) depends on the state of other input fields (linked via Binding ElementName)
        /// If true, then the test is running, false otherwise
        /// </summary>
        public bool ServerIpTcpBoxIsEnabled
        {
            get => serverIpTcpBoxIsEnabled;
            set
            {
                if (value == serverIpTcpBoxIsEnabled) return;
                serverIpTcpBoxIsEnabled = value;

                SetProperty (ref serverIpTcpBoxIsEnabled, value);
            }
        }
        #endregion

        #region UDP

        private bool serverIpUdpBoxIsEnabled;
        /// <summary>
        /// The state of this element (on/off) depends on the state of other input fields (linked via Binding ElementName)
        /// If true, then the test is running, false otherwise
        /// </summary>
        public bool ServerIpUdpBoxIsEnabled
        {
            get => serverIpUdpBoxIsEnabled;
            set
            {
                if (value == serverIpUdpBoxIsEnabled) return;
                serverIpUdpBoxIsEnabled = value;

                SetProperty(ref serverIpUdpBoxIsEnabled, value);
            }
        }

        #endregion

        #endregion

        #region StatusBar Icons and Text manage

        #region TCP

        private void TcpStatusBarManager(bool isTestRun)
        {
            if (isTestRun)
            {
                ServerIpTcpBoxIsEnabled = false;
                ProgressRingTcp = true;
                TextOnTcpStatusTest = mTcpTestClientStatusRunning;
                TcpTestIcon = PackIconMaterialKind.TestTube;
                return;
            }

            ServerIpTcpBoxIsEnabled = true;
            ProgressRingTcp = false;
            TextOnTcpStatusTest = mTcpTestClientStatusNotRunning;
            TcpTestIcon = PackIconMaterialKind.TestTubeOff;
        }

        #endregion

        #region UDP

        private void UdpStatusBarManager(bool isTestRun)
        {
            if (isTestRun)
            {
                ServerIpUdpBoxIsEnabled = false;
                ProgressRingUdp = true;
                TextOnUdpStatusTest = mUdpTestClientStatusRunning;
                UdpTestIcon = PackIconMaterialKind.TestTube;
                return;
            }

            ServerIpUdpBoxIsEnabled = true;
            ProgressRingUdp = false;
            TextOnUdpStatusTest = mUdpTestClientStatusNotRunning;
            UdpTestIcon = PackIconMaterialKind.TestTubeOff;
        }

        #endregion

        #endregion

        #region TCP methods

        private void ClearTcpResultField()
        {
            PublishTcpResultCount = 0;

            //time result
            TotalTimeTcpTest = "--:--:--";
            StartTcpTestTime = "--:--:--";
            FinishTcpTestTime = "--:--:--";

            //data result
            DataTcpCounter = "------";
            DataTcpThroughput = "------";

            //message result
            MessageTcpCounter = "------";
            MessageTcpLatency = "------";
            MessageTcpThroughput = "------";

            //error result
            ErrorTcpCounter = "------";
            ErrorTcpMessage = "------";
        }

        private void PublishTcpResults()
        {
            PublishTcpResultCount++;

            //time result
            TotalTimeTcpTest = Utilities.GenerateTimePeriod((TcpTestRunHelper.TimestampStop - TcpTestRunHelper.TimestampStart).TotalMilliseconds);
            StartTcpTestTime = TcpTestRunHelper.TimestampStart.ToLocalTime().ToLongTimeString();
            FinishTcpTestTime = TcpTestRunHelper.TimestampStop.ToLocalTime().ToLongTimeString();

            if (TcpTestRunHelper.TotalBytes == 0) return;

            //data result
            DataTcpCounter = Utilities.GenerateDataSize(TcpTestRunHelper.TotalBytes);
            DataTcpThroughput = $"{Utilities.GenerateDataSize(TcpTestRunHelper.TotalBytes / (TcpTestRunHelper.TimestampStop - TcpTestRunHelper.TimestampStart).TotalSeconds)}/s";

            //message result
            MessageTcpCounter = TcpTestRunHelper.TotalMessages.ToString();
            MessageTcpLatency = $"{Utilities.GenerateTimePeriod((TcpTestRunHelper.TimestampStop - TcpTestRunHelper.TimestampStart).TotalMilliseconds / TcpTestRunHelper.TotalMessages)}";
            MessageTcpThroughput = $"{(long)(TcpTestRunHelper.TotalMessages / (TcpTestRunHelper.TimestampStop - TcpTestRunHelper.TimestampStart).TotalSeconds)} msg/s";

            //error result
            ErrorTcpCounter = TcpTestRunHelper.TotalErrors.ToString();
            ErrorTcpMessage = TcpTestRunHelper.LastErrorMessage;

            if (string.IsNullOrEmpty(TcpTestRunHelper.LastErrorMessage))
            {
                ErrorTcpMessage = "Нет ошибок";
            }

            TcpStatusBarManager(false);
            NetworkWindowActivated = true;
        }

        private List<string> SaveTcpResultsToList()
        {
            List<string> tcpResults = new()
            {
                $"Time results",
                $"StartTcpTestTime {StartTcpTestTime}",
                $"FinishTcpTestTime {FinishTcpTestTime}",
                $"FactTotalTimeTcpTest {TotalTimeTcpTest}",
                $"",
                $"Data results",
                $"DataTcpCounter {DataTcpCounter}",
                $"MessageTcpCounter {MessageTcpCounter}",
                $"DataTcpThroughput {DataTcpThroughput}",
                $"MessageTcpLatency {MessageTcpLatency}",
                $"MessageTcpThroughput {MessageTcpThroughput}",
                $"ErrorTcpCounter {ErrorTcpCounter}",
                $"LastErrorTcpMessage {ErrorTcpMessage}",
                $"",
                $"Test parameters",
                $"BenchmarkTestServerIp {Properties.Settings.Default.BenchmarkTestServerIp}",
                $"BenchmarkTcpTestPort {Properties.Settings.Default.BenchmarkTcpTestPort}",
                $"BenchmarkTcpSizeByteArray {Properties.Settings.Default.BenchmarkTcpSizeByteArray}",
                $"BenchmarkTestTcpTime {Properties.Settings.Default.BenchmarkTestTcpTime}",
                $"BenchmarkTestTcpMessageQty {Properties.Settings.Default.BenchmarkTestTcpMessageQty}",
                $"BenchmarkTestTcpClientsQty {Properties.Settings.Default.BenchmarkTestTcpClientsQty}"
            };

            return tcpResults;
        }

        #endregion

        #region UDP methods

        private void ClearUdpResultField()
        {
            PublishUdpResultCount = 0;
            //time result
            TotalTimeUdpTest = "--:--:--";
            StartUdpTestTime = "--:--:--";
            FinishUdpTestTime = "--:--:--";

            //data result
            DataUdpCounter = "------";
            DataUdpThroughput = "------";

            //message result
            MessageUdpCounter = "------";
            MessageUdpLatency = "------";
            MessageUdpThroughput = "------";

            //error result
            ErrorUdpCounter = "------";
            ErrorUdpMessage = "------";
        }

        private void PublishUdpResults()
        {
            PublishUdpResultCount++;

            //time result
            TotalTimeUdpTest = Utilities.GenerateTimePeriod((UdpTestRunHelper.TimestampStop - UdpTestRunHelper.TimestampStart).TotalMilliseconds);
            StartUdpTestTime = UdpTestRunHelper.TimestampStart.ToLocalTime().ToLongTimeString();
            FinishUdpTestTime = UdpTestRunHelper.TimestampStop.ToLocalTime().ToLongTimeString();

            if (UdpTestRunHelper.TotalBytes == 0) return;

            //data result
            DataUdpCounter = Utilities.GenerateDataSize(UdpTestRunHelper.TotalBytes);
            DataUdpThroughput = $"{Utilities.GenerateDataSize(UdpTestRunHelper.TotalBytes / (UdpTestRunHelper.TimestampStop - UdpTestRunHelper.TimestampStart).TotalSeconds)}/s";

            //message result
            MessageUdpCounter = UdpTestRunHelper.TotalMessages.ToString();
            MessageUdpLatency = $"{Utilities.GenerateTimePeriod((UdpTestRunHelper.TimestampStop - UdpTestRunHelper.TimestampStart).TotalMilliseconds / UdpTestRunHelper.TotalMessages)}";
            MessageUdpThroughput = $"{(long)(UdpTestRunHelper.TotalMessages / (UdpTestRunHelper.TimestampStop - UdpTestRunHelper.TimestampStart).TotalSeconds)} msg/s";

            //error result
            ErrorUdpCounter = UdpTestRunHelper.TotalErrors.ToString();
            ErrorUdpMessage = UdpTestRunHelper.LastErrorMessage;

            if (string.IsNullOrEmpty(UdpTestRunHelper.LastErrorMessage))
            {
                ErrorUdpMessage = "Нет ошибок";
            }

            UdpStatusBarManager(false);

            NetworkWindowActivated = true;
        }

        private List<string> SaveUdpResultsToList()
        {
            List<string> tcpResults = new()
            {
                $"Time results",
                $"StartUdpTestTime {StartUdpTestTime}",
                $"FinishTcpTestTime {FinishUdpTestTime}",
                $"FactTotalTimeTcpTest {TotalTimeUdpTest}",
                $"",
                $"Data results",
                $"DataTcpCounter {DataUdpCounter}",
                $"MessageTcpCounter {MessageUdpCounter}",
                $"DataTcpThroughput {DataUdpThroughput}",
                $"MessageTcpLatency {MessageUdpLatency}",
                $"MessageTcpThroughput {MessageUdpThroughput}",
                $"ErrorTcpCounter {ErrorUdpCounter}",
                $"LastErrorTcpMessage {ErrorUdpMessage}",
                $"",
                $"Test parameters",
                $"BenchmarkTestServerIp {Properties.Settings.Default.BenchmarkTestServerIp}",
                $"BenchmarkTcpTestPort {Properties.Settings.Default.BenchmarkUdpTestPort}",
                $"BenchmarkTcpTestPort {Properties.Settings.Default.BenchmarkUdpSizeByteArray}",
                $"BenchmarkTcpSizeByteArray {Properties.Settings.Default.BenchmarkUdpSizeByteArray}",
                $"BenchmarkTestTcpTime {Properties.Settings.Default.BenchmarkTestUdpTime}",
                $"BenchmarkTestTcpMessageQty {Properties.Settings.Default.BenchmarkTestUdpMessageQty}",
                $"BenchmarkTestTcpClientsQty {Properties.Settings.Default.BenchmarkTestUdpClientsQty}"
            };

            return tcpResults;
        }

        #endregion

        #region Common methods

        private static string ConvertListToString(IList<string> results = null, IList<string> envParams = null)
        {
            string tempResults = string.Empty;
            
            if(envParams != null)
            {
                foreach (string @string in envParams)
                {
                    tempResults += $"{@string}\n";
                }
            }

            if(results != null)
            {
                foreach (string @string in results)
                {
                    tempResults += $"{@string}\n";
                }
            }

            return tempResults;
        }

        /// <summary>
        /// Writes environment parameters to a string array
        /// </summary>
        /// <param name="shortFormat">Use the short format when you need to get basic information about the environment</param>
        private IList<string> SaveEnvironmentParametersToList(bool shortFormat)
        {
            List<string> envParams = shortFormat 
                ? (new()
                {
                    $"Environment parameters",
                    $"MachineName {MachineName}",
                    $"OSVersion {OSVersion}",
                    $"OSDescription {OSDescription}",
                    $""
                }) 
                : (new()
                {
                    $"ProcessorCount {ProcessorCount}",
                    $"Is64BitOperatingSystem {Is64BitOperatingSystem}",
                    $"Is64BitProcess {Is64BitProcess}",
                    $"MachineName {MachineName}",
                    $"OSVersion {OSVersion}",
                    $"OSDescription {OSDescription}",
                    $"UserName {UserName}",
                    $"TickCount {TickCount}",
                    $"WorkingSet {WorkingSet}",
                    $""
                });

            return envParams;    
        }

        private void OnCloseWindow()
        {
            if (TcpTestRunHelper.IsTestRun)
            {
                TcpTestRunHelper.AbortTest();
            }

            if (UdpTestRunHelper.IsTestRun)
            {
                UdpTestRunHelper.AbortTest();
            }
                
            mComunicateTestHelper.DisconnectAndDestroy();

            SendApiComunicateCommand(ServerCommand.Stop);
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Api Calls
        
        private void SendApiComunicateCommand(ServerCommand command)
        {
            BaseApi.SendCommand(command, "BenchmarkStateTcpServer");
            BaseApi.SendCommand(command, "BenchmarkTcpServer");
            BaseApi.SendCommand(command, "BenchmarkUdpServer");
        }

        #endregion

        #region Commands

        #region TCP commands

        private DelegateCommand stopTcpBenchmarkTest;
        public DelegateCommand StopTcpBenchmarkTest => stopTcpBenchmarkTest ??= new DelegateCommand(() =>
        {
            TcpTestRunHelper.AbortTest();
        });

        private DelegateCommand clearTcpTestResult;
        public DelegateCommand ClearTcpTestResult => clearTcpTestResult ??= new DelegateCommand(() =>
        {
            ClearTcpResultField();
        }, () => PublishTcpResultCount > 0);

        private DelegateCommand startTcpBenchmarkTest;
        public DelegateCommand StartTcpBenchmarkTest => startTcpBenchmarkTest ??= new DelegateCommand(async () =>
        {
            TcpStatusBarManager(true);

            await Dispatcher.Yield(DispatcherPriority.Normal);
            await Task.Run (() => TcpTestRunHelper.Run());
            
            PublishTcpResults();
        }, () => IsTcpClientConnected == true);

        #endregion

        #region UDP commands

        private DelegateCommand stopUdpBenchmarkTest;
        public DelegateCommand StopUdpBenchmarkTest => stopUdpBenchmarkTest ??= new DelegateCommand(() =>
        {
            UdpTestRunHelper.AbortTest();
        });

        private DelegateCommand clearUdpTestResult;
        public DelegateCommand ClearUdpTestResult => clearUdpTestResult ??= new DelegateCommand(() =>
        {
            ClearUdpResultField();
        }, () => PublishUdpResultCount > 0);

        private DelegateCommand startUdpBenchmarkTest;
        public DelegateCommand StartUdpBenchmarkTest => startUdpBenchmarkTest ??= new DelegateCommand(async () =>
        {
            UdpStatusBarManager(true);

            await Dispatcher.Yield(DispatcherPriority.Normal);
            await Task.Run(() => UdpTestRunHelper.Run());

            PublishUdpResults();
        }, () => IsTcpClientConnected == true);


        #endregion

        #region Test client connect commands

        private DelegateCommand connectButtonComand;
        public DelegateCommand ConnectButtonComand => connectButtonComand ??= new DelegateCommand(async () =>
        {
            await Dispatcher.Yield(DispatcherPriority.Normal);

            if (mComunicateTestHelper.TcpClientIsConnected)
            {
                await Task.Run(() => mComunicateTestHelper.Disconnect());
                return;
            }

            if (!mComunicateTestHelper.TcpClientIsConnected)
            {
                await Task.Run(() => mComunicateTestHelper.Connect());
                return;
            }
        });

        #endregion

        #region Saved/Copy buttons commands

        private DelegateCommand copyResults;
        public DelegateCommand CopyResults => copyResults ??= new DelegateCommand(() => 
        {
            IList<string> envParamsShort = null;
            
            if (Properties.Settings.Default.BenchmarkAddEnvironmentParametersToResult)
            {
                envParamsShort = SaveEnvironmentParametersToList(true);
            }

            switch (SelectedTabIndex)
            {
                case 0:
                    {
                        IList<string> envParamsFull = SaveEnvironmentParametersToList(false);
                        string results = ConvertListToString(envParams: envParamsFull);
                        Clipboard.SetText(results);
                        break;
                    }
                case 1:
                    {
                        IList<string> tcpResults = SaveTcpResultsToList();
                        string results = ConvertListToString(tcpResults, envParamsShort);
                        Clipboard.SetText(results);
                        break;
                    }
                case 2:
                    {
                        IList<string> udpResults = SaveUdpResultsToList();
                        string results = ConvertListToString(udpResults, envParamsShort);
                        Clipboard.SetText(results);
                        break;
                    }

                default:
                    break;
            }
        });

        private DelegateCommand saveResults;

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand SaveResults => saveResults ??= new DelegateCommand(() => 
        {
            IList<string> envParamsShort = null;

            if (Properties.Settings.Default.BenchmarkAddEnvironmentParametersToResult)
            {
                envParamsShort = SaveEnvironmentParametersToList(true);
            }

            switch (SelectedTabIndex)
            {
                case 0:
                    {
                        IList<string> envParamsFull = SaveEnvironmentParametersToList(false);
                        string results = ConvertListToString(envParams: envParamsFull);
                        FileSaveDialog(results, "Сохранить параметры окружения", "EnvironmentParameters");
                        break;
                    }
                case 1:
                    {
                        IList<string> tcpResults = SaveTcpResultsToList();
                        string results = ConvertListToString(tcpResults, envParamsShort);
                        FileSaveDialog(results, "Сохранить результаты TCP RoundTrip теста", $"TCP benchmark results {DateTime.Now:HH-mm-ss.ff}");
                        break;
                    }
                case 2:
                    {
                        IList<string> udpResults = SaveUdpResultsToList();
                        string results = ConvertListToString(udpResults, envParamsShort);
                        FileSaveDialog(results, "Сохранить результаты UDP RoundTrip теста", $"UDP benchmark results {DateTime.Now:HH-mm-ss.ff}");
                        break;
                    }

                default:
                    break;
            }

        });

       

        private async void FileSaveDialog(string file, string title, string fileName)
        {

            if (IDialogManager.ShowSaveFileDialog(title, Properties.Settings.Default.SavedResultsNetworkTestsFolderPath, fileName, ".txt", "TXT documents (.txt)|*.txt"))
            {
                string path = IDialogManager.FilePathToSave;
                await FileHelper.WriteAsync(path, file);
            }
            else
            {
                //AppLogStatusBarAction("Файл не сохранен");
            }
        }

        #endregion

        #endregion

    }
}
