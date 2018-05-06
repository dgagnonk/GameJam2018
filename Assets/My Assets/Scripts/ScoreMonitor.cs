/*
 * This class monitors the play area for a possible victory (51% of mineshare)
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam2018
{

    public class ScoreMonitor : MonoBehaviour {

        // Need this to go through the spawned persons list
        public PersonSpawner PersonSpawner;

        // To write percentages and other data
        public Canvas GUICanvas;
        private float canvasWidth;
        private float canvasHeight;

        // Victory is triggered at this percent. 51% by default.
        public float VictoryTriggeredAtPercent = 0.51f;

        private List<GameObject> people;

        private int Victory = -1; // If victory > -1, then we know a player won

        public float[] CurrentMindshares;

        [Header("GUI Stuff")]
        public List<Texture> Portraits = new List<Texture>();
        public Texture Brain;
        public List<Texture> WinScreens = new List<Texture>();

	    // Use this for initialization
	    void Start ()
        {
            
            people = PersonSpawner.people;
            CurrentMindshares = new float[Constants.PlayerCount];

	    }

        #region Victory Monitor

        void VictoryMonitor()
        {
            float[] opTotals = new float[Constants.PlayerCount];
            //float grandTotal = 0.0f;

            // This double loop may look bad but the inner loop only executes 2-4 times
            foreach (GameObject gameObj in people)
            {
                OpinionStatus opStat = gameObj.GetComponent<OpinionStatus>();
                for (int i = 0; i < Constants.PlayerCount; i++)
                {
                    opTotals[i] += opStat.Opinions[i].Percent;
                    //grandTotal += opTotals[i];
                }
            }

            

            for (int i = 0; i < Constants.PlayerCount; i++)
            {
                //Debug.Log("Opinion " + i.ToString() + " has " + (opTotals[i] / people.Count).ToString() + "%, total is " + opTotals[i].ToString() + ", grand total is " + people.Count.ToString());

                CurrentMindshares[i] = (opTotals[i] / people.Count);

                if ((opTotals[i] / people.Count) >= VictoryTriggeredAtPercent)
                {
                    Victory = i;
                    break;
                }
            }
        }

        #endregion

        void Update()
        {

            canvasWidth = GUICanvas.GetComponent<RectTransform>().rect.width;
            canvasHeight = GUICanvas.GetComponent<RectTransform>().rect.height;

            if (Victory > -1)
            {
                //----------------------------------------
                // END GAME LOGIC HERE
                //---------------------------------------- 
                Debug.Log("Player " + Victory.ToString() + " has already won, why do we keep going?");
            }

            VictoryMonitor();

        }

        private void OnGUI()
        {
            if (Victory > -1)
            {

                Debug.Log("Loading scene: " + Victory.ToString() + "GameOver");
                SceneManager.LoadScene(Victory.ToString() + "GameOver");
            }



            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;




            // ------------------ PLAYER 0 ------------------ 
            string player0percent = Mathf.RoundToInt(CurrentMindshares[0] * 100).ToString() + "%"; 
            GUI.Label(new Rect(205, 20, 100, 100), player0percent, style);

            GUIContent portrait0 = new GUIContent();
            portrait0.image = Portraits[0];
            GUI.Label(new Rect(50, 10, 100, 100), portrait0);

            GUIContent brain0 = new GUIContent();
            brain0.image = Brain;
            GUI.Label(new Rect(140, 0, 60, 60), brain0);


            // ------------------ PLAYER 1 ------------------ 
            string player1percent = Mathf.RoundToInt(CurrentMindshares[1] * 100).ToString() + "%";
            GUI.Label(new Rect(canvasWidth - 250, 20, 100, 100), player1percent, style);

            GUIContent portrait1 = new GUIContent();
            portrait1.image = Portraits[1];
            GUI.Label(new Rect(canvasWidth - 150, 10, 100, 100), portrait1);

            GUIContent brain1 = new GUIContent();
            brain1.image = Brain;
            GUI.Label(new Rect(canvasWidth - 200, 0, 60, 60), brain1);




            if (Constants.PlayerCount == 3)
            {
                // ------------------ PLAYER 2 ------------------ 
                string player2percent = (CurrentMindshares[2] * 100).ToString() + "%";
                GUI.Label(new Rect(205, canvasHeight - 110, 100, 100), player2percent, style);

                GUIContent portrait2 = new GUIContent();
                portrait2.image = Portraits[2];
                GUI.Label(new Rect(50, canvasHeight - 120, 100, 100), portrait2);

                GUIContent brain2 = new GUIContent();
                brain2.image = Brain;
                GUI.Label(new Rect(140, canvasHeight - 130, 60, 60), brain2);

            }
            else if (Constants.PlayerCount == 4)
            {
                // ------------------ PLAYER 2 ------------------ 
                string player2percent = (CurrentMindshares[2] * 100).ToString() + "%";
                GUI.Label(new Rect(205, canvasHeight - 110, 100, 100), player2percent, style);

                GUIContent portrait2 = new GUIContent();
                portrait2.image = Portraits[2];
                GUI.Label(new Rect(50, canvasHeight - 120, 100, 100), portrait2);

                GUIContent brain2 = new GUIContent();
                brain2.image = Brain;
                GUI.Label(new Rect(140, canvasHeight - 130, 60, 60), brain2);

                // ------------------ PLAYER 3 ------------------ 
                string player3percent = Mathf.RoundToInt(CurrentMindshares[3] * 100).ToString() + "%";
                GUI.Label(new Rect(canvasWidth - 250, canvasHeight - 110, 100, 100), player3percent, style);

                GUIContent portrait3 = new GUIContent();
                portrait3.image = Portraits[3];
                GUI.Label(new Rect(canvasWidth - 150, canvasHeight - 120, 100, 100), portrait3);

                GUIContent brain3 = new GUIContent();
                brain3.image = Brain;
                GUI.Label(new Rect(canvasWidth - 200, canvasHeight - 130, 60, 60), brain3);
            }

            
        }

    }
}


