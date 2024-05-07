// Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    DDAManager DDAManagerScript;

    public Transform[] waypoints;
    public float speed;

    private int currentWaypoint = 0;
    private bool spottedPlayer = false;

    private void Start()
    {
        DDAManagerScript = GameObject.FindGameObjectWithTag("DDAManager").GetComponent<DDAManager>();
    }

    private void Update()
    {
        speed = DDAManagerScript.varToChange;

        // Move towards the current waypoint
        Vector3 direction = waypoints[currentWaypoint].position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // Rotate to face the current waypoint
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle > 5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
        }

        // Check if we reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        // If we spotted the player, respawn them
        if (spottedPlayer)
        {
            RespawnPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has been spotted
            spottedPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is no longer spotted
            spottedPlayer = false;
        }
    }

    private void RespawnPlayer()
    {
        //TODO: Implement method to respawn player
    }
}