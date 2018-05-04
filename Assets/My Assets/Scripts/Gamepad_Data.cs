using UnityEngine;
using XInputDotNetPure;

public enum PressState
{
    IDLE,
    HELD,
    PRESSED,
    RELEASED
}

public enum Button
{
    A,
    B,
    X,
    Y,
    SELECT,
    START,
    LB,
    RB,
    LS,
    RS,
    GUIDE,
    DPAD_UP,
    DPAD_RIGHT,
    DPAD_DOWN,
    DPAD_LEFT,

    NUM_BUTTONS
}

public enum InputSide
{
    LEFT,
    RIGHT
}

public class ControllerState
{
    //All of the buttons
    public PressState button_A = PressState.IDLE;
    public PressState button_B = PressState.IDLE;
    public PressState button_X = PressState.IDLE;
    public PressState button_Y = PressState.IDLE;
    public PressState button_LB = PressState.IDLE;
    public PressState button_RB = PressState.IDLE;
    public PressState button_Start = PressState.IDLE;
    public PressState button_Select = PressState.IDLE;
    public PressState button_LS = PressState.IDLE;
    public PressState button_RS = PressState.IDLE;
    public PressState button_Guide = PressState.IDLE;
    public PressState dpad_Up = PressState.IDLE;
    public PressState dpad_Right = PressState.IDLE;
    public PressState dpad_Down = PressState.IDLE;
    public PressState dpad_Left = PressState.IDLE;

    //The thumbsticks
    public Vector2 leftStick;
    public Vector2 rightStick;

    //Triggers
    public float leftTrigger;
    public float rightTrigger;
}

//No need to create more instances of this class. All of the necessary instances are handled by the gamepad manager class
[System.Serializable]
public class Gamepad_Data
{
    //--- Public Variables ---//
    public int playerIndex;
    


    //--- Private Variables ---//
    private ControllerState visibleState;
    private GamePadState currentInternalState;
    private GamePadState previousInternalState;
    private bool firstFrame;
    private bool vibrating;
    private float timeLeftOnVibration;
    private bool isConnected;



    //--- Constructors and Destructor ---//
    public Gamepad_Data()
    {
        //Init the private variables
        playerIndex = 1;
        visibleState = new ControllerState();
        currentInternalState = new GamePadState();
        previousInternalState = new GamePadState();
        firstFrame = true;
        vibrating = false;
        timeLeftOnVibration = 0.0f;
        isConnected = false;
    }

    public Gamepad_Data(int index)
    {
        //Init the private variables
        playerIndex = index;
        visibleState = new ControllerState();
        currentInternalState = new GamePadState();
        previousInternalState = new GamePadState();
        firstFrame = true;
        vibrating = false;
        timeLeftOnVibration = 0.0f;
        isConnected = false;
    }



    //--- Getters ---//
    public ControllerState getAllData()
    {
        //Return the visible controller state information, containing all of the information. Useful for minimizing number of calls
        return visibleState;
    }

    public PressState getButton(Button button)
    {
        //Return the press state for the current button
        switch (button)
        {
            case Button.A:
                return visibleState.button_A;
            case Button.B:
                return visibleState.button_B;
            case Button.X:
                return visibleState.button_X;
            case Button.Y:
                return visibleState.button_Y;
            case Button.LB:
                return visibleState.button_LB;
            case Button.RB:
                return visibleState.button_RB;
            case Button.SELECT:
                return visibleState.button_Select;
            case Button.START:
                return visibleState.button_Start;
            case Button.LS:
                return visibleState.button_LS;
            case Button.RS:
                return visibleState.button_RS;
            case Button.GUIDE:
                return visibleState.button_Guide;
            case Button.DPAD_UP:
                return visibleState.dpad_Up;
            case Button.DPAD_RIGHT:
                return visibleState.dpad_Right;
            case Button.DPAD_DOWN:
                return visibleState.dpad_Down;
            case Button.DPAD_LEFT:
                return visibleState.dpad_Left;
            default:
                Debug.LogWarning("Unknown controller button [" + button.ToString() + "]!");
                return PressState.IDLE;
        }
    }

    public Vector2 getStick(InputSide leftOrRight)
    {
        //Return the x and y values of the left or right stick
        if (leftOrRight == InputSide.LEFT)
            return visibleState.leftStick;
        else
            return visibleState.rightStick;
    }

    public float getTrigger(InputSide leftOrRight)
    {
        //Return the x and y values of the left or right trigger
        if (leftOrRight == InputSide.LEFT)
            return visibleState.leftTrigger;
        else
            return visibleState.rightTrigger;
    }
	
