using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this with a rigidbody and collider to your character
public class PlayerController : MonoBehaviour, ICollidable
{
    public static PlayerController instance;

    // Character movement speed, adjust to match your game scale
    private float m_speed = 3.0f;

    // Rigidbody, collider, and sprite renderer, and animator attached to your 2D character. It grabs these items at scene start
    Rigidbody2D rigidbody2d;
    //Collider2D collider;
    BulletHell.ProjectileEmitterBase shooter;
    SpriteRenderer sprite;

    // Define the layer your interactable colliders are on
    public LayerMask interactMask;

    // Defined by the DialogueUI script while dialogue is running
    [SerializeField] private bool m_gamePaused = false;
    [SerializeField] private bool m_gameSlowed = false;


    protected void Start()
    {
        // Get the components attached to your character. Animator commented out for still sprite testing
        rigidbody2d = GetComponentInChildren<Rigidbody2D>();
        //collider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        shooter = GetComponentInChildren<BulletHell.ProjectileEmitterBase>();
        instance = this;

        m_speed = GameplayParameters.instance.PlayerSpeed;


        CustomEvents.EventUtil.AddListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.AddListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.AddListener(CustomEventList.SLOW_TIME, OnSlowTime);
    }

    // Update is called once per frame
    protected void Update()
    {
        // Do nothing if dialogue is playing
        if (m_gamePaused)
        {
            return;
        }

        if (InputContainer.instance.fire.down || InputContainer.instance.fire.pressed)
        {
            // Fire at opponents
            shooter.FireProjectile(Vector2.up, 0.1f);
        }

        if(!m_gameSlowed && (InputContainer.instance.slowTime.down || InputContainer.instance.slowTime.pressed))
        {
            Debug.Log("Calling slow down time!");
            // Slow down game time
            StartCoroutine(SlowDownTimer.RunSlowDownTimer());
        }

        // Get your up/down/left/right player input
        Vector2 move = GetMovement();

        // Get your current position
        Vector2 position = rigidbody2d.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        position = position + move * m_speed * Time.deltaTime;

        // Tell the rigidbody to move to the positon specified
        rigidbody2d.MovePosition(position);
    }


    private Vector2 GetMovement()
    {
        if (!InputContainer.instance.networkButtons)
        {
            return InputContainer.instance.moveDir;
        }

        Vector2 moveDir = new Vector2(0, 0);

        if (InputContainer.instance.menuRight.down || InputContainer.instance.menuRight.pressed)
        {
            moveDir.x = 1;
        }
        else if(InputContainer.instance.menuLeft.down || InputContainer.instance.menuLeft.pressed)
        {
            moveDir.x = -1;
        }

        if (InputContainer.instance.menuUp.down || InputContainer.instance.menuUp.pressed)
        {
            moveDir.y = 1;
        }
        else if (InputContainer.instance.menuDown.down || InputContainer.instance.menuDown.pressed)
        {
            moveDir.y = -1;
        }

        return moveDir.normalized;
    }

    public void OnHit(GameObject collision)
    {
        if (!m_gamePaused)
        {
            Debug.Log("Hit by particle");
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.PLAYER_DIED);
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
        m_speed = GameplayParameters.instance.PlayerSpeed;
    }
    #endregion

    private void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.PARAMETER_CHANGE, OnParameterChange);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.GAME_PAUSED, OnGamePause);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.SLOW_TIME, OnSlowTime);
    }
}
