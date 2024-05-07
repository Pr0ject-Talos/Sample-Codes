using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseTwo : MonoBehaviour
{
    public NavMeshAgent agent;
    //Animator anim;
    AudioSource GuardSteps;


    public enum State
    {
        Patrol,
        Chase,
        Wary,

    }

    public State state;
    public bool alive;
    public float timer = 5f;
    private bool timerStart = false;
    public bool Following = false;
    public GameObject[] waypoints;

    private int waypointInd;
    public float patrolSpeed = 2f;

    public float ChaseSpeed = 4f;
    public GameObject target;
    private GameObject AIPos;





    void Start()
    {

        agent = GetComponent<NavMeshAgent>();


        agent.updatePosition = true;
        agent.updateRotation = false;
        AIPos = GameObject.Find("Enemy");
        

        waypointInd = Random.Range(0, waypoints.Length);


        state = AIChaseTwo.State.Patrol;

        alive = true;
        StartCoroutine("AIState");
        GuardSteps = GetComponent<AudioSource>();
    }

    IEnumerator AIState()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Patrol:
                    Patrol();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Wary:
                    Wary();
                    break;

            }
            yield return null;
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;
        timerStart = false;
        timer = 3f;
        if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) > 2)
        {
            agent.SetDestination(waypoints[waypointInd].transform.position);

            //print("Works");

        }
        else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
        {

            waypointInd = Random.Range(0, waypoints.Length);
        }

    }


    public void Chase()
    {
        Following = true;
        agent.SetDestination(target.transform.position);
       
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            state = AIChaseTwo.State.Chase;
            target = coll.gameObject;
        }


    }

    void Wary()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(agent.transform.position);
        timerStart = true;
        if (timer < 0f)
        {
            Following = false;
            state = AIChaseTwo.State.Patrol;
        }


    }




    private void OnTriggerExit(Collider coll)
    {
        state = AIChaseTwo.State.Wary;
    }

    //Update is called once per framee
    void Update()
    {
        if (timerStart == true)
        {
            timer -= Time.deltaTime;
        }


        if (Following == false)
        {
            Vector3 direction = waypoints[waypointInd].transform.position - transform.position;

            float angle = Vector3.Angle(direction, transform.forward);
            if (angle > 5f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
            }
        }
        else if (Following == true)
        {
            Vector3 Newdirection = target.transform.position - transform.position;

            float angle = Vector3.Angle(Newdirection, transform.forward);
            if (angle > 5f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Newdirection), 5f * Time.deltaTime);
            }
        }

        GuardSteps.Play();
    }
}