	public bool getIsConnected()
    {
        //Return if the controller is currently connected or not
        return isConnected;
    }



    //--- Methods ---//
    public void updateControls()
    {
        //Store the new information as a test variable so we can check if the controller is actually connected
        GamePadState testState = GamePad.GetState((PlayerIndex)playerIndex);

        //Update if the controller is currently connected or not
        isConnected = testState.IsConnected;

        //If the controller is connected, continue to update the available information. If not, log a warning
        if (isConnected)
        {
            //The current state from last frame is now the previous state
            previousInternalState = currentInternalState;

            //Update the information for the current state by taking it from the test state we used above. No need to call it again
            currentInternalState = testState;

            //Proceed with updating the button states to handle press / release events and filling in the rest of the externally visible information
            updateVisibleStates();

            //If currently vibrating, update the vibration effect
            if (vibrating)
                updateVibration();
        }            
    }

    public void vibrate(float _duration, float _leftMotorStrength = 0.5f, float _rightMotorStrength = 0.5f)
    {
        //Vibrate the two sides of the controller as requested
        GamePad.SetVibration((PlayerIndex)playerIndex, _leftMotorStrength, _rightMotorStrength);

        //Start to vibrate
        vibrating = true;

        //Update how long to vibrate for
        timeLeftOnVibration = _duration;
    }

    public void stopVibration()
    {
        //Disable the vibration
        vibrating = false;

        //Cap the time left to 0, to avoid negatives
        timeLeftOnVibration = 0.0f;

        //Set the vibration amounts down to 0
        GamePad.SetVibration((PlayerIndex)playerIndex, 0.0f, 0.0f);
    }



    //--- Utility Functions ---//
    private void updateVisibleStates()
    {
        //Update the buttons
        updateButtons();

        //Update the sticks and the triggers
        updateSticksAndTriggers();
    }

