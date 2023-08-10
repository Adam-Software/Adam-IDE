using AdamControllerLibrary.AdamComunicate;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AdamController.Helpers
{
    public class TcpTestRunHelper
    {
        public static string LastErrorMessage { get; internal set; } = string.Empty;
        public static long TotalErrors { get; internal set; } = 0;
        public static long TotalBytes { get; internal set; } = 0;
        public static DateTime TimestampStop { get; internal set; } = DateTime.UtcNow;
        public static DateTime TimestampStart { get; private set; } = DateTime.UtcNow;
        public static long TotalMessages { get; private set; } = 0;
        public static bool IsTestRun { get; private set; }

        private static byte[] mMessageToSend;
        private static string mAddress;
        private static int mPort;
        private static int mClientsQty;
        private static int mMessagesQty;
        private static int mSize;
        private static int mTestTime;
        private static List<AdamTestTcpClient> mAdamClients;
        
        private static CancellationTokenSource mTokenSource;

        public static void Run()
        {
            IsTestRun = true;
            mTokenSource = new CancellationTokenSource();

            ClearResultField();
            InitTestParam();

            mAdamClients = new();
            mMessageToSend = new byte[mSize];

            for (int i = 0; i < mClientsQty; ++i)
            {
                AdamTestTcpClient client = new(mAddress, mPort, mMessagesQty, mMessageToSend);
                
                mAdamClients.Add(client);
            }

            TimestampStart = DateTime.UtcNow;

            foreach (AdamTestTcpClient client in mAdamClients)
            {
                _ = client.ConnectAsync();
            }

            foreach (AdamTestTcpClient client in mAdamClients)
            {
                while (!client.IsConnected)
                {
                    _ = Thread.Yield();
                }
            }

            _ = mTokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(mTestTime));

            Stop();
        }

        private static void Stop()
        {
            IsTestRun = false;

            if (mAdamClients == null) return;

            TimestampStop = DateTime.UtcNow;

            foreach (AdamTestTcpClient client in mAdamClients)
            {
                TotalBytes += client.TotalBytes;
                LastErrorMessage += client.LastErrorMessage;
                TotalErrors += client.TotalErrors;

                _ = client.DisconnectAsync();
            }

            foreach (AdamTestTcpClient client in mAdamClients)
            {
                while (client.IsConnected)
                {
                    _ = Thread.Yield();
                }
            }

            mTokenSource.Dispose();

            TotalMessages = TotalBytes / mSize;
        }

        public static void AbortTest()
        {
            mTokenSource.Cancel();
        }

        private static void InitTestParam()
        {
            mAddress = Properties.Settings.Default.BenchmarkTestServerIp;
            mPort = Properties.Settings.Default.BenchmarkTcpTestPort;
            mClientsQty = Properties.Settings.Default.BenchmarkTestTcpClientsQty;
            mMessagesQty = Properties.Settings.Default.BenchmarkTestTcpMessageQty;
            mSize = Properties.Settings.Default.BenchmarkTcpSizeByteArray;
            mTestTime = Properties.Settings.Default.BenchmarkTestTcpTime;
        }

        private static void ClearResultField()
        {
            LastErrorMessage = string.Empty;
            TotalErrors = 0;
            TotalBytes = 0;
            TimestampStart = DateTime.UtcNow;
            TimestampStop = DateTime.UtcNow;
            TotalMessages = 0;
        }
    }
}
