/*
 * This class monitors the play area for a possible victory (51% of mineshare)
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam2018
{

    public class ScoreMonitor : MonoBehaviour {

        // Need this to go through the spawned persons list
        public PersonSpawner PersonSpawner;

        // To write percentages and other data
        public Canvas GUICanvas;

        // Victory is triggered at this percent. 51% by default.
        public float VictoryTriggeredAtPercent = 0.51f;

        private List<GameObject> people;

        private int Victory = -1; // If victory > -1, then we know a player won

        public float[] CurrentMindshares;

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

            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;

            string player0percent = Mathf.RoundToInt(CurrentMindshares[0] * 100).ToString() + "%"; 
            GUI.Label(new Rect(200, 15, 100, 100), player0percent, style);

            string player1percent = Mathf.RoundToInt(CurrentMindshares[1] * 100).ToString() + "%";
            GUI.Label(new Rect(965, 15, 100, 100), player1percent, style);

            if (Constants.PlayerCount == 3)
            {
                string player2percent = (CurrentMindshares[2] * 100).ToString() + "%";
                GUI.Label(new Rect(200, 550, 100, 100), player2percent, style);
            }
            else if (Constants.PlayerCount == 4)
            {
                string player2percent = Mathf.RoundToInt(CurrentMindshares[2] * 100).ToString() + "%";
                GUI.Label(new Rect(200, 555, 100, 100), player2percent, style);


                string player3percent = Mathf.RoundToInt(CurrentMindshares[3] * 100).ToString() + "%";
                GUI.Label(new Rect(965, 555, 100, 100), player3percent, style);
            }

            
        }

    }
}


