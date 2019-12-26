using System;
using System.Collections;
using System.Collections.Generic;
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

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        udpTx = new UdpClient("127.0.0.1", 23456);

        StartReceiving();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SendFrame(string frame)
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
                    switch (cmd)
                    {
                        case "U":
                            playerController.SetVerticalAxis(1.0f);
                            break;

                        case "u":
                            playerController.SetVerticalAxis(0.0f);
                            break;

                        case "D":
                            playerController.SetVerticalAxis(-1.0f);
                            break;

                        case "d":
                            playerController.SetVerticalAxis(0.0f);
                            break;

                        case "L":
                            playerController.SetHorizontalAxis(-1.0f);
                            break;

                        case "l":
                            playerController.SetHorizontalAxis(0.0f);
                            break;

                        case "R":
                            playerController.SetHorizontalAxis(1.0f);
                            break;

                        case "r":
                            playerController.SetHorizontalAxis(0.0f);
                            break;

                        default:
                            Debug.Log("Unknown command: " + cmd);
                            break;
                    }

                }
            }
            catch (SocketException)
            {
            }
        }
    }
}
