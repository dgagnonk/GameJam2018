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

        // Victory is triggered at this percent. 51% by default.
        public float VictoryTriggeredAtPercent = 0.51f;

        private List<GameObject> people;
        private int PlayerCount;

        private int Victory = -1; // If victory > -1, then we know a player won

	    // Use this for initialization
	    void Start ()
        {
            
            people = PersonSpawner.people;
            PlayerCount = Constants.PlayerCount;

	    }

        #region Victory Monitor

        void VictoryMonitor()
        {
            float[] opTotals = new float[PlayerCount];
            //float grandTotal = 0.0f;

            // This double loop may look bad but the inner loop only executes 2-4 times
            foreach (GameObject gameObj in people)
            {
                OpinionStatus opStat = gameObj.GetComponent<OpinionStatus>();
                for (int i = 0; i < PlayerCount; i++)
                {
                    opTotals[i] += opStat.Opinions[i].Percent;
                    //grandTotal += opTotals[i];
                }
            }

            for (int i = 0; i < PlayerCount; i++)
            {
                //Debug.Log("Opinion " + i.ToString() + " has " + (opTotals[i] / people.Count).ToString() + "%, total is " + opTotals[i].ToString() + ", grand total is " + people.Count.ToString());

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

    }
}


