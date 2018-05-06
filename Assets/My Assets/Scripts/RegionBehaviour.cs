using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam2018
{
    public class RegionBehaviour : MonoBehaviour
    {

        public RegionSpawner regionSpawner;
        public List<GameObject> PeopleInRegion;

        public float DominantMindshareAtPercent = 0.51f;
        public int KingOfRegion = -1;
        public float[] CurrentMindshares;
        public int ID;

        // Use this for initialization
        void Start()
        {
            PeopleInRegion = new List<GameObject>();
            regionSpawner = GameObject.Find("Region Spawner").GetComponent<RegionSpawner>();
            CurrentMindshares = new float[Constants.PlayerCount];
        }

        // Update is called once per frame
        void Update()
        {
            CalcMindshare();
        }

        void CalcMindshare()
        {
            float[] opTotals = new float[Constants.PlayerCount];
            //float grandTotal = 0.0f;

            // This double loop may look bad but the inner loop only executes 2-4 times
            foreach (GameObject gameObj in PeopleInRegion)
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

                CurrentMindshares[i] = (opTotals[i] / PeopleInRegion.Count);

                if ((opTotals[i] / PeopleInRegion.Count) >= DominantMindshareAtPercent)
                {             
                    KingOfRegion = i;
                    this.gameObject.GetComponent<MeshRenderer>().material = regionSpawner.Materials[i];
                    break;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Person")
                CalcMindshare();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Person")
            {
                PeopleInRegion.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Person")
            {
                PeopleInRegion.Remove(other.gameObject);
            }
        }
    }
}


