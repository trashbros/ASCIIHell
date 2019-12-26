using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this with a rigidbody and collider to your character
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    // Character movement speed, adjust to match your game scale
    public float speed = 3.0f;

    // Rigidbody, collider, and sprite renderer, and animator attached to your 2D character. It grabs these items at scene start
    Rigidbody2D rigidbody2d;
    //Collider2D collider;
    SpriteRenderer sprite;

    // Define with unity player input button to use for object interaction
    public string interactButton = "Jump";

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

        if (Input.GetButtonDown(interactButton))
        {

        }

        // Get your up/down/left/right player input
        float horizontal = GetHorizontalAxis();
        float vertical = GetVerticalAxis();

        // Movement direction and rate is your horizontal and vertical input values (good for joystick or arrow buttons)
        Vector2 move = new Vector2(horizontal, vertical);

        // Get your current position
        Vector2 position = rigidbody2d.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        position = position + move * speed * Time.deltaTime;

        // Tell the rigidbody to move to the positon specified
        rigidbody2d.MovePosition(position);
    }

    public void SetHorizontalAxis(float value)
    {
        m_horizontal = value;
    }

    public void SetVerticalAxis(float value)
    {
        m_vertical = value;
    }

    private float GetHorizontalAxis()
    {
        if (m_horizontal == 0.0f)
        {
            return Input.GetAxis("Horizontal");
        }
        else
        {
            return m_horizontal;
        }
    }
    private float GetVerticalAxis()
    {
        if (m_vertical == 0.0f)
        {
            return Input.GetAxis("Vertical");
        }
        else
        {
            return m_vertical;
        }
    }
}
