using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    InputContainer inputs;
    BulletHell.ProjectileEmitterBase shooter;

    protected new void Start()
        {
            base.Start();

            shooter = GetComponentInChildren<BulletHell.ProjectileEmitterBase>();
        }

    protected new void Update()
    {
        if (m_gamePaused)
        {
            return;
        }

        inputs = InputContainer.instance;

        if (inputs.fire.down || inputs.fire.pressed)
        {
            // Fire at opponents
            shooter.FireProjectile(Vector2.up, 0.1f);
        }

        if(!m_gameSlowed && (inputs.slowTime.down || inputs.slowTime.pressed))
        {
            Debug.Log("Calling slow down time!");
            // Slow down game time
            StartCoroutine(SlowDownTimer.RunSlowDownTimer());
        }

        MoveEntity();
    }

    protected override Vector2 GetMovement()
    {
        if (!inputs.udpButtons)
        {
            return inputs.moveDir;
        }

        Vector2 moveDir = new Vector2(0, 0);

        if (inputs.menuRight.down || inputs.menuRight.pressed)
        {
            moveDir.x = 1;
        }
        else if(inputs.menuLeft.down || inputs.menuLeft.pressed)
        {
            moveDir.x = -1;
        }

        if (inputs.menuUp.down || inputs.menuUp.pressed)
        {
            moveDir.y = 1;
        }
        else if (inputs.menuDown.down || inputs.menuDown.pressed)
        {
            moveDir.y = -1;
        }

        return moveDir.normalized;
    }

    protected override void OnDeath()
    {
        m_alive = false;
        CustomEvents.EventUtil.DispatchEvent(CustomEventList.PLAYER_DIED);
    }

    protected override void OnParameterChange(CustomEvents.EventArgs evt)
    {
        //m_slowDownPercent = GameplayParameters.instance.SlowDownPercent;
    }
}