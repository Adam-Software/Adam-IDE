using AdamController.Commands;
using AdamController.Helpers;
using AdamController.ViewModels.Common;
using AdamController.WebApi.Client.v1;
using AdamController.WebApi.Client.v1.RequestModel;
using MahApps.Metro.IconPacks;
using MessageDialogManagerLib;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AdamController.ViewModels
{
    public class NetworkTestView : BaseViewModel
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

        public NetworkTestView()
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
                ConnectButtonComand.Execute(null);
            }
            else
            {
                //init fields if autorun off
                OnTcpDisconnected();
            }
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
                OnPropertyChanged(nameof(NetworkWindowActivated));

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
                OnPropertyChanged(nameof(IsTcpClientConnected));
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

                OnPropertyChanged(nameof(PublishTcpResultCount));
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

                OnPropertyChanged(nameof(PublishUdpResultCount));
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
                OnPropertyChanged(nameof(StartTcpTestTime));
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
                OnPropertyChanged(nameof(StartUdpTestTime));
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
                OnPropertyChanged(nameof(FinishTcpTestTime));
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
                OnPropertyChanged(nameof(FinishUdpTestTime));
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
                OnPropertyChanged(nameof(TotalTimeTcpTest));
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
                OnPropertyChanged(nameof(TotalTimeUdpTest));
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

                OnPropertyChanged(nameof(DataTcpCounter));
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

                OnPropertyChanged(nameof(DataUdpCounter));
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
                OnPropertyChanged(nameof(MessageTcpCounter));
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
                OnPropertyChanged(nameof(MessageUdpCounter));
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

                OnPropertyChanged(nameof(DataTcpThroughput));
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

                OnPropertyChanged(nameof(DataUdpThroughput));
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

                OnPropertyChanged(nameof(MessageTcpLatency));
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

                OnPropertyChanged(nameof(MessageUdpLatency));
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

                OnPropertyChanged(nameof(MessageTcpThroughput));
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

                OnPropertyChanged(nameof(MessageUdpThroughput));
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

                OnPropertyChanged(nameof(ErrorTcpCounter));
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

                OnPropertyChanged(nameof(ErrorUdpCounter));
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

                OnPropertyChanged(nameof(ErrorTcpMessage));
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

                OnPropertyChanged(nameof(ErrorUdpMessage));
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
                OnPropertyChanged(nameof(ProgressRingTcp));
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
                OnPropertyChanged(nameof(ProgressRingUdp));
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
                OnPropertyChanged(nameof(ReconnectProgressRing));
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
                OnPropertyChanged(nameof(SelectedTabIndex));
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
                OnPropertyChanged(nameof(TextOnStatusConnectButton));
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
                OnPropertyChanged(nameof(ConnectIcon));
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
                OnPropertyChanged(nameof(TextOnTcpStatusTest));
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
                OnPropertyChanged(nameof(TcpTestIcon));
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
                OnPropertyChanged(nameof(TextOnUdpStatusTest));
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
                OnPropertyChanged(nameof(UdpTestIcon));
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

                OnPropertyChanged(nameof(ServerIpTcpBoxIsEnabled));
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

                OnPropertyChanged(nameof(ServerIpUdpBoxIsEnabled));
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

        private RelayCommand stopTcpBenchmarkTest;
        public RelayCommand StopTcpBenchmarkTest => stopTcpBenchmarkTest ??= new RelayCommand(obj =>
        {
            TcpTestRunHelper.AbortTest();
        });

        private RelayCommand clearTcpTestResult;
        public RelayCommand ClearTcpTestResult => clearTcpTestResult ??= new RelayCommand(obj =>
        {
            ClearTcpResultField();
        }, canExecute => PublishTcpResultCount > 0);

        private RelayCommand startTcpBenchmarkTest;
        public RelayCommand StartTcpBenchmarkTest => startTcpBenchmarkTest ??= new RelayCommand(async obj =>
        {
            TcpStatusBarManager(true);

            await Dispatcher.Yield(DispatcherPriority.Normal);
            await Task.Run (() => TcpTestRunHelper.Run());
            
            PublishTcpResults();
        }, canExecute => IsTcpClientConnected == true);

        #endregion

        #region UDP commands

        private RelayCommand stopUdpBenchmarkTest;
        public RelayCommand StopUdpBenchmarkTest => stopUdpBenchmarkTest ??= new RelayCommand(obj =>
        {
            UdpTestRunHelper.AbortTest();
        });

        private RelayCommand clearUdpTestResult;
        public RelayCommand ClearUdpTestResult => clearUdpTestResult ??= new RelayCommand(obj =>
        {
            ClearUdpResultField();
        }, canExecute => PublishUdpResultCount > 0);

        private RelayCommand startUdpBenchmarkTest;
        public RelayCommand StartUdpBenchmarkTest => startUdpBenchmarkTest ??= new RelayCommand(async obj =>
        {
            UdpStatusBarManager(true);

            await Dispatcher.Yield(DispatcherPriority.Normal);
            await Task.Run(() => UdpTestRunHelper.Run());

            PublishUdpResults();
        }, canExecute => IsTcpClientConnected == true);


        #endregion

        #region Common comands

        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand => closeWindowCommand ??= new RelayCommand(obj =>
        {
            OnCloseWindow();

            ((Window)obj).Close();
        });

        #endregion

        #region Test client connect commands

        private RelayCommand connectButtonComand;
        public RelayCommand ConnectButtonComand => connectButtonComand ??= new RelayCommand(async obj =>
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

        private RelayCommand copyResults;
        public RelayCommand CopyResults => copyResults ??= new RelayCommand(obj => 
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

        private RelayCommand saveResults;
        public RelayCommand SaveResults => saveResults ??= new RelayCommand(obj => 
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

        #region IWindowParam

        public override string WindowTitle => @"Тест сети";
        public override double Width => 1000;
        public override double Height => 550;
        public override bool IsModal => false;

        public override void OnClosed(Window window)
        {
            OnCloseWindow();

            base.OnClosed(window);
        }

        #endregion
    }
}
