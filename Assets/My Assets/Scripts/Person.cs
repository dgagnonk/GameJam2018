using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameJam2018
{
    public class Person : MonoBehaviour
    {


        /*
         *  PERSON TYPES
         *  A person type is an int between 1 and 4 (inclusive).
         */

        public int HowManyPersonTypes = 4;
        public int PersonType;

        private Animator _animator;

        void Start()
        {
            PersonType = (int)Random.Range(1, HowManyPersonTypes + 1); // +1 here because max is exclusive
            this._animator = this.GetComponentInChildren<Animator>();

            this._animator.SetBool("Walking", true);
            this._animator.SetBool("Listening", false);
            this._animator.SetBool("Talking", false);


            //this._animator.runtimeAnimatorController.
        }

    }
}


