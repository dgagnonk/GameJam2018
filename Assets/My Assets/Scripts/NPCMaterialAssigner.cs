using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMaterialAssigner : MonoBehaviour {

    public GameObject mesh;
    public Material material;
    Material liveMaterial;
	// Use this for initialization
	void Start () {
        mesh.GetComponent<MeshRenderer>().material = new Material(material);
        liveMaterial = mesh.GetComponent<MeshRenderer>().material;
	}
	
	public Material getMaterial()
    {
        return liveMaterial;
    }
}
