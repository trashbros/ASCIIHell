using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this with a rigidbody and collider to your character
public class PlayerController : MonoBehaviour, ICollidable
{
    public static PlayerController instance;

    // Character movement speed, adjust to match your game scale
    public float speed = 3.0f;

    // Rigidbody, collider, and sprite renderer, and animator attached to your 2D character. It grabs these items at scene start
    Rigidbody2D rigidbody2d;
    //Collider2D collider;
    SpriteRenderer sprite;

    // Define the layer your interactable colliders are on
    public LayerMask interactMask;

    // Defined by the DialogueUI script while dialogue is running
    public bool inDialogue = false;

    private float m_horizontal = 0.0f;
    private float m_vertical = 0.0f;


    protected void Start()
    {
        // Get the components attached to your character. Animator commented out for still sprite testing
        rigidbody2d = GetComponentInChildren<Rigidbody2D>();
        //collider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        instance = this;
    }

    // Update is called once per frame
    protected void Update()
    {
        // Do nothing if dialogue is playing
        if (inDialogue)
        {
            return;
        }

        if (InputContainer.instance.fire.down || InputContainer.instance.fire.pressed)
        {
            // Fire at opponents
        }

        // Get your up/down/left/right player input
        Vector2 move = GetMovement();



        // Get your current position
        Vector2 position = rigidbody2d.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        position = position + move * speed * Time.deltaTime;

        // Tell the rigidbody to move to the positon specified
        rigidbody2d.MovePosition(position);
    }


    private Vector2 GetMovement()
    {
        if (!InputContainer.instance.udpButtons)
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
        Debug.Log("Hit by particle");
        CustomEvents.EventUtil.DispatchEvent(CustomEventList.PLAYER_DIED);
    }
}
