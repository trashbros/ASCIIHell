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

    public InputHandler inputController;

    // Start is called before the first frame update
    void Start()
    {
        if (inputController == null)
        {
            inputController = GetComponent<InputHandler>();
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

                    inputController.SetInputs(cmd);
                    Debug.Log("Got input from udp: " + cmd);

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
