
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

    [System.Serializable]
    public class Opinion
    {
        public float Percent;
        public Color Colour;

        //This constructor will generate a random colour
        //Probably only best to use this for testing since we can't detect when a colour has been used for another follower
        public Opinion(float Percent)
        {
            this.Percent = Percent;
            this.Colour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        public Opinion(float Percent, Color Colour)
        {
            this.Percent = Percent;
            this.Colour = Colour;
        }
    }


    public class OpinionStatus : MonoBehaviour
    {

        private int PlayerCount;

        [Header("Opinions")]
        [Tooltip("Reminder that all opinion percentages should add up to 1.")]
        public Opinion[] Opinions;

        public List<Opinion> test = new List<Opinion>();

        void Start()
        {

            this.PlayerCount = Constants.PlayerCount;
            this.Opinions = new Opinion[PlayerCount];

            // Uncomment this part to mess around with the system for testing
            Test();

        }


        // For testing purposes
        void Test()
        {
            try
            {
                this.Opinions[0] = new Opinion(0.25f, Color.red);
                this.Opinions[1] = new Opinion(0.25f, Color.white);
                this.Opinions[2] = new Opinion(0.25f, Color.black);
                this.Opinions[3] = new Opinion(0.25f, Color.yellow);
            }
            catch
            { /* don't fuggin care */ }


            AddToOpinion(Mathf.FloorToInt(Random.Range(0,4)), 0.26f);

            foreach (Opinion o in this.Opinions)
                Debug.Log(o.Percent.ToString());

            float sum = 0.0f;
            foreach (Opinion o in this.Opinions)
                sum += o.Percent;

            Debug.Log(sum.ToString()); // We always want all opinion percentages to add up to 1
        }

        // All opinions in the array add up to 1, since they are percentages
        // When you add more onto a specific opinion, an equal fraction of the added amount is removed from the other opinions
        // OpinionIndex : The index of the opinions array of which to add "ToAdd"
        public void AddToOpinion(int OpinionIndex, float ToAdd)
        {
            if (ToAdd > 1.0f - this.Opinions[OpinionIndex].Percent) ToAdd = 1.0f - this.Opinions[OpinionIndex].Percent;
            if (ToAdd <= 0.0f) return;

            Opinions[OpinionIndex].Percent += ToAdd;

            var ZeroOpinions = (from op in this.Opinions where op.Percent == 0.0f select op).ToArray(); // Get zero opinions and "skip" so we don't get negatives
            float ToRemove = ToAdd / (PlayerCount - ZeroOpinions.Length - 1);

            for (int i = 0; i < PlayerCount; i++)
            {
                if (i == OpinionIndex) continue;
                if (this.Opinions[i].Percent == 0.0f) continue;

                this.Opinions[i].Percent -= ToRemove;
            }
        }

        //a "dominant" opinion is one that has more than 50% of control 
        //returns -1 if there isn't a dominant opinion, 0-3 for the dominant opinion (index of the opinion)
        public int getDominantOpinion()
        {
            if(Opinions.Length == 0)
            {
                Debug.Log("Opinions array is empty!");
            }
            for(int i = 0; i < 4; i++)
            {
                if (Opinions[i] == null)
                {
                    Debug.Log("Opinion was null!");
                }
                if (Opinions[i].Percent > 0.5f)
                {
                    return i;
                }
            }
            return -1;
        }

    }

}

