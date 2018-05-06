using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public Texture endScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        float canvasWidth = this.gameObject.GetComponent<RectTransform>().rect.width;
        float canvasHeight = this.gameObject.GetComponent<RectTransform>().rect.height;

       
        GUIContent img = new GUIContent();
        img.image = endScreen;
        GUI.Label(new Rect(0, 0, canvasWidth, canvasHeight), img);

        if(GUI.Button(new Rect(0, 0, 250, 50), "Restart"))
        {
            SceneManager.LoadScene("PlayerWithNPCs");
        }
        
    }
}
