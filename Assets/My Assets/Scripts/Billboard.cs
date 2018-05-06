using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJam2018
{

    public class Billboard : MonoBehaviour, ICapturable
    {

        private int _owner = -1;

        private MeshRenderer _signMeshRenderer;
        private MeshRenderer _bubbleMeshRenderer;
        private SphereCollider _bubbleCollider;

        private double _timer;
        private const double TICK_DURATION = 1;

        private List<GameObject> _affectedPeople = new List<GameObject>();

        // Use this for initialization
        void Start() {
            this._signMeshRenderer = this.GetComponent<MeshRenderer>();
            List<MeshRenderer> renderers = new List<MeshRenderer>(this.GetComponentsInChildren<MeshRenderer>());
            renderers.Remove(this._signMeshRenderer);
            this._bubbleMeshRenderer = renderers.First();
            this._bubbleCollider = this.GetComponentInChildren<SphereCollider>();
            this._signMeshRenderer.material.color = Color.grey;
        }

        // Update is called once per frame
        void Update() {

            
        }

        public void AddAffectedPerson(GameObject person)
        {
            if (person.tag.Equals("Person") && !this._affectedPeople.Contains(person))
            {
                this._affectedPeople.Add(person);
            }
        }

        public void RemoveAffectedPerson(GameObject person)
        {
            if (person.tag.Equals("Person") && this._affectedPeople.Contains(person))
            {
                this._affectedPeople.Remove(person);
            }
        }

        private void FixedUpdate()
        {
            this._timer += Time.deltaTime;

            foreach (GameObject person in this._affectedPeople)
            {
                OpinionStatus opinion = person.GetComponent<OpinionStatus>();
                opinion.AddToOpinion(this._owner, Time.deltaTime * 0.001f);
            }
            
        }

        

        public void SetOwner(int newOwner)
        {
            this._owner = newOwner;

            if (newOwner == -1)
            {
                this._signMeshRenderer.material.color = Color.grey;
                this._bubbleMeshRenderer.enabled = false;
                this._bubbleCollider.enabled = false;
            }
            else
            {
                Color ownerColor = Opinion.OpinionPalette[newOwner];
                this._signMeshRenderer.material.color = ownerColor;
                this._bubbleCollider.enabled = true;
                this._bubbleMeshRenderer.enabled = true;
                this._bubbleMeshRenderer.material.color = new Color(ownerColor.r, ownerColor.g, ownerColor.b, 0.2f);
            }
        }

        public int GetOwner()
        {
            return this._owner;
        }


    }

}