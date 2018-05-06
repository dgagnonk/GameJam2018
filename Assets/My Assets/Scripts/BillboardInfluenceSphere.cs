using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameJam2018
{
    public class BillboardInfluenceSphere : MonoBehaviour
    {

        private Billboard _billBoard;

        // Use this for initialization
        void Start()
        {
            this._billBoard = this.GetComponentInParent<Billboard>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            this._billBoard.AddAffectedPerson(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            this._billBoard.AddAffectedPerson(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            this._billBoard.RemoveAffectedPerson(other.gameObject);
        }
    }
}

