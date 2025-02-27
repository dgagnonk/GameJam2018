using System;
using UnityEngine;
using XInputDotNetPure;

namespace GameJam2018
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private PlayerCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private GamePadState state;
        private GamePadState prevState;

        private PlayerIndex Index
        {
            get
            {
                return (PlayerIndex)this.gameObject.GetComponent<PlayerCharacter>().playerIndex;
            }
        }

        
        private void Start()
        {
     
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<PlayerCharacter>();

            this.state = GamePad.GetState(this.Index);
        }


        private void Update()
        {
            this.prevState = this.state;
            this.state = GamePad.GetState(this.Index);
            if (!m_Jump)
            {
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                m_Jump = this.state.Buttons.A == ButtonState.Pressed;
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            
            float h = this.state.ThumbSticks.Left.X;
            float v = this.state.ThumbSticks.Left.Y;

            bool crouch = false;

            if(this.state.Buttons.X == ButtonState.Pressed)
            {
                m_Character.Talk();
            }

            if(this.state.Buttons.Y == ButtonState.Pressed)
            {
                m_Character.Shout();
            }


            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
