using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves the game object this script is attached to randomly
public class Wander : MonoBehaviour 
{
    //Public variables
    public float movementSpeed = 1.0f;  //movement speed in meters per second
    public float circleDistance = 1.0f; //the distance between the character's center and the circle used for wander calculations
    public float circleRadius = 1.0f; //radius of circle used for wander calculations

    public bool drawDebugShapes = false;

    public GameObject debugCircle;
    public GameObject debugRandomPoint;

    //Private variables
    private Rigidbody rb;

	void Start () 
	{
        //finding the rigid body to move around
        rb = GetComponent<Rigidbody>();

        //getting a random starting velocity, normalizing it, and then multiplying by the movement speed
        Vector3 startingVelocity = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        startingVelocity = startingVelocity.normalized;
        startingVelocity *= movementSpeed;
        gameObject.transform.forward = startingVelocity;

        //setting the initial movement speed
        rb.velocity = startingVelocity;
	}
	
	void Update () 
	{
        if(drawDebugShapes)
        {
            debugCircle.gameObject.SetActive(true);
            debugRandomPoint.gameObject.SetActive(true);

            debugCircle.transform.position = transform.position + (transform.forward * circleDistance);
            debugCircle.transform.localScale = new Vector3(circleRadius * 2, debugCircle.transform.localScale.y, circleRadius * 2);


        }
        else
        {
            debugCircle.gameObject.SetActive(false);
            debugRandomPoint.gameObject.SetActive(false);

        }
	}
}
