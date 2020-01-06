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

    private bool m_isMoving = false;

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
        if(m_isMoving)
        {
            return;
        }

        Vector2 move = GetMovement();

        if(move == Vector2.zero)
        {
            return;
        }

        // Get your current position
        Vector2 position = this.transform.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        var posAdd = move * m_speed * Time.deltaTime * (!UseSlowDown ? 1f : (m_gameSlowed ? m_slowDownPercent : 1f));

        //position += posAdd;
        position = position + new Vector2(Mathf.RoundToInt(posAdd.x), Mathf.RoundToInt(posAdd.y));

        this.transform.position = new Vector2(position.x, position.y);
        //StartCoroutine(SmoothMovement(position));
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

    #region Coroutines

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        m_isMoving = true;
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            var inverseMoveTime = m_speed * Time.deltaTime * (!UseSlowDown ? 1f : (m_gameSlowed ? m_slowDownPercent : 1f));

            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(transform.position, end, inverseMoveTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            transform.position = new Vector2(newPostion.x, newPostion.y);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        m_isMoving = false;
    }

    #endregion

    protected void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.SLOW_TIME, OnSlowTime);
    }
}
