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
    [SerializeField]private NetworkController m_networkController;

    bool m_gameRunning = false;

    public bool writeFile = true;
    public bool writeNetwork = true;

    private string titleText;

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.EventUtil.AddListener(CustomEventList.GAME_RUNNING, OnGameRunning);

        titleText = Resources.Load<TextAsset>("TitleScreen").text;

        if(m_text == null)
        {
            return;
        }

        if(m_Camera == null)
        {
            this.GetComponent<Camera>();
        }
        
        if (m_networkController == null)
        {
            this.GetComponent<NetworkController>();
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

            StringBuilder sb = new StringBuilder();

            // Current score and such value
            if (m_gameRunning)
            {
                sb.Append("     Lives: " + GameplayParameters.instance.Lives.ToString());
                sb.Append("     Boosts: " + GameplayParameters.instance.SlowDowns.ToString());
                sb.Append("     Score: " + GameplayParameters.instance.Score.ToString());
            }
            sb.Append("\r\n");

            for (int y = t2d.height; y >= 0; y--)
            {
                for (int x = 0; x < t2d.width; x++)
                {
                    var pixel = t2d.GetPixel(x, y);

                    int color = (int)(pixel.grayscale * (ascii_colors_r.Length - 1));

                    if (pixel.r > pixel.g && pixel.r > pixel.b)
                    {
                        sb.Append(ascii_colors_r[color]);
                    }
                    else if (pixel.g > pixel.r && pixel.g > pixel.b)
                    {
                        sb.Append(ascii_colors_g[color]);
                    }
                    else if (pixel.b > pixel.r && pixel.b > pixel.g)
                    {
                        sb.Append(ascii_colors_b[color]);
                    }
                    else
                    {
                        sb.Append(ascii_colors_y[color]);
                    }
                }
                sb.Append("\r\n");
            }
            m_text.text = sb.ToString();

            if (writeNetwork)
            {
                m_networkController.SendFrame(sb.ToString());
            }

            // If we should write the file
            if (writeFile)
            {
                string ascii_file = Application.dataPath + @"/../" + @"ascii_output.txt";
                File.WriteAllText(ascii_file, sb.ToString());
            }

            // Test line to write to file
            if (writeFile)
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

    private void RenderTextFrame()
    {
        // Current score and such value
        if (m_gameRunning)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("     Lives: " + GameplayParameters.instance.Lives.ToString());
            sb.Append("     Boosts: " + GameplayParameters.instance.SlowDowns.ToString());
            sb.Append("     Score: " + GameplayParameters.instance.Score.ToString());

            sb.Append("\r\n");

            List<Vector2> objectPositions = GetObjectPositions();

            for(int i = resolutionHeight; i >=0 ; i --)
            {
                for(int j = 0; j <= resolutionWidth; j++)
                {
                    if(objectPositions.Contains(new Vector2(j,i)))
                    {
                        sb.Append('.');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }
                sb.Append("\r\n");
            }

            m_text.text = sb.ToString();

            if (writeNetwork)
            {
                m_networkController.SendFrame(sb.ToString());
            }
        }
        else
        {
            if (writeNetwork)
            {
                m_networkController.SendFrame(titleText);
            }
        }
        
    }

    private List<Vector2> GetObjectPositions()
    {
        return new List<Vector2>();
    }

    private void OnGameRunning(CustomEvents.EventArgs evt)
    {
        m_gameRunning = (bool)evt.args.GetValue(0);
    }

    private void OnApplicationQuit()
    {
        Camera.onPostRender -= SendRenderedCamera;
        CustomEvents.EventUtil.RemoveListener(CustomEventList.GAME_RUNNING, OnGameRunning);
    }
}