    private void updateButtons()
    {
        //The first frame has an issue with the 'previous state' being null. Needs to wait one frame for it to work. Otherwise, every button is considered released
        if (firstFrame)
        {
            firstFrame = false;
            return;
        }

        //Update the visible values for the A button. Could be one of four values
        if (currentInternalState.Buttons.A == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.A == ButtonState.Released)
                visibleState.button_A = PressState.PRESSED;
            else
                visibleState.button_A = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.A == ButtonState.Released)
                visibleState.button_A = PressState.IDLE;
            else
                visibleState.button_A = PressState.RELEASED;
        }


        //Update the visible values for the B button. Could be one of four values
        if (currentInternalState.Buttons.B == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.B == ButtonState.Released)
                visibleState.button_B = PressState.PRESSED;
            else
                visibleState.button_B = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.B == ButtonState.Released)
                visibleState.button_B = PressState.IDLE;
            else
                visibleState.button_B = PressState.RELEASED;
        }


        //Update the visible values for the X button. Could be one of four values
        if (currentInternalState.Buttons.X == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.X == ButtonState.Released)
                visibleState.button_X = PressState.PRESSED;
            else
                visibleState.button_X = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.X == ButtonState.Released)
                visibleState.button_X = PressState.IDLE;
            else
                visibleState.button_X = PressState.RELEASED;
        }


        //Update the visible values for the Y button. Could be one of four values
        if (currentInternalState.Buttons.Y == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Y == ButtonState.Released)
                visibleState.button_Y = PressState.PRESSED;
            else
                visibleState.button_Y = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Y == ButtonState.Released)
                visibleState.button_Y = PressState.IDLE;
            else
                visibleState.button_Y = PressState.RELEASED;
        }


        //Update the visible values for the LB button. Could be one of four values
        if (currentInternalState.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.LeftShoulder == ButtonState.Released)
                visibleState.button_LB = PressState.PRESSED;
            else
                visibleState.button_LB = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.LeftShoulder == ButtonState.Released)
                visibleState.button_LB = PressState.IDLE;
            else
                visibleState.button_LB = PressState.RELEASED;
        }


        //Update the visible values for the RB button. Could be one of four values
        if (currentInternalState.Buttons.RightShoulder == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.RightShoulder == ButtonState.Released)
                visibleState.button_RB = PressState.PRESSED;
            else
                visibleState.button_RB = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.RightShoulder == ButtonState.Released)
                visibleState.button_RB = PressState.IDLE;
            else
                visibleState.button_RB = PressState.RELEASED;
        }


        //Update the visible values for the select button. Could be one of four values
        if (currentInternalState.Buttons.Back == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Back == ButtonState.Released)
                visibleState.button_Select = PressState.PRESSED;
            else
                visibleState.button_Select = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Back == ButtonState.Released)
                visibleState.button_Select = PressState.IDLE;
            else
                visibleState.button_Select = PressState.RELEASED;
        }


        //Update the visible values for the start button. Could be one of four values
        if (currentInternalState.Buttons.Start == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Start == ButtonState.Released)
                visibleState.button_Start = PressState.PRESSED;
            else
                visibleState.button_Start = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Start == ButtonState.Released)
                visibleState.button_Start = PressState.IDLE;
            else
                visibleState.button_Start = PressState.RELEASED;
        }


        //Update the visible values for the left stick button. Could be one of four values
        if (currentInternalState.Buttons.LeftStick == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.LeftStick == ButtonState.Released)
                visibleState.button_LS = PressState.PRESSED;
            else
                visibleState.button_LS = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.LeftStick == ButtonState.Released)
                visibleState.button_LS = PressState.IDLE;
            else
                visibleState.button_LS = PressState.RELEASED;
        }


        //Update the visible values for the right stick button. Could be one of four values
        if (currentInternalState.Buttons.RightStick == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.RightStick == ButtonState.Released)
                visibleState.button_RS = PressState.PRESSED;
            else
                visibleState.button_RS = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.RightStick == ButtonState.Released)
                visibleState.button_RS = PressState.IDLE;
            else
                visibleState.button_RS = PressState.RELEASED;
        }


        //Update the visible values for the guide button. Could be one of four values
        if (currentInternalState.Buttons.Guide == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Guide == ButtonState.Released)
                visibleState.button_Guide = PressState.PRESSED;
            else
                visibleState.button_Guide = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.Buttons.Guide == ButtonState.Released)
                visibleState.button_Guide = PressState.IDLE;
            else
                visibleState.button_Guide = PressState.RELEASED;
        }


        //Update the visible values for the dpad up button. Could be one of four values
        if (currentInternalState.DPad.Up == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Up == ButtonState.Released)
                visibleState.dpad_Up = PressState.PRESSED;
            else
                visibleState.dpad_Up = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Up == ButtonState.Released)
                visibleState.dpad_Up = PressState.IDLE;
            else
                visibleState.dpad_Up = PressState.RELEASED;
        }


        //Update the visible values for the dpad right button. Could be one of four values
        if (currentInternalState.DPad.Right == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Right == ButtonState.Released)
                visibleState.dpad_Right = PressState.PRESSED;
            else
                visibleState.dpad_Right = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Right == ButtonState.Released)
                visibleState.dpad_Right = PressState.IDLE;
            else
                visibleState.dpad_Right = PressState.RELEASED;
        }


        //Update the visible values for the dpad down button. Could be one of four values
        if (currentInternalState.DPad.Down == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Down == ButtonState.Released)
                visibleState.dpad_Down = PressState.PRESSED;
            else
                visibleState.dpad_Down = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Down == ButtonState.Released)
                visibleState.dpad_Down = PressState.IDLE;
            else
                visibleState.dpad_Down = PressState.RELEASED;
        }


        //Update the visible values for the dpad left button. Could be one of four values
        if (currentInternalState.DPad.Left == ButtonState.Pressed)
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Left == ButtonState.Released)
                visibleState.dpad_Left = PressState.PRESSED;
            else
                visibleState.dpad_Left = PressState.HELD;
        }
        else
        {
            //If it was released last frame but pressed this frame, it is a press event. Otherwise, it is a hold event
            if (previousInternalState.DPad.Left == ButtonState.Released)
                visibleState.dpad_Left = PressState.IDLE;
            else
                visibleState.dpad_Left = PressState.RELEASED;
        }
    }

    private void updateSticksAndTriggers()
    {
        //Update the values for the thumbsticks
        visibleState.leftStick = new Vector2(currentInternalState.ThumbSticks.Left.X, currentInternalState.ThumbSticks.Left.Y);
        visibleState.rightStick = new Vector2(currentInternalState.ThumbSticks.Right.X, currentInternalState.ThumbSticks.Right.Y);

        //Update the values for the triggers
        visibleState.leftTrigger = currentInternalState.Triggers.Left;
        visibleState.rightTrigger = currentInternalState.Triggers.Right;
    }

    private void updateVibration()
    {
        //Decrease the time remaining on the vibration
        timeLeftOnVibration -= Time.deltaTime;

        //If out of time, stop vibrating
        if (timeLeftOnVibration <= 0.0f)
            stopVibration();
    }
}
