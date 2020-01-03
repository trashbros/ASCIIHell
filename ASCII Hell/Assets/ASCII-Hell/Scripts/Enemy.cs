using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy Stats")]
    [SerializeField] protected int m_pointValue = 5;
    [SerializeField] private EnemyPathStep[] enemyPathSteps;

    float m_timeSinceDirectionChange = 0f;
    int m_CurrentPathStep = 0;

    protected new void Start()
        {
            base.Start();
        }

    protected new void Update()
    {
        base.Update();
    }

    public void Initialize(SpawnerData spawnerData)
    {
        m_pointValue = spawnerData.PointValue;
        // Set bullet pattern
        enemyPathSteps = spawnerData.MovementPattern;
        base.Initialize(spawnerData.StartingLocation, spawnerData.Health, spawnerData.Speed);
    }

    protected override Vector2 GetMovement()
    {
        m_timeSinceDirectionChange += Time.deltaTime;
        if(m_timeSinceDirectionChange >= enemyPathSteps[m_CurrentPathStep].TimeDuration)
        {
            if(m_CurrentPathStep == enemyPathSteps.Length - 1)
            {
                m_CurrentPathStep = 0;
            }
            else
            {
                m_CurrentPathStep++;
            }

            m_timeSinceDirectionChange = 0f;
        }
        return enemyPathSteps[m_CurrentPathStep].MoveDirection.normalized;
    }

    protected override void OnDeath()
    {
        CustomEvents.EventUtil.DispatchEvent(CustomEventList.ADD_POINTS, new object[1] { m_pointValue });
        m_alive = false;
        SetActive(false);
        //this.gameObject.SetActive(false);
            //Object.Destroy(this.gameObject);
    }

    protected override void OnParameterChange(CustomEvents.EventArgs evt)
    {
        m_slowDownPercent = GameplayParameters.instance.SlowDownPercent;
    }
}