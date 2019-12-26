using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCIIHellView
{
    public partial class MainForm : Form
    {
        private UdpClient udpClient = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            BtnStart.Enabled = false;
            BtnStop.Enabled = true;

            StartReceiving();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            BtnStart.Enabled = true;
            BtnStop.Enabled = false;

            StopReceiving();
        }

        private void UpdateAscii(string asciiText)
        {
            Invoke((Action)(() =>
            {
                TextAscii.SuspendLayout();
                TextAscii.Text = asciiText.Replace("\n", "\r\n");
                TextAscii.ResumeLayout();
            }));
        }

        private async void StartReceiving()
        {
            using (udpClient = new UdpClient(23456))
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

        private void StopReceiving()
        {
            udpClient?.Close();
        }

        private void TextAscii_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TextAscii_KeyDown(object sender, KeyEventArgs e)
        {
            byte[] datagram = new byte[0];

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                datagram = Encoding.ASCII.GetBytes("U");
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                datagram = Encoding.ASCII.GetBytes("D");
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                datagram = Encoding.ASCII.GetBytes("L");
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                datagram = Encoding.ASCII.GetBytes("R");
            }
            else if (e.KeyCode == Keys.E)
            {
                datagram = Encoding.ASCII.GetBytes("E");
            }
            else if (e.KeyCode == Keys.Q)
            {
                datagram = Encoding.ASCII.GetBytes("Q");
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

            e.Handled = true;
        }

        private void TextAscii_KeyUp(object sender, KeyEventArgs e)
        {
            //byte[] datagram = new byte[0];

            //if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            //{
            //    datagram = Encoding.ASCII.GetBytes("u");
            //}
            //else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            //{
            //    datagram = Encoding.ASCII.GetBytes("d");
            //}
            //else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            //{
            //    datagram = Encoding.ASCII.GetBytes("l");
            //}
            //else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            //{
            //    datagram = Encoding.ASCII.GetBytes("r");
            //}
            //else if (e.KeyCode == Keys.E)
            //{
            //    datagram = Encoding.ASCII.GetBytes("e");
            //}
            //else if (e.KeyCode == Keys.Q)
            //{
            //    datagram = Encoding.ASCII.GetBytes("q");
            //}

            //if (datagram.Length > 0)
            //{
            //    UdpClient udpSender = new UdpClient("127.0.0.1", 65432);
            //    try
            //    {
            //        udpSender.Send(datagram, datagram.Length);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //    }
            //}

            e.Handled = true;
        }
    }
}
