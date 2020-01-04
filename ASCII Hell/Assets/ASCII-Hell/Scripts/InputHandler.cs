using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class InputHandler : MonoBehaviour
{
    // All child IInputReceivers will get a reference to this
    [System.Serializable]
    private enum ConnectionType
    {
        UDP,
        TCP,
        Local
    }

    [SerializeField] private ConnectionType connectionMethod = ConnectionType.TCP;

    [Header("UDP Controls")]
    [SerializeField] private string confirmButtonUDP = "eE";
    [SerializeField] private string cancelButtonUDP = "qQ";
    [SerializeField] private string upButtonUDP = "uU";
    [SerializeField] private string downButtonUDP = "dD";
    [SerializeField] private string leftButtonUDP = "lL";
    [SerializeField] private string rightButtonUDP = "rR";
    [SerializeField] private string fireButtonUDP = "fF";
    [SerializeField] private string slowTimeButtonUDP = "xX";


    [Header("TCP Controls")]
    [SerializeField] private string confirmButtonTcp = "\\r";
    [SerializeField] private string cancelButtonTcp = "qQ";
    [SerializeField] private string upButtonTcp = "wW";
    [SerializeField] private string downButtonTcp = "sS";
    [SerializeField] private string leftButtonTcp = "aA";
    [SerializeField] private string rightButtonTcp = "dD";
    //[SerializeField] private string nwButtonTcp = "eE";
    [SerializeField] private string fireButtonTcp = "fF";
    [SerializeField] private string slowTimeButtonTcp = "xX";

    [Header("Local Controls")]
    [SerializeField] private string confirmButton = "Submit";
    [SerializeField] private string cancelButton = "Cancel";
    [SerializeField] private string fireButton = "Jump";
    [SerializeField] private string slowTimeButton = "Fire3";
    //[SerializeField] private string menuButton = "Menu";

    //[Range(0, 1)]
    //public float analogueDeadzone = 0.05f;

    private int networkReceivedFrame;

    void Awake()
    {
        InputContainer.instance = new InputContainer();
    }

    public void SetInputs(string cmd)
    {
        if (!cmd.Equals("*"))
        {
            Debug.Log("Key stroke recieved is: " + cmd);

            if(connectionMethod == ConnectionType.UDP)
            {
                InputContainer.instance.confirm.down = Regex.Unescape(confirmButtonUDP).Contains(cmd);
                InputContainer.instance.cancel.down = Regex.Unescape(cancelButtonUDP).Contains(cmd);

                //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);
                InputContainer.instance.fire.down = Regex.Unescape(fireButtonUDP).Contains(cmd);
                InputContainer.instance.slowTime.down = Regex.Unescape(slowTimeButtonUDP).Contains(cmd);

                //converts axis input into button input for menus
                InputContainer.instance.menuUp.down = Regex.Unescape(upButtonUDP).Contains(cmd);
                InputContainer.instance.menuDown.down = Regex.Unescape(downButtonUDP).Contains(cmd);
                InputContainer.instance.menuRight.down = Regex.Unescape(rightButtonUDP).Contains(cmd);
                InputContainer.instance.menuLeft.down = Regex.Unescape(leftButtonUDP).Contains(cmd);
            }

            if(connectionMethod == ConnectionType.TCP)
            {
                InputContainer.instance.confirm.down = Regex.Unescape(confirmButtonTcp).Contains(cmd);
                InputContainer.instance.cancel.down = Regex.Unescape(cancelButtonTcp).Contains(cmd);

                //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);
                InputContainer.instance.fire.down = Regex.Unescape(fireButtonTcp).Contains(cmd);
                InputContainer.instance.slowTime.down = Regex.Unescape(slowTimeButtonTcp).Contains(cmd);

                //converts axis input into button input for menus
                InputContainer.instance.menuUp.down = Regex.Unescape(upButtonTcp).Contains(cmd);
                InputContainer.instance.menuDown.down = Regex.Unescape(downButtonTcp).Contains(cmd);
                InputContainer.instance.menuRight.down = Regex.Unescape(rightButtonTcp).Contains(cmd);
                InputContainer.instance.menuLeft.down = Regex.Unescape(leftButtonTcp).Contains(cmd);
            }
        }
        else
        {
            InputContainer.instance.confirm.down = false;
            InputContainer.instance.cancel.down = false;
            InputContainer.instance.fire.down = false;
            InputContainer.instance.slowTime.down = false;
            InputContainer.instance.menuUp.down = false;
            InputContainer.instance.menuDown.down = false;
            InputContainer.instance.menuRight.down = false;
            InputContainer.instance.menuLeft.down = false;
        }

        //input.start.down = Input.GetButton("Start");
        //input.select.down = Input.GetButton("Select");
        InputContainer.instance.networkButtons = connectionMethod != ConnectionType.Local;

        if (
                InputContainer.instance.confirm.down    ||
                InputContainer.instance.cancel.down     ||
                InputContainer.instance.start.down      ||
                InputContainer.instance.select.down     ||
                InputContainer.instance.fire.down       ||
                InputContainer.instance.slowTime.down   ||
                InputContainer.instance.menuOpen.down
            )
        {
            InputContainer.instance.anyButton = true;
        }
        else
        {
            InputContainer.instance.anyButton = false;
        }

        networkReceivedFrame = TimescaleManager.Instance.trueFrameCount;
    }
    void Update()
    {
        if(connectionMethod != ConnectionType.Local)
        {
            if (networkReceivedFrame != TimescaleManager.Instance.trueFrameCount) { SetInputs("*"); }
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
        InputContainer.instance.slowTime.down = Input.GetButton(slowTimeButton);

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
        InputContainer.instance.networkButtons = connectionMethod != ConnectionType.Local;

        if (InputContainer.instance.confirm.down || InputContainer.instance.cancel.down || InputContainer.instance.start.down
            || InputContainer.instance.select.down || InputContainer.instance.fire.down || InputContainer.instance.slowTime.down || InputContainer.instance.menuOpen.down) InputContainer.instance.anyButton = true;
        else InputContainer.instance.anyButton = false;
        //Vector2 controllerInput = new Vector2(
        //    Input.GetAxisRaw("Horizontal"),
        //    Input.GetAxisRaw("Vertical")
        //);


    }
}
