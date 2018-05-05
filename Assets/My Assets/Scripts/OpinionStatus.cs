
/*
 * This script keeps track of which opinion a follower has based on the number of players (2 - 4)
 * It can be attached to all Followers.
 *
 * Opinion percentages are stored in the Opinions array. E.g. Opinions[0] = 20%, Opinions[1] = 80%... 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameJam2018
{
    public class OpinionStatus : MonoBehaviour
    {

        private int PlayerCount;

        public float[] opinions;
        public float[] Opinions { get { return opinions; } }

        void Start()
        {
            this.PlayerCount = Constants.PlayerCount;
            this.opinions = new float[PlayerCount];

            // Uncomment this part to mess around with the system for testing
            //Test();

        }


        // For testing purposes
        void Test()
        {
            try
            {
                this.opinions[0] = 0.5f;
                this.opinions[1] = 0.0f;
                this.opinions[2] = 0.0f;
                this.opinions[3] = 0.5f;
            }
            catch
            { /* don't fuggin care */ }


            AddToOpinion(0, 0.1f);

            foreach (float f in this.opinions)
                Debug.Log(f.ToString());

            float sum = 0.0f;
            foreach (float f in this.opinions)
                sum += f;

            Debug.Log(sum.ToString()); // We always want all opinion percentages to add up to 1
        }

        // All opinions in the array add up to 1, since they are percentages
        // When you add more onto a specific opinion, an equal fraction of the added amount is removed from the other opinions
        // OpinionIndex : The index of the opinions array of which to add "ToAdd"
        public void AddToOpinion(int OpinionIndex, float ToAdd)
        {
            if (ToAdd > 1.0f - this.opinions[OpinionIndex]) ToAdd = 1.0f - this.opinions[OpinionIndex];
            if (ToAdd <= 0.0f) return;

            Opinions[OpinionIndex] += ToAdd;

            var ZeroOpinions = (from op in this.opinions where op == 0.0f select op).ToArray(); // Get zero opinions and "skip" so we don't get negatives
            float ToRemove = ToAdd / (PlayerCount - ZeroOpinions.Length - 1);

            for (int i = 0; i < PlayerCount; i++)
            {
                if (i == OpinionIndex) continue;
                if (this.opinions[i] == 0.0f) continue;

                this.opinions[i] -= ToRemove;
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}

