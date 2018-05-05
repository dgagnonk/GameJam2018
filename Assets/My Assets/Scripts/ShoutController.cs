using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameJam2018
{
    public class ShoutController : MonoBehaviour
    {

        private PlayerCharacter _playerCharacter;

        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;

        // Use this for initialization
        void Start()
        {
            this._playerCharacter = this.GetComponentInParent<PlayerCharacter>();
            this._meshRenderer = this.GetComponent<MeshRenderer>();
            this._meshCollider = this.GetComponent<MeshCollider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        internal void StartShout()
        {
            this._meshCollider.enabled = true;
            this._meshRenderer.enabled = true;
        }

        internal void EndShout()
        {
            this._meshCollider.enabled = false;
            this._meshRenderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {

            GameObject target = other.gameObject;
            if (!target.tag.Equals("Person")) { return; }

            var targetOpinion = target.GetComponent<OpinionStatus>();
            targetOpinion.AddToOpinion(this._playerCharacter.playerIndex, 0.2f);
        }
    }
}

