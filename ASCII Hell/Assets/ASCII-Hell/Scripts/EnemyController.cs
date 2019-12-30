using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, ICollidable
{
    [SerializeField] private int m_pointValue = 5;
    [SerializeField] private float m_speed = 3.0f;
    [SerializeField] private float m_slowDownPercent = 0.5f;
    [SerializeField] private bool m_gamePaused = false;
    [SerializeField] private bool m_gameSlowed = false;

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.EventUtil.AddListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.AddListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.AddListener(CustomEventList.SLOW_TIME, OnSlowTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_gamePaused)
        {
            return;
        }
    }

    public void OnHit(GameObject collision)
    {
        if (!m_gamePaused)
        {
            Debug.Log("Hit by particle");
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.ADD_POINTS, new object[1] { m_pointValue });
            Object.Destroy(this.gameObject);
        }
    }

    #region EventListeners
    private void OnGamePause(CustomEvents.EventArgs evt)
    {
        m_gamePaused = (bool)evt.args.GetValue(0);
    }

    private void OnSlowTime(CustomEvents.EventArgs evt)
    {
        m_gameSlowed = (bool)evt.args.GetValue(0);
    }

    private void OnParameterChange(CustomEvents.EventArgs evt)
    {
        m_speed = GameplayParameters.instance.EnemySpeed;
        m_slowDownPercent = GameplayParameters.instance.SlowDownPercent;
    }
    #endregion

    private void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.SLOW_TIME, OnSlowTime);
    }
}
