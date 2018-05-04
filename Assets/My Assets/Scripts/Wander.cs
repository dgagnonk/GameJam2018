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
    public float wanderDelta = 1.0f; //'power' of wandering, amount the wander angle changes per frame
    public float wanderSmoothingPower = 0.1f; //the power that the agent's velocity interpolates over to the goal velocity with, a rnage 

    public bool drawDebugShapes = false;

    public GameObject debugCircle;
    public GameObject debugRandomPoint;

    public SpawnBoundaries boundaries;

    //Private variables
    private Rigidbody rb;
    private Vector3 goalVelocity;
    private float wanderAngle = 0.0f;

	void Start () 
	{
        //finding the rigid body to move around
        rb = GetComponent<Rigidbody>();

        //getting a random starting velocity, normalizing it, and then multiplying by the movement speed
        Vector3 startingVelocity = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        startingVelocity = startingVelocity.normalized;
        startingVelocity *= movementSpeed;

        //Setting the goal velocity to the starting velocity, and setting the agent's rotation so that it's facing the direction it's moving
        goalVelocity = startingVelocity;
        gameObject.transform.forward = startingVelocity;

        //setting the initial movement speed
        rb.velocity = startingVelocity;
	}

    //wandering stuff based on this https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-wander--gamedev-1624
    void Update () 
	{
        //the center of the circle used for wandering calculations
        Vector3 circleCenter = goalVelocity.normalized * circleDistance;

        //displacement is the vector from the center of the circle to the outside of the circle that's rotated to calculate the wander force
        Vector3 displacement = new Vector3(0, 0, 1);
        displacement *= circleRadius;
        
        //rotating the displacement vector based on the current wander angle
        displacement = setAngle(displacement, wanderAngle);

        //getting a positive or negative wanderDelta (wanderDelta or -wanderDelta) and then adding that to the wander angle
        float angleChange = ((Random.Range(0, 2) * 2) - 1) * wanderDelta;
        wanderAngle += angleChange;

        //goal velocity is the velocity we *want* to be going, we interpolate towards it, from the current velocity based on wanderSmoothingPower
        goalVelocity = (circleCenter + displacement).normalized * movementSpeed;

        rb.velocity = Vector3.Lerp(rb.velocity, goalVelocity, wanderSmoothingPower);

        //rotate the agent so it's facing the velocity it's moving
        gameObject.transform.forward = rb.velocity.normalized;

        //ensure that the agent is within the boundaries
        if(gameObject.transform.position.x > boundaries.xMax)
        {
            gameObject.transform.position = new Vector3(boundaries.xMax, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x < boundaries.xMin)
        {
            gameObject.transform.position = new Vector3(boundaries.xMin, gameObject.transform.position.y,gameObject.transform.position.z);
        }
        if (gameObject.transform.position.z < boundaries.zMin)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, boundaries.zMin);
        }
        if (gameObject.transform.position.z > boundaries.zMax)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, boundaries.zMax);
        }

        //if enabled, draw debug shapes
        if (drawDebugShapes)
        {
            debugCircle.gameObject.SetActive(true);
            debugRandomPoint.gameObject.SetActive(true);

            debugCircle.transform.position = transform.position + circleCenter;
            debugCircle.transform.localScale = new Vector3(circleRadius * 2, debugCircle.transform.localScale.y, circleRadius * 2);

            debugRandomPoint.transform.position = debugCircle.transform.position + setAngle(new Vector3(0,0,1), wanderAngle);

        }
        else
        {
            debugCircle.gameObject.SetActive(false);
            debugRandomPoint.gameObject.SetActive(false);

        }
	}

    //rotates & returns the given vector (in 2D space only, rotates around the Y axis)
    Vector3 setAngle(Vector3 vector, float angle)
    {
        Vector3 result = Vector3.zero;

        float length = vector.magnitude;

        result.x = Mathf.Cos(angle) * length;
        result.z = Mathf.Sin(angle) * length;

        return result;
    }
}
