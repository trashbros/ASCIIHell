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

    //[SerializeField] private TextAsset enemyPath;

    [SerializeField] private EnemyPathStep[] enemyPathSteps;

    float m_timeSinceDirectionChange = 0f;
    //Vector2 m_MoveDir;
    int m_CurrentPathStep = 0;
    

    Rigidbody2D m_rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();

        //if(enemyPath != null)
        //{
        //    enemyPathSteps = EnemyPathReader.ReadPathData(enemyPath.text);
        //}

        if(enemyPathSteps == null)
        {
            enemyPathSteps = new EnemyPathStep[1];
            enemyPathSteps[0].MoveDirection = Vector2.zero;
            enemyPathSteps[0].TimeDuration = 50f;
        }

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

        Vector2 move = GetMovement(m_rigidbody2d.position);

        // Get your current position
        Vector2 position = m_rigidbody2d.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        position = position + move * m_speed * Time.deltaTime * (m_gameSlowed ? m_slowDownPercent : 1f);

        // Tell the rigidbody to move to the positon specified
        m_rigidbody2d.MovePosition(position);
    }

    private Vector2 GetMovement(Vector2 currentPos)
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
