using AdamControllerLibrary.AdamComunicate;
using AdamControllerProjectsTest.CommonComponents;
using AdamControllerProjectsTest.CommonComponents.Servers;
using NUnit.Framework;
using System;
using System.Threading;

namespace AdamControllerProjectsTest.AdamControllerLibraryTests.AdamTcpUdpClientNativeTest
{
    [TestFixture, Ignore("need refact")]
    public class UdpClientTests 
    {
        private readonly AdamUdpClient mAdamUdpClient;
        private readonly MockMessageUdpServer mMockServer = new();
        private byte[] mRequestBytesReturned;
        //private readonly Service mService;

        public UdpClientTests()
        {
            //mService = new(1);
            //mService.Start();

            //mAdamUdpClient = new(mService, "127.0.0.1", 5005);
            mMockServer.DataReceived += MockDataReceived;
        }


        /// <summary>
        /// Simple test concept
        /// </summary>
        /// <param name="data"></param>
        [TestCase(new byte[] { 0 })]
        [TestCase(new byte[] { 0, 0 })]
        [TestCase(new byte[] { 0, 0, 0, 0 })]
        [TestCase(new byte[] { 0, 0, 0, 0, 0, 0 })]
        [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
        public void AdamSimpleNativeClientTest(byte[] data)
        {
            AdamClientSend(data);

            Assert.AreEqual(data, mRequestBytesReturned);
            
            Console.WriteLine("Actual");
            Utils.PrintByteArray(data);
            Console.WriteLine("Expected");
            Utils.PrintByteArray(mRequestBytesReturned);
        }
        
        private void AdamClientSend(byte[] data)
        {
            mAdamUdpClient.Send(data);
            Thread.Sleep(40);
        }

        private void MockDataReceived(object sender, UpdServerEventArgs e)
        {
            mRequestBytesReturned = e.Data;
        }

    }
}