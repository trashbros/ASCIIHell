using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    private UdpClient udpTx;
    private UdpClient udpRx;
    private readonly IPEndPoint ipEndPoint;

    private TcpListener tcpListener;
    private TcpClient tcpClient;
    private BinaryWriter writer;
    private BinaryReader reader;

    enum NetworkTransportType
    {
        None,
        UDP,
        TCP
    }

    [SerializeField]
    private NetworkTransportType networkTransportType = NetworkTransportType.None;


    public InputHandler inputController;

    // Start is called before the first frame update
    void Start()
    {
        if (inputController == null)
        {
            inputController = GetComponent<InputHandler>();
        }

        if (networkTransportType == NetworkTransportType.UDP)
        {
            StartUdp();
        }
        else if (networkTransportType == NetworkTransportType.TCP)
        {
            StartTcp();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void StartUdp()
    {
        udpTx = new UdpClient("127.0.0.1", 23456);
        StartReceiving();
    }

    private void StartTcp()
    {
        StartListening();
    }

    public void SendFrame(string frame)
    {
        if (networkTransportType == NetworkTransportType.UDP)
        {
            SendFrameUdp(frame);
        }
        else if (networkTransportType == NetworkTransportType.TCP)
        {
            SendFrameTcp(frame);
        }
    }

    private void SendFrameUdp(string frame)
    {
        byte[] datagram = Encoding.ASCII.GetBytes(frame);
        try
        {
            udpTx.Send(datagram, datagram.Length);
        }
        catch (Exception e)
        {
            Debug.Log("Error sending UDP datagram");
            Debug.Log(e);
        }
    }

    private void SendFrameTcp(string frame)
    {
        try
        {
            byte[] data = Encoding.ASCII.GetBytes(frame);
            writer?.Write((byte)0x0C);
            writer?.Write(data);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private async void StartListening()
    {
        tcpListener = new TcpListener(IPAddress.Any, 23456);
        tcpListener.Start();
        while (true)
        {
            tcpClient = await tcpListener.AcceptTcpClientAsync();
            var stream = tcpClient.GetStream();
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            byte[] buffer = new byte[1];

            try
            {
                while (true)
                {
                    buffer[0] = await Task.Run<byte>(() => reader.ReadByte());
                    string cmd = Encoding.ASCII.GetString(buffer);
                    inputController.SetInputs(cmd);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            reader = null;
            writer = null;
        }
    }

    private async void StartReceiving()
    {
        using (udpRx = new UdpClient(65432))
        {
            try
            {
                var ipEndpoint = new IPEndPoint(IPAddress.Any, 65432);
                while (true)
                {
                    var datagram = await Task.Run<byte[]>(() => udpRx.Receive(ref ipEndpoint));
                    string cmd = Encoding.ASCII.GetString(datagram);

                    inputController.SetInputs(cmd);
                }
            }
            catch (SocketException)
            {
            }
        }
    }
}
