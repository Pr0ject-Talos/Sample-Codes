using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField] GameObject Enemy;


    AIChaseTwo NewState;

    private void Start()
    {



        NewState = Enemy.GetComponent<AIChaseTwo>();

    }


    private void Update()
    {

       
    }


    void PlayerDetected()
    {

       
        NewState.agent.SetDestination((NewState.target).transform.position);
        NewState.state = AIChaseTwo.State.Chase;
    }


    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            NewState.target = coll.gameObject;
            PlayerDetected();
            
        }
    }
}
