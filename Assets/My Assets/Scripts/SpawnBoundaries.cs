using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is just 4 rectangular prisms that move around to represent the possible area that NPCs can spawn in. 
[ExecuteInEditMode]
public class SpawnBoundaries : MonoBehaviour
{

    public float xMin, xMax;
    public float y;
    public float zMin, zMax;
    public float boundaryWidth = 0.25f;

    public GameObject xMinBoundary, xMaxBoundary;
    public GameObject zMinBoundary, zMaxBoundary;

    public GameObject boundaryCube;

    [ExecuteInEditMode]
    void Update()
    {
        drawBoundaries();
    }

    [ExecuteInEditMode]
    void drawBoundaries()
    {   
        //If we're missing an object, instantiate it!
        if (xMinBoundary == null)
        {
            xMinBoundary = Instantiate(boundaryCube, this.gameObject.transform);
            xMinBoundary.name = "xMinBoundary";
        }

        if (!xMaxBoundary)
        {
            xMaxBoundary = Instantiate(boundaryCube, this.gameObject.transform);
            xMaxBoundary.name = "xMaxBoundary";
        }

        if (!zMinBoundary)
        {
            zMinBoundary = Instantiate(boundaryCube, this.gameObject.transform);
            zMinBoundary.name = "zMinBoundary";
        }

        if (!zMaxBoundary)
        {
            zMaxBoundary = Instantiate(boundaryCube, this.gameObject.transform);
            zMaxBoundary.name = "zMaxBoundary";
        }

        //Set the objects' position/scale to reflect the boundaries 
        zMinBoundary.transform.position = new Vector3((xMin + xMax) / 2, y, zMin);
        zMaxBoundary.transform.position = new Vector3((xMin + xMax) / 2, y, zMax);
        xMinBoundary.transform.position = new Vector3(xMin, y, (zMin + zMax) / 2);
        xMaxBoundary.transform.position = new Vector3(xMax, y, (zMin + zMax) / 2);

        zMinBoundary.transform.localScale = new Vector3(xMax - xMin + boundaryWidth, boundaryWidth, boundaryWidth);
        zMaxBoundary.transform.localScale = new Vector3(xMax - xMin + boundaryWidth, boundaryWidth, boundaryWidth);
        xMinBoundary.transform.localScale = new Vector3(boundaryWidth, boundaryWidth, zMax - zMin + boundaryWidth);
        xMaxBoundary.transform.localScale = new Vector3(boundaryWidth, boundaryWidth, zMax - zMin + boundaryWidth);
    }
}
