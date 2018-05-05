using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {


    /*
     *  PERSON TYPES
     *  A person type is an int between 1 and 4 (inclusive).
     */

    public int HowManyPersonTypes = 4;
    public int PersonType;

	void Start ()
    {
        PersonType = (int)Random.Range(1, HowManyPersonTypes + 1); // +1 here because max is exclusive
	}
	
}
