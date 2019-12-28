using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIHellConsole
{
    class Program
    {
        private static void UpdateAscii(string asciiText)
        {
            Console.Write("<" + asciiText + ">");
        }

        private static async void StartReceiving()
        {
            using (var udpClient = new UdpClient(23456))
            {
                try
                {
                    var ipEndpoint = new IPEndPoint(IPAddress.Any, 23456);
                    while (true)
                    {
                        var datagram = await Task.Run<byte[]>(() => udpClient.Receive(ref ipEndpoint));
                        UpdateAscii(Encoding.ASCII.GetString(datagram));
                    }
                }
                catch (SocketException)
                {
                }
            }
        }

        static void Main(string[] args)
        {
            StartReceiving();

            while (true)
            {

                var keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}
