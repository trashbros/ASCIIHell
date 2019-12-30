using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayParameters : MonoBehaviour
{
    public static GameplayParameters instance;

    [Header("Game Info")]
    [SerializeField] private static int StartingLives = 3;
    [SerializeField] private static int StartingBoosts = 3;

    [Header("Gameplay Parameters")]
    [SerializeField] private float m_slowDownTime = 5.0f;
    [SerializeField] private float m_playerSpeed = 3.0f;
    [SerializeField] private float m_enemySpeed = 3.0f;
    [SerializeField] private float m_fireRate = 1.0f;
    [SerializeField] private float m_slowDownPercent = 0.5f;

    [Header("Game Info")]
    [SerializeField] private int m_lives = 3;
    [SerializeField] private int m_slowDowns = 3;
    [SerializeField] private int m_score = 0;


    [ExposeInEditor(RuntimeOnly = true)]
    public void UpdateParameters()
    {
        CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
    }
    
    public void ResetParameters()
    {
        m_lives = StartingLives;
        m_slowDowns = StartingBoosts;
        m_score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.EventUtil.AddListener(CustomEventList.ADD_POINTS, OnPointsAdded);
        CustomEvents.EventUtil.AddListener(CustomEventList.SLOW_TIME, OnSlowTime);

        ResetParameters();

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPointsAdded(CustomEvents.EventArgs evt)
    {
        Score += (int)evt.args.GetValue(0);
    }

    private void OnSlowTime(CustomEvents.EventArgs evt)
    {
        if((bool)evt.args.GetValue(0))
        {
            SlowDowns--;
        }
    }

    #region ParameterProperties
    public float SlowDownPercent
    {
        get { return m_slowDownPercent; }
        set
        {
            m_slowDownPercent = value;
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
        }
    }
    public float PlayerSpeed
    {
        get { return m_playerSpeed; }
        set
        {
            m_playerSpeed = value;
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
        }
    }

    public float EnemySpeed
    {
        get { return m_enemySpeed; }
        set
        {
            m_enemySpeed = value;
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
        }
    }
    public float FireRate
    {
        get { return m_fireRate; }
        set
        {
            m_fireRate = value;
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
        }
    }
    public float SlowDownTime
    {
        get { return m_slowDownTime; }
        set
        {
            m_slowDownTime = value;
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
        }
    }
    #endregion

    #region InfoProperties
    public int Lives
    {
        get { return m_lives; }
        set { m_lives = value; }
    }
    public int SlowDowns
    {
        get { return m_slowDowns; }
        set { m_slowDowns = value; }
    }
    public int Score
    {
        get { return m_score; }
        set { m_score = value; }
    }
    #endregion
}
