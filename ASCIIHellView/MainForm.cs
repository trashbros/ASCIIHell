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
            Invoke((Action)(() => { TextAcii.Text = asciiText.Replace("\n", "\r\n"); }));
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
    }
}
