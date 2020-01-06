using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ASCIIDrawer : MonoBehaviour
{
    public int resolutionWidth = 480;
    public int resolutionHeight = 640;

    private string m_horizontalBorder = "";

    //private Texture2D texture2D;
    //private Rect rect;

    //[SerializeField] private Camera m_Camera;
    //[SerializeField] private UnityEngine.UI.Text m_text;
    [SerializeField] private NetworkController m_networkController;
    [SerializeField] private LevelManager m_levelManager;

    bool m_gameRunning = false;

    //bool m_newParticleData = false;
    bool m_allParticleData = false;

    object[] m_particleData = null;

    List<Vector2> m_objectData = null;

    //public bool writeFile = true;
    public bool writeNetwork = true;

    private string titleText;

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.EventUtil.AddListener(CustomEventList.GAME_RUNNING, OnGameRunning);
        CustomEvents.EventUtil.AddListener(CustomEventList.PARTICLE_INFO, OnParticleInfo);

        titleText = Resources.Load<TextAsset>("TitleScreen").text;

        MakeTopBottomBorder();

        if (m_networkController == null)
        {
            this.GetComponent<NetworkController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RenderTextFrame();
    }

    private void RenderTextFrame()
    {
        if(!writeNetwork)
        {
            return;
        }

        // Current score and such value
        if (m_gameRunning)
        {
            if(!m_allParticleData || m_particleData == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("     Lives: " + GameplayParameters.instance.Lives.ToString());
            sb.Append("     Boosts: " + GameplayParameters.instance.SlowDowns.ToString());
            sb.Append("     Score: " + GameplayParameters.instance.Score.ToString());
            sb.Append("\r\n");

            sb.Append(m_horizontalBorder);

            m_objectData = GetObjectPositions();

            for (int i = resolutionHeight; i >= 0; i--)
            {
                sb.Append('|');
                for (int j = 0; j <= resolutionWidth; j++)
                {
                    sb.Append(GetLocationCharacter(j, i));
                }
                sb.Append("|\r\n");
            }

            sb.Append(m_horizontalBorder);

            if (writeNetwork)
            {
                m_networkController.SendFrame(sb.ToString());
                m_particleData = null;
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

    private char GetLocationCharacter(int x, int y)
    {
        if(m_objectData.Exists(o => o.x == (float)x && o.y == (float)y))
        {
            return 'X';
        }
        
        return ' ';
    }

    [ExposeInEditor(RuntimeOnly = true)]
    public void MakeTopBottomBorder()
    {
        m_horizontalBorder = "";
        m_horizontalBorder += ".";

        for (int j = 0; j <= resolutionWidth; j++)
        {
            m_horizontalBorder += "-";
        }

        m_horizontalBorder+= ".\r\n";
    }

    private List<Vector2> GetObjectPositions()
    {
        return m_levelManager.GetEntityPositions();
    }

    private void OnGameRunning(CustomEvents.EventArgs evt)
    {
        m_gameRunning = (bool)evt.args.GetValue(0);
    }

    private void OnParticleInfo(CustomEvents.EventArgs evt)
    {
        if(m_particleData == null)
        {
            m_particleData = new object[(int)evt.args.GetValue(0)];
        }

        m_particleData[(int)evt.args.GetValue(1)] = new object[3] { evt.args.GetValue(2),
            evt.args.GetValue(3),
            evt.args.GetValue(4)
        };

        bool haveData = true;
        foreach(var pData in m_particleData)
        {
            if(pData == null)
            {
                haveData = false;
            }
        }

        m_allParticleData = haveData;
    }

    private void OnApplicationQuit()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.GAME_RUNNING, OnGameRunning);
    }
}
