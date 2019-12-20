//using System.Collections;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System;
//using System.IO;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Runtime.InteropServices;
//using System.Collections.Generic;

//// Struct for the camera header information packet
//public struct CameraHeader
//{
//    public Camera_T CameraType;
//    public int CameraName;
//    public int FrameByteSize;
//    //public double TimeStamp;
//    //public int frameid;
//    public int width;
//    public int height;
//}

//public enum Camera_T
//{
//    DockCam,
//    AreaCam
//}

//// From: https://www.mathworks.com/matlabcentral/answers/196774-connection-between-matlab-and-unity3d

//public class CameraPipe : MonoBehaviour {


//    // Use this for initialization
    
//    public string FrameId = "Camera";
//    public int resolutionWidth = 640;
//    public int resolutionHeight = 480;
//    public String Host = "localhost";
//    public Int32 Port = 55000;

//    const int SEND_RECEIVE_COUNT = 20;//42;//54;//4;
//    private Texture2D[] texture2D;
//    private Rect[] rect;

//    internal Boolean socketReady = false;
//    internal int count = 0;
//    TcpClient mySocket = null;
//    NetworkStream theStream = null;
//    StreamWriter theWriter = null;

//    Camera[] ImageCameras;
//    RawImage canvasView;


//    public int Timeout = 10;

//    public bool writefile = false;

//    private int write_img = 0;

//    public void Awake()
//    {
        

//    }

//    void Start()
//    {
//        Application.runInBackground = true;

//        ImageCameras = this.GetComponentsInChildren<Camera>();
//        texture2D = new Texture2D[ImageCameras.Length];
//        rect = new Rect[ImageCameras.Length];

//        mySocket = new TcpClient();

//        canvasView = this.GetComponentInChildren<Canvas>().GetComponent<RawImage>();

//        InitializeGameObject();

//        //Camera.onPostRender += SendRenderedCamera;
//        if (SetupSocket())
//        {
//            Debug.Log("socket is set up");
            
//        }

//        Camera.onPostRender += SendRenderedCamera;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(!socketReady && count > 10)
//        {
//            count = 0;
//            SetupSocket();
//        }
//        count += 1;

//    }

//    public bool SetupSocket()
//    {
//        try
//        {
//            mySocket.Connect(Host, Port);
//            theStream = mySocket.GetStream();
//            theWriter = new StreamWriter(theStream);
//            socketReady = true;
//            //Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("yah!! it works");
//            //mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
//            //Debug.Log("socket is sent");
//            return true;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("Socket error: " + e);
//            return false;
//        }
//    }

//    private void InitializeGameObject()
//    {
//        for (int i = 0; i < ImageCameras.Length; i++)
//        {
//            texture2D[i] = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
//            rect[i] = new Rect(0, 0, resolutionWidth, resolutionHeight);
//            ImageCameras[i].targetTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
//        }
//    }

//    //Converts the data size to byte array and put result to the fullBytes array
//    void byteLengthToFrameByteArray(int byteLength, byte[] fullBytes)
//    {
//        //Clear old data
//        Array.Clear(fullBytes, 0, fullBytes.Length);
//        //Convert int to bytes
//        byte[] bytesToSendCount = BitConverter.GetBytes(byteLength);
//        //Copy result to fullBytes
//        bytesToSendCount.CopyTo(fullBytes, 0);
//    }

//    //Converts the data size to byte array and put result to the fullBytes array
//    void HeaderToByte(CameraHeader camHead, byte[] fullBytes)
//    {
//        //Clear old data
//        Array.Clear(fullBytes, 0, fullBytes.Length);
//        byte[] arr = new byte[SEND_RECEIVE_COUNT];
//        int index = 0;

//        BitConverter.GetBytes((int)camHead.CameraType).CopyTo(arr, index);
//        index += BitConverter.GetBytes((int)camHead.CameraType).Length;
//        /*foreach (char nameC in camHead.CameraName)
//        {
//            BitConverter.GetBytes(nameC).CopyTo(arr, index);
//            index += BitConverter.GetBytes(nameC).Length;
//        }*/
//        BitConverter.GetBytes(camHead.CameraName).CopyTo(arr, index);
//        index += BitConverter.GetBytes(camHead.CameraName).Length;
//        BitConverter.GetBytes(camHead.FrameByteSize).CopyTo(arr, index);
//        index += BitConverter.GetBytes(camHead.FrameByteSize).Length;
//        //BitConverter.GetBytes(camHead.TimeStamp).CopyTo(arr, index);
//        //index += BitConverter.GetBytes(camHead.TimeStamp).Length;
//        //BitConverter.GetBytes(camHead.frameid).CopyTo(arr, index);
//        //index += BitConverter.GetBytes(camHead.frameid).Length;
//        BitConverter.GetBytes(camHead.width).CopyTo(arr, index); ;
//        index += BitConverter.GetBytes(camHead.width).Length;
//        BitConverter.GetBytes(camHead.height).CopyTo(arr, index);

