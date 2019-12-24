using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public bool writefile = true;

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


            // If we should write the file
            if (writefile)
            {

                char[] ascii_colors = new char[] { ' ', '.', ':', '}', 'v', '0', 'q', 'M' };
                string ascii_file = Application.dataPath + @"/../" + @"ascii_output.txt";
                StringBuilder sb = new StringBuilder();
                for (int y = t2d.height; y >= 0; y--)
                {
                    for (int x = 0; x < t2d.width; x++)
                    {
                        var pixel = t2d.GetPixel(x, y);
                        int color = (int)(pixel.grayscale * (ascii_colors.Length - 1));
                        sb.Append(ascii_colors[color]);
                    }
                    sb.Append('\n');
                }
                m_text.text = sb.ToString();
                //Debug.Log("Text is: " + sb.ToString());
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
