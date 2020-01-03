using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Entity
{
    [Header("Boss Stats")]
    [SerializeField] protected int m_pointValue = 1000;
    [SerializeField] protected BossPhase[] m_bossPhases;

    private int m_currentPhase = 0;

    protected new void Start()
        {
            base.Start();
        }

    protected new void Update()
    {
        base.Update();
    }

    public void Initialize(BossData spawnerData)
    {
        m_currentPhase = 0;
        m_pointValue = spawnerData.PointValue;
        // Set bullet pattern
        m_bossPhases = spawnerData.BossPhases;
        base.Initialize(spawnerData.StartingLocation, m_bossPhases[m_currentPhase].StartingHealth, 0f);
    }

    protected override Vector2 GetMovement()
    {
        return Vector2.zero;
    }

    protected override void OnDeath()
    {
        if(m_currentPhase < m_bossPhases.Length - 1)
        {
            m_currentPhase ++;
            m_health = m_bossPhases[m_currentPhase].StartingHealth;
            // Update bullet pattern
            return;
        }
        CustomEvents.EventUtil.DispatchEvent(CustomEventList.ADD_POINTS, new object[1] { m_pointValue });
        //Object.Destroy(this.gameObject);
        m_alive = false;
        SetActive(false);
        //this.gameObject.SetActive(false);
    }

    protected override void OnParameterChange(CustomEvents.EventArgs evt)
    {
        m_slowDownPercent = GameplayParameters.instance.SlowDownPercent;
    }
}