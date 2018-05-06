using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    public float conversationDuration = 5.0f;
    public GameObject conversationStarter;
    public int conversationOpinion = 0; //the index of which idea will be transferred through this conversation
    [Tooltip("The amount of influence that will be added to listeners in this conversation")]
    public float conversationPower = 0.1f;
	void Start ()
    {
        StartCoroutine(endConversation());
	}
	
	void Update ()
    {
		
	}

    IEnumerator endConversation()
    {
        //wait for the duration of the conversation
        yield return new WaitForSeconds(conversationDuration);

        //make everyone in the conversation start wandering again
        //state = wandering
        //speed = 1
        Collider[] talkingPeople = Physics.OverlapSphere(gameObject.transform.position, gameObject.transform.localScale.x / 2);
        foreach( Collider collider in talkingPeople)
        {
            GameJam2018.Person person = collider.GetComponentInParent<GameJam2018.Person>();
            if(person != null)
            {
                person.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 1;
                person.state = GameJam2018.NPCState.WANDERING;
            }
        }

        //destroy the conversation
        Destroy(this.gameObject);
    }
}
