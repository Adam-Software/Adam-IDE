using AdamController.Core.AdamComunicate;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AdamController.Core.Helpers
{
    public class UdpTestRunHelper
    {
        public static string LastErrorMessage { get; internal set; } = string.Empty;
        public static long TotalErrors { get; internal set; } = 0;
        public static long TotalBytes { get; internal set; } = 0;
        public static DateTime TimestampStop { get; internal set; } = DateTime.UtcNow;
        public static DateTime TimestampStart { get; private set; } = DateTime.UtcNow;
        public static long TotalMessages { get; set; } = 0;
        public static bool IsTestRun { get; private set; }

        private static byte[] mMessageToSend;
        private static string mAddress;
        private static int mPort;
        private static int mClientsQty;
        private static int mMessagesQty;
        private static int mSize;
        private static int mSeconds;
        private static List<AdamUdpTestClient> mAdamClients;

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
                AdamUdpTestClient client = new (mAddress, mPort, mMessagesQty, mMessageToSend);
                mAdamClients.Add(client);
            }

            TimestampStart = DateTime.UtcNow;

            foreach (AdamUdpTestClient client in mAdamClients)
            {
                _ = client.Connect();
            }

            foreach (AdamUdpTestClient client in mAdamClients)
            {
                while (!client.IsConnected)
                {
                    _ = Thread.Yield();
                }
            }

            _ = mTokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(mSeconds));

            Stop();
        }

        private static void Stop()
        {
            IsTestRun = false;
            if (mAdamClients == null) return;

            TimestampStop = DateTime.UtcNow;

            foreach (AdamUdpTestClient client in mAdamClients)
            {
                TotalBytes += client.TotalBytes;
                LastErrorMessage += client.LastErrorMessage;
                TotalErrors += client.TotalErrors;

                _ = client.Disconnect();
            }

            foreach (AdamUdpTestClient client in mAdamClients)
            {
                while (client.IsConnected)
                {
                    _ = Thread.Yield();
                }
            }

            mTokenSource.Dispose();

            //?
            TotalMessages = TotalBytes / mSize;
        }

        public static void AbortTest()
        {
            mTokenSource.Cancel();
        }

        private static void InitTestParam()
        {
            mAddress = Core.Properties.Settings.Default.BenchmarkTestServerIp;
            mPort = Core.Properties.Settings.Default.BenchmarkUdpTestPort;
            mClientsQty = Core.Properties.Settings.Default.BenchmarkTestUdpClientsQty;
            mMessagesQty = Core.Properties.Settings.Default.BenchmarkTestUdpMessageQty;
            mSize = Core.Properties.Settings.Default.BenchmarkUdpSizeByteArray;
            mSeconds = Core.Properties.Settings.Default.BenchmarkTestUdpTime;
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
