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

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.EventUtil.AddListener(CustomEventList.GAME_RUNNING, OnGameRunning);

        if(m_text == null)
        {
            m_text = this.GetComponent<UnityEngine.UI.Text>();
        }

        if(m_Camera == null)
        {
            m_Camera = this.GetComponent<Camera>();
        }
        
        if (m_networkController == null)
        {
            m_networkController = this.GetComponent<NetworkController>();
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

            StringBuilder textWithColors = new StringBuilder();
            StringBuilder textNoColors = new StringBuilder();

            // Current score and such value
            if (m_gameRunning)
            {
                textWithColors.Append("     Lives: " + GameplayParameters.instance.Lives.ToString());
                textWithColors.Append("     Boosts: " + GameplayParameters.instance.SlowDowns.ToString());
                textWithColors.Append("     Score: " + GameplayParameters.instance.Score.ToString());

                textNoColors.Append("     Lives: " + GameplayParameters.instance.Lives.ToString());
                textNoColors.Append("     Boosts: " + GameplayParameters.instance.SlowDowns.ToString());
                textNoColors.Append("     Score: " + GameplayParameters.instance.Score.ToString());
            }

            textWithColors.Append("\r\n");
            textNoColors.Append("\r\n");

            var lastColor = Color.gray;

            for (int y = t2d.height; y >= 0; y--)
            {
                for (int x = 0; x < t2d.width; x++)
                {
                    var pixel = t2d.GetPixel(x, y);

                    int color = (int)(pixel.grayscale * (ascii_colors_r.Length - 1));

                    if (pixel.r > pixel.g && pixel.r > pixel.b)
                    {
                        if (lastColor != Color.red)
                        {
                            textWithColors.Append("\u001B[31;1m");
                            lastColor = Color.red;
                        }

                        textWithColors.Append(ascii_colors_r[color]);
                        textNoColors.Append(ascii_colors_r[color]);
                    }
                    else if (pixel.g > pixel.r && pixel.g > pixel.b)
                    {
                        if (lastColor != Color.green)
                        {
                            textWithColors.Append("\u001B[32;1m");
                            lastColor = Color.green;
                        }

                        textWithColors.Append(ascii_colors_g[color]);
                        textNoColors.Append(ascii_colors_g[color]);
                    }
                    else if (pixel.b > pixel.r && pixel.b > pixel.g)
                    {
                        if (lastColor != Color.blue)
                        {
                            textWithColors.Append("\u001B[34;1m");
                            lastColor = Color.blue;
                        }

                        textWithColors.Append(ascii_colors_b[color]);
                        textNoColors.Append(ascii_colors_b[color]);
                    }
                    else
                    {
                        if (lastColor != Color.gray)
                        {
                            textWithColors.Append("\u001B[37;1m");
                            lastColor = Color.gray;
                        }

                        textWithColors.Append(ascii_colors_y[color]);
                        textNoColors.Append(ascii_colors_y[color]);
                    }
                }
                textWithColors.Append("\r\n");
                textNoColors.Append("\r\n");
            }

            m_text.text = textNoColors.ToString();

            if (writeNetwork)
            {
                m_networkController.SendFrame(textWithColors.ToString());
            }

            // If we should write the file
            if (writeFile)
            {
                string ascii_color_file = Application.dataPath + @"/../" + @"ascii_color_output.txt";
                File.WriteAllText(ascii_color_file, textWithColors.ToString());

                string ascii_file = Application.dataPath + @"/../" + @"ascii_output.txt";
                File.WriteAllText(ascii_file, textNoColors.ToString());
            }

            // If we should write the file
            if (writeFile)
            {
                string temp = Application.dataPath + @"/../" + @"cam_output.jpg";
                File.WriteAllBytes(temp, texture2D.EncodeToJPG());
            }
        }
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
