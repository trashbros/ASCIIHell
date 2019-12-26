﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputHandler : MonoBehaviour
{
    // All child IInputReceivers will get a reference to this

    [SerializeField] private bool usingUDP = false;
    [SerializeField] private string confirmButtonUDP = "eE";
    [SerializeField] private string cancelButtonUDP = "qQ";
    [SerializeField] private string upButtonUDP = "uU";
    [SerializeField] private string downButtonUDP = "dD";
    [SerializeField] private string leftButtonUDP = "lL";
    [SerializeField] private string rightButtonUDP = "rR";
    [SerializeField] private string fireButtonUDP = "fF";
    [SerializeField] private string dashButtonUDP = "mM";

    [SerializeField] private string confirmButton = "Submit";
    [SerializeField] private string cancelButton = "Cancel";
    [SerializeField] private string fireButton = "Jump";
    [SerializeField] private string dashButton = "Fire3";
    //[SerializeField] private string menuButton = "Menu";

    //[Range(0, 1)]
    //public float analogueDeadzone = 0.05f;

    private int udpRecievedFrame;

    void Awake()
    {
        InputContainer.instance = new InputContainer();
    }

    public void SetInputs(string cmd)
    {
        //if (confirmButtonUDP.Contains(cmd))
        //{
        //    InputContainer.instance.confirm.down = true;
        //    Debug.Log("Enter pressed");
        //}

        //if(cancelButtonUDP.Contains(cmd))
        //{
        //    InputContainer.instance.cancel.down = true;
        //    Debug.Log("Exit pressed");
        //}

        //if(upButtonUDP.Contains(cmd))
        //{
        //    InputContainer.instance.menuUp.down = true;
        //}
        //else if (downButtonUDP.Contains(cmd))
        //{
        //    InputContainer.instance.menuDown.down = true;
        //}


        //if (leftButtonUDP.Contains(cmd))
        //{
        //    InputContainer.instance.menuLeft.down = true;
        //}
        //else if(rightButtonUDP.Contains(cmd))
        //{
        //    InputContainer.instance.menuRight.down = true;
        //}

        //InputContainer.instance.moveDir = Vector2.zero;
        //if (InputContainer.instance.moveDir.magnitude < analogueDeadzone) InputContainer.instance.moveDir = Vector2.zero;
        //InputContainer.instance.menuControl = InputContainer.instance.moveDir;

        //InputContainer.instance.interact.down = Input.GetButton(interactButton);
        //InputContainer.instance.run.down = Input.GetButton(sprintButton);

        InputContainer.instance.confirm.down = confirmButtonUDP.Contains(cmd);
        InputContainer.instance.cancel.down = cancelButtonUDP.Contains(cmd);

        //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);
        InputContainer.instance.fire.down = fireButtonUDP.Contains(cmd);
        InputContainer.instance.dash.down = dashButtonUDP.Contains(cmd);

        //converts axis input into button input for menus
        InputContainer.instance.menuUp.down = upButtonUDP.Contains(cmd);
        InputContainer.instance.menuDown.down = downButtonUDP.Contains(cmd);
        InputContainer.instance.menuRight.down = rightButtonUDP.Contains(cmd);
        InputContainer.instance.menuLeft.down = leftButtonUDP.Contains(cmd);


        //input.start.down = Input.GetButton("Start");
        //input.select.down = Input.GetButton("Select");
        InputContainer.instance.udpButtons = usingUDP;

        if (InputContainer.instance.confirm.down || InputContainer.instance.cancel.down || InputContainer.instance.start.down
            || InputContainer.instance.select.down || InputContainer.instance.fire.down || InputContainer.instance.dash.down || InputContainer.instance.menuOpen.down) InputContainer.instance.anyButton = true;
        else InputContainer.instance.anyButton = false;
        udpRecievedFrame = TimescaleManager.Instance.trueFrameCount;
    }
    void Update()
    {
        if(usingUDP)
        {
            if (udpRecievedFrame != TimescaleManager.Instance.trueFrameCount) { SetInputs("*"); }
            return;
        }

        Vector2 controllerInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        InputContainer.instance.moveDir = controllerInput;
        //if (InputContainer.instance.moveDir.magnitude < analogueDeadzone) InputContainer.instance.moveDir = Vector2.zero;
        InputContainer.instance.menuControl = InputContainer.instance.moveDir;

        InputContainer.instance.fire.down = Input.GetButton(fireButton);
        InputContainer.instance.dash.down = Input.GetButton(dashButton);

        InputContainer.instance.confirm.down = Input.GetButton(confirmButton);
        InputContainer.instance.cancel.down = Input.GetButton(cancelButton);

        //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);

        //converts axis input into button input for menus
        InputContainer.instance.menuUp.down = Input.GetAxisRaw("Vertical") > (0.5f);
        InputContainer.instance.menuDown.down = Input.GetAxisRaw("Vertical") < (-0.5f);
        InputContainer.instance.menuRight.down = Input.GetAxisRaw("Horizontal") > (0.5f);
        InputContainer.instance.menuLeft.down = Input.GetAxisRaw("Horizontal") < (-0.5f);


        //input.start.down = Input.GetButton("Start");
        //input.select.down = Input.GetButton("Select");
        InputContainer.instance.udpButtons = usingUDP;

        if (InputContainer.instance.confirm.down || InputContainer.instance.cancel.down || InputContainer.instance.start.down
            || InputContainer.instance.select.down || InputContainer.instance.fire.down || InputContainer.instance.dash.down || InputContainer.instance.menuOpen.down) InputContainer.instance.anyButton = true;
        else InputContainer.instance.anyButton = false;
        //Vector2 controllerInput = new Vector2(
        //    Input.GetAxisRaw("Horizontal"),
        //    Input.GetAxisRaw("Vertical")
        //);


    }
}
