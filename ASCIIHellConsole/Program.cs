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
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            Console.Write(asciiText);
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
                else
                {
                    byte[] datagram = new byte[0];

                    if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.W)
                    {
                        datagram = Encoding.ASCII.GetBytes("U");
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.S)
                    {
                        datagram = Encoding.ASCII.GetBytes("D");
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.A)
                    {
                        datagram = Encoding.ASCII.GetBytes("L");
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow || keyInfo.Key == ConsoleKey.D)
                    {
                        datagram = Encoding.ASCII.GetBytes("R");
                    }
                    else if (keyInfo.Key == ConsoleKey.E)
                    {
                        // Start button
                        datagram = Encoding.ASCII.GetBytes("E");
                    }
                    else if (keyInfo.Key == ConsoleKey.Q)
                    {
                        // Quit button
                        datagram = Encoding.ASCII.GetBytes("Q");
                    }
                    else if (keyInfo.Key == ConsoleKey.F)
                    {
                        // Fire projectiles
                        datagram = Encoding.ASCII.GetBytes("F");
                    }
                    else if (keyInfo.Key == ConsoleKey.X)
                    {
                        // Slow Down Bullets
                        datagram = Encoding.ASCII.GetBytes("X");
                    }

                    if (datagram.Length > 0)
                    {
                        UdpClient udpSender = new UdpClient("127.0.0.1", 65432);
                        try
                        {
                            udpSender.Send(datagram, datagram.Length);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
        }
    }
}
