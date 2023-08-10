using SimpleUdp;
using System;

namespace AdamControllerProjectsTest.CommonComponents.Servers
{
    /// <summary>
    /// Не используется. 
    /// При запуске в качестве консольного приложения будет работать как UDP-сервер принимающий byte[]  и отображающий в консоли
    /// </summary>
    public class VerboseUdpServer
    {
        public static void Main()
        {
            //UDPServer videoUdpServer = new();
            //UDPServer wheelsUdpServer = new();
            //UDPServer messageUdpServer = new();

            //videoUdpServer.Server("127.0.0.1", 4999);
            //wheelsUdpServer.Server("127.0.0.1", 4998);
            //messageUdpServer.Server("127.0.0.1", 5005);

            UdpEndpoint udp = new("127.0.0.1", 5005);
            udp.DatagramReceived += DatagramReceived;
            udp.StartServer();


            Console.WriteLine("Servers Start");
            _ = Console.Read();
        }

        private static void DatagramReceived(object sender, Datagram e)
        {
            Console.WriteLine("[" + e.Ip + ":" + e.Port + " recivedData: ]");
            Utils.PrintByteArray(e.Data);
        }
    }
}
