//using UnityEngine;
//using System.Collections;
//using System.Net;
//using System.Net.Sockets;
//using System.Linq;
//using System;
//using System.IO;
//using System.Text;
//using System.Runtime.Serialization.Formatters.Binary;

//// From: https://www.mathworks.com/matlabcentral/answers/196774-connection-between-matlab-and-unity3d
//namespace MatLabComm
//{

//    public class PosePipe : MonoBehaviour
//    {
//        // Use this for initialization
//        public String Host = "localhost";
//        public Int32 Port = 50000;

//        Mule mule;
//        Dock[] docks;
//        Trailer trailer;

//        private TcpListener listener = null;//new TcpListener(IPAddress.Parse("127.0.0.1"), Port);
//        private TcpClient client = null;
//        private NetworkStream ns = null;
//        string msg;

//        void Start()
//        {
//            mule = GetComponentInChildren<Mule>();
//            docks = GetComponentsInChildren<Dock>();
//            trailer = GetComponentInChildren<Trailer>();

//            listener = new TcpListener(Dns.GetHostEntry(Host).AddressList[1], Port);
//            listener.Start();
//            print("is listening");

//            //TODO add escape function for cancelling or otherwise exiting this loop.
//            //while (!listener.Pending())
//            //    System.Threading.Thread.Sleep(10);

//            //Note: This if statement may seem redundant, but it will be important for
//            //      when the previous while loop exits without connecting.
//            if (listener.Pending())
//            {
//                client = listener.AcceptTcpClient();
//                Console.WriteLine("Connected");
//            }
//        }

//        // Update is called once per frame
//        void Update()
//        {
            
//            if (client == null)
//            {
//                if (listener.Pending())
//                {
//                    client = listener.AcceptTcpClient();
//                    Console.WriteLine("Connected");
//                }
//                else
//                {
//                    return;
//                }
//            }

//            ns = client.GetStream();

//            if ((ns != null) && (ns.DataAvailable))
//            {
//                //Reads the data from MATLAB
//                byte[] data = new byte[112];
//                int bytes = ns.Read(data, 0, data.Length);

//                //Seperate out the mule data from the trailer data
//                byte[] mba = new byte[9*8];
//                byte[] tba = new byte[5*8];
//                Array.Copy(data, 0, mba, 0, 9 * 8);
//                Array.Copy(data, 9 * 8, tba, 0, 5 * 8);

//                //Decode the data from MATLAB
//                Mule_Data md = new Mule_Data(mba);
//                Trailer_Data td = new Trailer_Data(tba);

//                //BinaryFormatter formatter = new BinaryFormatter();
//                //packetData data = (packetData)formatter.Deserialize(ns);
//                mule.UpdateMulePose(md);
//                trailer.UpdateTrailerPose(td);
//            }
//            //StreamReader reader = new StreamReader(ns);
//            //msg = reader.ReadToEnd();
//            //print(msg);
//        }

//        private void OnApplicationQuit()
//        {
//            if (listener != null)
//                listener.Stop();
//        }
//    }
//}
///* MatLab Client code:
// * 
//clc
//clear all
//tcpipClient = tcpip('127.0.0.1',55001,'NetworkRole','Client');
//set(tcpipClient,'Timeout',30);
//fopen(tcpipClient);
//a='yah!! we could make it';
//fwrite(tcpipClient,a);
//fclose(tcpipClient);
// *
// */
