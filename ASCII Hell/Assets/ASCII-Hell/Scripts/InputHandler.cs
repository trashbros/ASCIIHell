using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private bool usingUDP = false;
    [SerializeField] private bool usingTcp = false;
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
    [SerializeField] private string confirmButtonTcp = "eE";
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
        Debug.Log("Key stroke recieved is: " + cmd);

        if(connectionMethod == ConnectionType.UDP)
        {
            InputContainer.instance.confirm.down = confirmButtonUDP.Contains(cmd);
            InputContainer.instance.cancel.down = cancelButtonUDP.Contains(cmd);

            //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);
            InputContainer.instance.fire.down = fireButtonUDP.Contains(cmd);
            InputContainer.instance.slowTime.down = slowTimeButtonUDP.Contains(cmd);

            //converts axis input into button input for menus
            InputContainer.instance.menuUp.down = upButtonUDP.Contains(cmd);
            InputContainer.instance.menuDown.down = downButtonUDP.Contains(cmd);
            InputContainer.instance.menuRight.down = rightButtonUDP.Contains(cmd);
            InputContainer.instance.menuLeft.down = leftButtonUDP.Contains(cmd);
        }

        if(connectionMethod == ConnectionType.TCP)
        {
            InputContainer.instance.confirm.down = confirmButtonTcp.Contains(cmd);
            InputContainer.instance.cancel.down = cancelButtonTcp.Contains(cmd);

            //InputContainer.instance.menuOpen.down = Input.GetButton(menuButton);
            InputContainer.instance.fire.down = fireButtonTcp.Contains(cmd);
            InputContainer.instance.slowTime.down = slowTimeButtonTcp.Contains(cmd);

            //converts axis input into button input for menus
            InputContainer.instance.menuUp.down = upButtonTcp.Contains(cmd);
            InputContainer.instance.menuDown.down = downButtonTcp.Contains(cmd);
            InputContainer.instance.menuRight.down = rightButtonTcp.Contains(cmd);
            InputContainer.instance.menuLeft.down = leftButtonTcp.Contains(cmd);
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
