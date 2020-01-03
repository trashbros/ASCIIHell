using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, ICollidable
{
    [Header("Entity Stats")]
    [SerializeField] protected int m_health = 1;
    [SerializeField] protected float m_speed = 3.0f;
    [SerializeField] protected float m_slowDownPercent = 0.5f;
    [SerializeField] protected bool m_alive = false;

    [SerializeField] protected bool UseSlowDown = false;
    [SerializeField] protected bool m_gamePaused = false;
    [SerializeField] protected bool m_gameSlowed = false;

    [SerializeField] protected SpriteRenderer m_sprite;
    [SerializeField] protected BulletHell.ProjectileEmitterBase m_emitter;

    public bool IsAlive{ get{return m_alive;}}

    // Start is called before the first frame update
    protected void Start()
    {
        if(m_sprite == null)
        {
            m_sprite = this.GetComponentInChildren<SpriteRenderer>();
        }

        if(m_emitter == null)
        {
            m_emitter = this.GetComponentInChildren<BulletHell.ProjectileEmitterBase>();
        }

        SetActive(false);

        CustomEvents.EventUtil.AddListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.AddListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.AddListener(CustomEventList.SLOW_TIME, OnSlowTime);

        //OnParameterChange();
    }

    public void Initialize(Vector2 position, int health, float speed)
    {
        m_health = health;
        this.transform.position = new Vector2(position.x, position.y);
        m_speed = speed;
        m_alive = true;
        SetActive(true);
        //this.gameObject.SetActive(true);
    }

    public void SetActive(bool onOff)
    {
        if (m_sprite != null)
        {
            m_sprite.enabled = onOff;
        }
        if (m_emitter != null)
        {
            m_emitter.enabled = onOff;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if(m_gamePaused || !m_alive)
        {
            return;
        }

        MoveEntity();
    }

    protected void MoveEntity()
    {
        Vector2 move = GetMovement();

        // Get your current position
        Vector2 position = this.transform.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        position = position + move * m_speed * Time.deltaTime * (!UseSlowDown ? 1f : (m_gameSlowed ? m_slowDownPercent : 1f));

        this.transform.position = new Vector2(position.x, position.y);
    }

    protected abstract Vector2 GetMovement();

    public void OnHit(GameObject collision)
    {
        if (!m_gamePaused)
        {
            m_health -= 1;

            if(m_health <= 0)
            {
                OnDeath();
            }
        }
    }

    protected abstract void OnDeath();

    #region EventListeners
    private void OnGamePause(CustomEvents.EventArgs evt)
    {
        m_gamePaused = (bool)evt.args.GetValue(0);
    }

    private void OnSlowTime(CustomEvents.EventArgs evt)
    {
        m_gameSlowed = (bool)evt.args.GetValue(0);
    }

    protected abstract void OnParameterChange(CustomEvents.EventArgs evt);
    #endregion

    protected void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.SLOW_TIME, OnSlowTime);
    }
}
