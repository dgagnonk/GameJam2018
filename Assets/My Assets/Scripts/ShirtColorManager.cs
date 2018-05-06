using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtColorManager : MonoBehaviour {

    NPCMaterialAssigner materialHandle;
    GameJam2018.OpinionStatus opinionStatus;

    void Start ()
    {
        materialHandle = GetComponent<NPCMaterialAssigner>();
        opinionStatus = GetComponent<GameJam2018.OpinionStatus>();
    }
	
	void Update () {
        //Get dominant opinion
        GameJam2018.Opinion dominantOpinion = opinionStatus.GetHighestOpinion();

        //If -1, set shirt color to grey.
        if(dominantOpinion == null)
        {
            materialHandle.getMaterial().SetColor("Color_F9AFFB91", new Color(0.75f, 0.75f, 0.75f));
        }
   
        //If not -1, set shirt color to dominant opinion color
        else
        {
            materialHandle.getMaterial().SetColor("Color_F9AFFB91", dominantOpinion.Colour);
        }
    }
}
