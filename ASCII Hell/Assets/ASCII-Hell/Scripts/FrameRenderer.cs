using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FrameRenderer : MonoBehaviour
{
    public int resolutionWidth = 480;
    public int resolutionHeight = 640;

    private Texture2D texture2D;
    private Rect rect;

    [SerializeField]private Camera m_Camera;

    public bool writefile = true;

    // Start is called before the first frame update
    void Start()
    {
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


            // Test line to write to file
            if (writefile)
            {
                string temp = Application.dataPath + @"/../" + @"cam_output.jpg";
                Debug.Log("Writing camera frame to: " + temp);
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
