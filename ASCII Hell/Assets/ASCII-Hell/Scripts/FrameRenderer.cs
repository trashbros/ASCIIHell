using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class FrameRenderer : MonoBehaviour
{
    public int resolutionWidth = 480;
    public int resolutionHeight = 640;

    private Texture2D texture2D;
    private Rect rect;

    [SerializeField]private Camera m_Camera;
    [SerializeField]private UnityEngine.UI.Text m_text;
    [SerializeField]private UdpController m_udpController;

    public bool writefile = true;
    public bool writeUDP = true;

    // Start is called before the first frame update
    void Start()
    {
        if(m_text == null)
        {
            return;
        }

        if(m_Camera == null)
        {
            this.GetComponent<Camera>();
        }
        
        if (m_udpController == null)
        {
            this.GetComponent<UdpController>();
        }

        texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
        m_Camera.targetTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);

        Camera.onPostRender += SendRenderedCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendRenderedCamera(Camera _camera)
    {
        //yield return endOfFrame;
        if (texture2D != null && _camera == m_Camera)
        {

            texture2D.ReadPixels(rect, 0, 0);
            Texture2D t2d = texture2D;

            byte[] imgBytes = t2d.GetRawTextureData();//.EncodeToJPG();

            char[] ascii_colors_r = " `.'~;)]j50%GX#W".ToCharArray();
            char[] ascii_colors_g = " `.-_|}iv&V8DmHW".ToCharArray();
            char[] ascii_colors_b = " `.,!{([t3y$qQNW".ToCharArray();
            char[] ascii_colors_y = " `.^:/=?ceuOgK@M".ToCharArray();

            string ascii_file = Application.dataPath + @"/../" + @"ascii_output.txt";
            StringBuilder sb = new StringBuilder();
            for (int y = t2d.height; y >= 0; y--)
            {
                for (int x = 0; x < t2d.width; x++)
                {
                    var pixel = t2d.GetPixel(x, y);

                    int color = 0;

                    if (pixel.maxColorComponent == pixel.r)
                    {
                        color = (int)(pixel.r * (ascii_colors_r.Length - 1));
                        sb.Append(ascii_colors_r[color]);
                    }
                    else if (pixel.maxColorComponent == pixel.g)
                    {
                        color = (int)(pixel.g * (ascii_colors_g.Length - 1));
                        sb.Append(ascii_colors_g[color]);
                    }
                    else if (pixel.maxColorComponent == pixel.b)
                    {
                        color = (int)(pixel.b * (ascii_colors_b.Length - 1));
                        sb.Append(ascii_colors_b[color]);
                    }
                }
                sb.Append('\n');
            }
            m_text.text = sb.ToString();

            if (writeUDP)
            {
                m_udpController.SendFrame(sb.ToString());
            }

            // If we should write the file
            if (writefile)
            {
                File.WriteAllText(ascii_file, sb.ToString());
            }

            // Test line to write to file
            if (writefile)
            {
                string temp = Application.dataPath + @"/../" + @"cam_output.jpg";
                //Debug.Log("Writing camera frame to: " + temp);
                File.WriteAllBytes(temp, texture2D.EncodeToJPG());
            }


            /*
            Color rgb = texture2D[camindex].GetPixel(0, 0);
            byte r = (byte)(rgb.r * 255);
            byte g = (byte)(rgb.g * 255);
            byte b = (byte)(rgb.b * 255);

            byte ar = pngBytes[0];
            byte ag = pngBytes[1];
            byte ab = pngBytes[2];

            int a = 0;
            */
        }
    }

    private void OnApplicationQuit()
    {
        Camera.onPostRender -= SendRenderedCamera;
    }
}
