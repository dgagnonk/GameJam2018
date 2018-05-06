using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;


namespace GameJam2018
{
    public class SplashScreen : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        } 

        // Update is called once per frame
        void Update()
        {
            for(int i=0; i < 4; i++)
            {
                GamePadState state = GamePad.GetState((PlayerIndex)i);
                if(state.Buttons.Start == ButtonState.Pressed)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            
        }
    }

}
