using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UdpController : MonoBehaviour
{
    private UdpClient udpTx;
    private UdpClient udpRx;
    private IPEndPoint ipEndPoint;

    private TcpListener tcpListener;
    private TcpClient tcpClient;
    private BinaryWriter writer;
    private BinaryReader reader;

    public InputHandler inputController;

    // Start is called before the first frame update
    void Start()
    {
        if (inputController == null)
        {
            inputController = GetComponent<InputHandler>();
        }

        /*
        udpTx = new UdpClient("127.0.0.1", 23456);

        StartReceiving();
        */

        StartListening();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SendFrame(string frame)
    {
        /*
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
        */

        byte[] data = Encoding.ASCII.GetBytes(frame);
        writer?.Write((byte)0x0C);
        writer?.Write(data);
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
                    //Debug.Log("Got input from udp: " + cmd);

                    //switch (cmd)
                    //{
                    //    case "U":
                    //    case "u":
                    //        //playerController.SetVerticalAxis(1.0f);
                    //        InputContainer.instance.moveDir = new Vector2(0f, 1f);
                    //        break;

                    //    case "D":
                    //    case "d":
                    //        //playerController.SetVerticalAxis(-1.0f);
                    //        InputContainer.instance.moveDir = new Vector2(0f, -1f);
                    //        break;

                    //    case "L":
                    //    case "l":
                    //        //playerController.SetHorizontalAxis(-1.0f);
                    //        InputContainer.instance.moveDir = new Vector2(-1f, 0f);
                    //        break;

                    //    case "R":
                    //    case "r":
                    //        //playerController.SetHorizontalAxis(1.0f);
                    //        InputContainer.instance.moveDir = new Vector2(-1f, 0f);
                    //        break;

                    //    case "Q":
                    //    case "q":
                    //        InputContainer.instance.cancel.down = true;
                    //        break;

                    //    default:
                    //        Debug.Log("Unknown command: " + cmd);
                    //        break;
                    //}

                }
            }
            catch (SocketException)
            {
            }
        }
    }
}
