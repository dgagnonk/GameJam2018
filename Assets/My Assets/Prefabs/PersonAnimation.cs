using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonAnimation : MonoBehaviour {

    public Animator p_Animation;
    public Rigidbody person;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        p_Animation.SetBool("Walking", false);
        p_Animation.SetBool("Talking", false);
        p_Animation.SetBool("Listening", false);

        if (person.velocity.x != 0 || person.velocity.z != 0)
            p_Animation.SetBool("Walking", true);
        else
            p_Animation.SetBool("Listening", true);

    }
}
