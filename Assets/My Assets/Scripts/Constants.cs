using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameJam2018
{
    public class Constants : MonoBehaviour
    {
        public const int PlayerCount = 4;

        public List<Material> CommonMaterials = new List<Material>();

        private void Start()
        {
            CommonMaterials.Add(Resources.Load("Materials/Yellow", typeof(Material)) as Material);
            CommonMaterials.Add(Resources.Load("Materials/Green", typeof(Material)) as Material);
            CommonMaterials.Add(Resources.Load("Materials/Red", typeof(Material)) as Material);
            CommonMaterials.Add(Resources.Load("Materials/Pink", typeof(Material)) as Material);
        }

    }
}