//        //IntPtr ptr = Marshal.AllocHGlobal(len);
//        //Marshal.StructureToPtr(camHead, ptr, true);
//        //Marshal.Copy(ptr, arr, 0, len);
//        //Marshal.FreeHGlobal(ptr);
//        //Copy result to fullBytes
//        arr.CopyTo(fullBytes, 0);
//    }

//    //Converts the byte array to the data size and returns the result
//    int frameByteArrayToByteLength(byte[] frameBytesLength)
//    {
//        int byteLength = BitConverter.ToInt32(frameBytesLength, 0);
//        return byteLength;
//    }

//    public void SendRenderedCamera(Camera _camera)
//    {
//        if (!socketReady) return;
//        if (mySocket == null || !Array.Exists(ImageCameras, cam => cam == _camera))
//        {
//            return;
//        }

//        int camindex = Array.IndexOf(ImageCameras, _camera);
//        //yield return endOfFrame;
//        if (texture2D != null && camindex > -1)
//        {

//            texture2D[camindex].ReadPixels(rect[camindex], 0, 0);
//            Texture2D t2d = texture2D[camindex];

//            CameraHeader camHead;
//            if (_camera.tag == "DockCam")
//            {
//                camHead.CameraType = Camera_T.DockCam;
//            }
//            else if(_camera.tag == "AreaCam")
//            {
//                camHead.CameraType = Camera_T.AreaCam;
//            }
//            else
//            {
//                return;
//            }

//            byte[] imgBytes = t2d.GetRawTextureData();//.EncodeToJPG();
//            Debug.Log("Camera num: " + _camera.name.ToCharArray()[12]);
//            camHead.CameraName = (int)char.GetNumericValue(_camera.name.ToCharArray()[12]);
//            camHead.FrameByteSize = imgBytes.Length;
//            camHead.height = t2d.height;
//            camHead.width = t2d.width;
//            //camHead.frameid = write_img;
//            //camHead.TimeStamp = DateTime.Now.Ticks;


//            // Test line to write to file
//            if (writefile)
//            {
//                string temp = Application.dataPath + @"/../" + camHead.CameraName + /*camHead.frameid.ToString() +*/ @".jpg";
//                Debug.Log("Writing camera frame to: " + temp);
//                File.WriteAllBytes(temp, texture2D[camindex].EncodeToJPG());
//            }

//            //Fill header info
//            byte[] camHeaderbyte = new byte[SEND_RECEIVE_COUNT];
//            HeaderToByte(camHead, camHeaderbyte);

//            try
//            {
//                //Send Header info first
//                if (mySocket.Connected)
//                {
//                    theStream.Write(camHeaderbyte, 0, camHeaderbyte.Length);
//                    Debug.Log("Sent Image byte Length: " + camHeaderbyte.Length);
//                }


//                //Send the image bytes
//                if (mySocket.Connected)
//                {
//                    theStream.Write(imgBytes, 0, imgBytes.Length);
//                }
//            }
//            catch (Exception e)
//            {
//                Debug.Log("Socket error: " + e);
//            }
//            /*
//            Color rgb = texture2D[camindex].GetPixel(0, 0);
//            byte r = (byte)(rgb.r * 255);
//            byte g = (byte)(rgb.g * 255);
//            byte b = (byte)(rgb.b * 255);

//            byte ar = pngBytes[0];
//            byte ag = pngBytes[1];
//            byte ab = pngBytes[2];

//            int a = 0;
//            */
//        }
//    }

//    private void OnApplicationQuit()
//    {
//        Camera.onPostRender -= SendRenderedCamera;
//        if(mySocket.Connected)
//            mySocket.Close();
//    }

//}

///* MatLab Server code:
// * 
//clc
//clear all
//tcpipServer = tcpip('0.0.0.0',55000,'NetworkRole','Server');
//while(1)
//data = membrane(1);
//fopen(tcpipServer);
//rawData = fread(tcpipServer,14,'char');
//for i=1:14 rawwData(i)= char(rawData(i));
//end
//fclose(tcpipServer);
//end
// * 
// */
