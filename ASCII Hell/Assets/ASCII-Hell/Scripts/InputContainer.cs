using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script provided as part of the Tutorial Stater Pack on https://www.patreon.com/taxiderby and maybe itch.io later idk
//Special thanks to @WeaverDev and @_pikopik for introducing me to this input system!!
[System.Serializable]
public class InputContainer
{
    public static InputContainer instance;

    // Contains player input values
    public InputBool fire;
    public Vector2 menuControl;
    public Vector2 moveDir;
    public InputBool slowTime;
    public InputBool confirm;
    public InputBool cancel;
    public InputBool start;
    public InputBool select;
    public InputBool menuUp;
    public InputBool menuDown;
    public InputBool menuLeft;
    public InputBool menuRight;
    public InputBool menuOpen;

    public bool anyButton = false;

    public bool udpButtons = false;

    //The player's input can be locked for a number of reasons.
    //To prevent issues like allowing the player to move again if a textbox closes while in a cutscene, there are multiple "locks."
    //For the most part, only "locked" should be checked by the player itself, but all others can be written to.
    public bool locked { get { return (manualLock || dead || cutsceneLock || textLock || menuLock); } }
    public bool manualLock = false;
    public bool dead = false;
    public bool menuLock = false;
    public bool cutsceneLock = false;
    public bool textLock = false;

    public struct InputBool
    {
        public static int doubleTapWindow = 7;
        public static int buffer = 8;
        public static float holdTime = 0.3f;

        public int lastPress;
        public float lastPressTime;
        public int lastRelease;
        public int previousPress;

        public bool buffered;
        //public bool tapBuffered; //pretty much used exclusively for combo attacks
        bool _down;
        bool _trueDown;

        public bool trueTime;

        public int frameCount
        {
            get
            {
                return trueTime ? TimescaleManager.Instance.trueFrameCount : TimescaleManager.Instance.fixedFrameCount;
            }
        }
        /// <summary>
        /// Whether the key is currently being pressed.
        /// </summary>
        /// 
        public bool down
        {
            get
            {
                return _down;
            }
            set
            {
                if (_down != value)
                {
                    if (value)
                    {
                        buffered = true;
                        previousPress = lastPress;
                        lastPress = frameCount;
                        lastPressTime = Time.unscaledTime;
                    }
                    else
                    {
                        //buffered = false;
                        lastRelease = frameCount;
                    }
                }
                _down = value;
            }
        }

        /// <summary>
        /// True during the frame the user starts pressing down the key.
        /// </summary>
        public bool pressed
        {
            get
            {
                return frameCount == lastPress;
            }
        }

        /// <summary>
        /// True doubleTapWindow frames after the key was pressed, if it was pressed only once.
        /// </summary>
        public bool singlePress
        {
            get
            {
                return frameCount == lastPress + doubleTapWindow
                    && previousPress < lastPress - doubleTapWindow;
            }
        }


        /// <summary>
        /// True if the key was pressed twice within doubleTapWindow frames.
        /// </summary>
        public bool doublePress
        {
            get
            {
                return frameCount == lastPress
                    && frameCount <= previousPress + doubleTapWindow;
            }
        }

        /// <summary>
        /// True if the key was pressed and then released within holdTime seconds.
        /// </summary>
        public bool tap
        {
            get
            {
                return frameCount == lastRelease
                    && Time.unscaledTime < lastPressTime + holdTime;
            }
        }

        /// <summary>
        /// True is the key was pressed and is still down within the doubleTapWindow. This avoids dropped inputs if the player hits jump RIGHT before landing, etc.
        /// </summary>
        public bool bufferedPress
        {
            get
            {
                if (buffered && down && frameCount <= lastPress + doubleTapWindow)
                {
                    buffered = false;
                    return true;
                }
                else return false;
            }
        }

        /// <summary>
        /// True is the key was pressed within the doubleTapWindow, regardless if it's still down or not. Useful for things like combo attacks.
        /// </summary>
        public bool bufferedTap
        {
            get
            {
                if (buffered && frameCount <= lastPress + doubleTapWindow)
                {
                    buffered = false;
                    return true;
                }
                else return false;
            }
        }

        /// <summary>
        /// Checks if the button is down within the buffered press window, without checking/setting the buffered state.
        /// </summary>
        public bool bufferedPressCheck
        {
            get
            {
                return down && frameCount <= lastPress + doubleTapWindow;
            }
        }

        /// <summary>
        /// Checks if the button was tapped within the buffered press window, without checking/setting the buffered state.
        /// </summary>
        public bool bufferedTapCheck
        {
            get
            {
                if (frameCount <= lastPress + doubleTapWindow)
                {
                    return true;
                }
                else return false;
            }
        }

        /// <summary>
        /// True if the key was held down for at least holdTime frames.
        /// </summary>
        public bool hold
        {
            get
            {
                return down && Time.unscaledTime >= lastPressTime + holdTime;
            }
        }
    }

    public void ResetInputs()
    {
        fire.down = slowTime.down = start.down = select.down = confirm.down = cancel.down
            = menuUp.down = menuLeft.down = menuRight.down = menuDown.down = menuOpen.down = false;
    }

    public InputContainer()
    {
        // Initialize the inputs so they don't all appear to have been tapped at the start of the game
        // Structs cannot have parameterless constructors or field initializers so this is the only way
        fire.lastPress = fire.previousPress = fire.lastRelease
            = slowTime.lastPress = slowTime.previousPress = slowTime.lastRelease
            = start.lastPress = start.previousPress = start.lastRelease
            = select.lastPress = select.previousPress = select.lastRelease
            = confirm.lastPress = confirm.previousPress = confirm.lastRelease
            = cancel.lastPress = cancel.previousPress = cancel.lastRelease
            = menuUp.lastPress = menuUp.previousPress = menuUp.lastRelease
            = menuDown.lastPress = menuDown.previousPress = menuDown.lastRelease
            = menuLeft.lastPress = menuLeft.previousPress = menuLeft.lastRelease
            = menuRight.lastPress = menuRight.previousPress = menuRight.lastRelease 
            = menuOpen.lastPress = menuOpen.previousPress = menuOpen.lastRelease = int.MinValue;

        //these are listed separately since, unlike other inputs, can be called if the game is paused.
        confirm.trueTime = cancel.trueTime = menuUp.trueTime = menuDown.trueTime
            = menuLeft.trueTime = menuRight.trueTime = start.trueTime = select.trueTime = menuOpen.trueTime = true;
    }

    public override string ToString()
    {
        return
            "Inputs:" +
            "\nInteract: " + fire +
            "\nRun: " + slowTime +
            "\nMove Dir: " + moveDir.ToString() +
            "\nMenu Up: " + menuUp +
            "\nMenu Down: " + menuDown +
            "\nMenu Right: " + menuRight +
            "\nMenu Left: " + menuLeft +
            "\nMenu Open: " + menuOpen +
            "\nConfirm: " + confirm +
            "\nCancel: " + cancel;
    }
}
