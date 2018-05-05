using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam2018
{

    public class PersonSpawner : MonoBehaviour
    {
        public GameObject personPrefab;
        public int numPeople = 10;
        public List<GameObject> people;
        public bool randomSpawn; //if true, NPCs will spawn randomly within boundaries. If false, NPCs will spawn on a grid.
        public int rows = 1;        //Number of rows/columns to spawn NPCs in
        public int columns = 1;
        public Transform peopleParent;

        public SpawnBoundaries boundaries;

        //Ensure the list of people is empty, and spawn NPCs.
        void Start()
        {
            people.Capacity = 0;
            spawnPeople();
        }


        void spawnPeople()
        {
            //If we want to spawn randomly, then spawn the desired amount of people randomly. 
            if (randomSpawn)
            {
                for (int i = 0; i < numPeople; i++)
                {
                    spawnPerson(new Vector3(Random.Range(boundaries.xMin, boundaries.xMax), 0, Random.Range(boundaries.zMin, boundaries.zMax)));
                }
            }
            //If we want to evenly space the NPCs
            else
            {
                //X and Z offset are the amount of space between NPC spawns
                float xOffset = (boundaries.xMax - boundaries.xMin) / columns;
                float zOffset = (boundaries.zMax - boundaries.zMin) / rows;

                //The initial offset is the minimum plus half the offset - just a feather so that NPCs don't spawn right against the edge
                float xInitialOffset = boundaries.xMin + (xOffset / 2);
                float zInitialOffset = boundaries.zMin + (zOffset / 2);

                //Loop through in 2 dimensions, spawning NPCs.
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        spawnPerson(new Vector3(xInitialOffset + (i * xOffset), 0, zInitialOffset + (j * zOffset)));
                        Debug.Log((i * xOffset).ToString() + " " + (j * zOffset).ToString());
                    }
                }
            }
        }

        //Spawn an NPC at the provided position.
        void spawnPerson(Vector3 position)
        {
            people.Add(Instantiate(personPrefab, position, Quaternion.identity, peopleParent));
            people[people.Count - 1].GetComponent<Wander>().boundaries = boundaries;
        }
    }

}