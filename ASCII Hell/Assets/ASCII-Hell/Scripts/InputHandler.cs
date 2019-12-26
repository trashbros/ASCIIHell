using System.Collections;
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
    [SerializeField] private string fireButtonUDP = " ";

    [SerializeField] private string confirmButton = "Submit";
    [SerializeField] private string cancelButton = "Cancel";
    [SerializeField] private string interactButton = "Jump";
    [SerializeField] private string sprintButton = "Fire3";
    [SerializeField] private string menuButton = "Menu";

    //[Range(0, 1)]
    //public float analogueDeadzone = 0.05f;

    void Awake()
    {
        InputContainer.instance = new InputContainer();
    }

    public void SetInputs(string cmd)
    {
        InputContainer.instance.moveDir = Vector2.zero;
        //if (InputContainer.instance.moveDir.magnitude < analogueDeadzone) InputContainer.instance.moveDir = Vector2.zero;
        InputContainer.instance.menuControl = InputContainer.instance.moveDir;

        //InputContainer.instance.interact.down = Input.GetButton(interactButton);
        //InputContainer.instance.run.down = Input.GetButton(sprintButton);

        InputContainer.instance.confirm.down = confirmButtonUDP.Contains(cmd);
        InputContainer.instance.cancel.down = cancelButtonUDP.Contains(cmd);

        //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);

        //converts axis input into button input for menus
        InputContainer.instance.menuUp.down = upButtonUDP.Contains(cmd);
        InputContainer.instance.menuDown.down = downButtonUDP.Contains(cmd);
        InputContainer.instance.menuRight.down = rightButtonUDP.Contains(cmd);
        InputContainer.instance.menuLeft.down = leftButtonUDP.Contains(cmd);


        //input.start.down = Input.GetButton("Start");
        //input.select.down = Input.GetButton("Select");
        InputContainer.instance.udpButtons = usingUDP;

        if (InputContainer.instance.confirm.down || InputContainer.instance.cancel.down || InputContainer.instance.start.down
            || InputContainer.instance.select.down || InputContainer.instance.interact.down || InputContainer.instance.run.down || InputContainer.instance.menuOpen.down) InputContainer.instance.anyButton = true;
        else InputContainer.instance.anyButton = false;
    }
    void Update()
    {
        if(usingUDP)
        {
            return;
        }

        Vector2 controllerInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        InputContainer.instance.moveDir = controllerInput;
        //if (InputContainer.instance.moveDir.magnitude < analogueDeadzone) InputContainer.instance.moveDir = Vector2.zero;
        InputContainer.instance.menuControl = InputContainer.instance.moveDir;

        InputContainer.instance.interact.down = Input.GetButton(interactButton);
        InputContainer.instance.run.down = Input.GetButton(sprintButton);

        InputContainer.instance.confirm.down = Input.GetButton(confirmButton);
        InputContainer.instance.cancel.down = Input.GetButton(cancelButton);

        InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);

        //converts axis input into button input for menus
        InputContainer.instance.menuUp.down = Input.GetAxisRaw("Vertical") > (0.5f);
        InputContainer.instance.menuDown.down = Input.GetAxisRaw("Vertical") < (-0.5f);
        InputContainer.instance.menuRight.down = Input.GetAxisRaw("Horizontal") > (0.5f);
        InputContainer.instance.menuLeft.down = Input.GetAxisRaw("Horizontal") < (-0.5f);


        //input.start.down = Input.GetButton("Start");
        //input.select.down = Input.GetButton("Select");
        InputContainer.instance.udpButtons = usingUDP;

        if (InputContainer.instance.confirm.down || InputContainer.instance.cancel.down || InputContainer.instance.start.down
            || InputContainer.instance.select.down || InputContainer.instance.interact.down || InputContainer.instance.run.down || InputContainer.instance.menuOpen.down) InputContainer.instance.anyButton = true;
        else InputContainer.instance.anyButton = false;
        //Vector2 controllerInput = new Vector2(
        //    Input.GetAxisRaw("Horizontal"),
        //    Input.GetAxisRaw("Vertical")
        //);


    }
}
