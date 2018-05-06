using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam2018
{
    public class RegionSpawner : MonoBehaviour {

        public GameObject RegionPrefab;
        public GameObject Ground;
        private Vector3 GroundSize;
        public List<GameObject> Grid;
        public List<Material> Materials;

        public List<GameObject> CapturablePrefabs;
        private Dictionary<string, double> _capturableProbabilities = new Dictionary<string, double>()
        {
            { "None", 0.75 },
            { "Billboard", 0.25 }
        };

        public float RegionSize; // Keep in mind Ground is 10x10, so RegionSize should be 10 / 4 = 2.5 for a 4x4 grid
        public int Dimension = 4;

	    void Start ()
        {

            GroundSize = Ground.GetComponent<MeshFilter>().mesh.bounds.size;

            Grid = new List<GameObject>();

            // Add regionsize because the region's centre point is in the middle of the cube
            // The point on which to start the grid spawning
            Vector3 spawnpt = new Vector3(-10f + RegionSize, 0.0f, -10f + RegionSize);

            int IDcounter = 0;

            for (int i = 0; i < Dimension; i++)
            {
                float origX = spawnpt.x;

                for (int j = 0; j < Dimension; j++)
                {
                    GameObject cellX = Instantiate(RegionPrefab, this.transform, true);

                    cellX.GetComponent<RegionBehaviour>().ID = IDcounter;

                    //cellX.GetComponent<MeshRenderer>().material = Materials[(int)Random.Range(0, 4)];

                    cellX.transform.localScale = new Vector3(RegionSize * 2, 0.01f, RegionSize * 2);
                    cellX.transform.position = spawnpt;

                    spawnpt.x += (RegionSize * 2);

                    double capturableRoll = UnityEngine.Random.value;

                    double prob = 0;

                    string selectedCapturableName = "None";

                    foreach(string key in this._capturableProbabilities.Keys)
                    {
                        prob += this._capturableProbabilities[key];

                        if(capturableRoll <= prob)
                        {
                            selectedCapturableName = key;
                            break;
                        }
                    }

                    if (!selectedCapturableName.Equals("None"))
                    {
                        foreach(GameObject capturablePrefab in CapturablePrefabs)
                        {
                            if (capturablePrefab.name.Equals(selectedCapturableName))
                            {
                                cellX.GetComponent<RegionBehaviour>().AssignCapturable(capturablePrefab);
                            }
                        }
                    }


                    IDcounter++;
               
                }

                spawnpt.x = origX;

                GameObject cellZ = Instantiate(RegionPrefab, this.transform, true);

                cellZ.GetComponent<RegionBehaviour>().ID = IDcounter;

                //cellZ.GetComponent<MeshRenderer>().material = Materials[(int)Random.Range(0, 4)];
                cellZ.transform.localScale = new Vector3(RegionSize * 2, 0.01f, RegionSize * 2);
                cellZ.transform.position = spawnpt;

                spawnpt.z += (RegionSize * 2);

                IDcounter++;
            }

	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}


