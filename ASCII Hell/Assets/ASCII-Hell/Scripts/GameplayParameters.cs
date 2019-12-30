using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayParameters : MonoBehaviour
{
    public static GameplayParameters instance;

    [Header("Gameplay Parameters")]
    [SerializeField] private float m_slowDownTime = 0.5f;
    [SerializeField] private float m_playerSpeed = 3.0f;
    [SerializeField] private float m_fireRate = 1.0f;


    [ExposeInEditor(RuntimeOnly = true)]
    public void UpdateParameters()
    {
        CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region ParameterProperties
    public float SlowDownTime
    {
        get { return m_slowDownTime; }
        set
        {
            m_slowDownTime = value;
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
    public float FireRate
    {
        get { return m_fireRate; }
        set
        {
            m_fireRate = value;
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PARAMETER_CHANGE);
        }
    }
    #endregion
}
