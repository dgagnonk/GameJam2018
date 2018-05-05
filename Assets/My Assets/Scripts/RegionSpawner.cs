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

        public float RegionSize; // Keep in mind Ground is 10x10, so RegionSize should be 10 / 4 = 2.5 for a 4x4 grid
        public int Dimension = 4;

	    void Start ()
        {

            GroundSize = Ground.GetComponent<MeshFilter>().mesh.bounds.size;

            Grid = new List<GameObject>();

            // Add regionsize because the region's centre point is in the middle of the cube
            // The point on which to start the grid spawning
            Vector3 spawnpt = new Vector3(-10f + RegionSize, 0.0f, -10f + RegionSize);

            for (int i = 0; i < Dimension; i++)
            {
                float origX = spawnpt.x;

                for (int j = 0; j < Dimension; j++)
                {
                    GameObject cellX = Instantiate(RegionPrefab, this.transform, true);

                    cellX.GetComponent<MeshRenderer>().material = Materials[(int)Random.Range(0, 4)];

                    cellX.transform.localScale = new Vector3(RegionSize * 2, 0.01f, RegionSize * 2);
                    cellX.transform.position = spawnpt;

                    spawnpt.x += (RegionSize * 2);             
                
                
                }

                spawnpt.x = origX;

                GameObject cellZ = Instantiate(RegionPrefab, this.transform, true);

                cellZ.GetComponent<MeshRenderer>().material = Materials[(int)Random.Range(0, 4)];
                cellZ.transform.localScale = new Vector3(RegionSize * 2, 0.01f, RegionSize * 2);
                cellZ.transform.position = spawnpt;

                spawnpt.z += (RegionSize * 2);
            }

	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}


