using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState
{
    WANDERING,
    TALKING,
    LISTENING
}

public class Person : MonoBehaviour {


    /*
     *  PERSON TYPES
     *  A person type is an int between 1 and 4 (inclusive).
     */

    public int HowManyPersonTypes = 4;
    public int PersonType;
    public NPCState state;
    [Tooltip("The chance that a conversation will start when two people collide")]
    public float conversationStartChance = 0.5f;
    public GameObject conversationPrefab;

	void Start ()
    {
        PersonType = (int)Random.Range(1, HowManyPersonTypes + 1); // +1 here because max is exclusive
	}

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Conversation Collider" && state == NPCState.WANDERING && other.gameObject.GetComponentInParent<Person>().state == NPCState.WANDERING)
        {
            if (Random.Range(0.0f, 1.0f) < conversationStartChance)
            {
                int dominantOpinion = gameObject.GetComponent<GameJam2018.OpinionStatus>().getDominantOpinion();

                if(dominantOpinion != -1)
                {
                    //Debug.Log("Starting Conversation");

                    Person conversationStarter = gameObject.GetComponentInParent<Person>();
                    Person conversationJoiner = other.gameObject.GetComponentInParent<Person>();
                    //create conversation
                    //the conversation spreads the speaker's opinions to the other NPCs in the conversation
                    //NPCs that aren't currently talking 
                    state = NPCState.TALKING;
                    conversationJoiner.GetComponent<Person>().state = NPCState.LISTENING;
                    conversationJoiner.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;

                    //make NPCs look at eachother
                    Vector3 conversationCenter = (gameObject.transform.position + other.gameObject.transform.position) / 2;
                    gameObject.transform.forward = (gameObject.transform.position - conversationCenter).normalized;
                    conversationJoiner.gameObject.transform.forward = (conversationJoiner.gameObject.transform.position - conversationCenter).normalized;

                    //Instantiate conversation
                    Conversation conversation = Instantiate(conversationPrefab, conversationCenter, Quaternion.identity).GetComponent<Conversation>();
                    GameJam2018.OpinionStatus starterStatus = conversationStarter.GetComponent<GameJam2018.OpinionStatus>();

                    conversation.conversationStarter = conversationStarter.gameObject;

                    conversation.conversationOpinion = dominantOpinion;

                    //Spread the opinion to the other listener
                    conversationJoiner.GetComponent<GameJam2018.OpinionStatus>().AddToOpinion(dominantOpinion, conversation.conversationPower);

                    GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
                } 
            }
        }

        else if (other.tag == "Active Conversation" && gameObject.GetComponentInParent<Person>().state == NPCState.WANDERING)
        {
            Conversation joinedConversation = other.GetComponent<Conversation>();
            //Joining in-progress conversation
            state = NPCState.LISTENING;
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;

            //Look at conversation leader
            gameObject.transform.forward = (gameObject.transform.position - joinedConversation.conversationStarter.transform.position).normalized;

            //Get opinion from speaker
            gameObject.GetComponent<GameJam2018.OpinionStatus>().AddToOpinion(joinedConversation.conversationOpinion, joinedConversation.conversationPower);
        }
    }
}