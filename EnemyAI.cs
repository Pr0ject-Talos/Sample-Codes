
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

[RequireComponent(typeof(AwarenessSystem))]
public class EnemyAI : MonoBehaviour

{

    public NavMeshAgent agent;

    public bool alive;
    public float timer = 5f;
    private bool timerStart = false;
    public float patrolSpeed = 2f;

    public float ChaseSpeed = 4f;
    private GameObject AIPos;
    public GameObject[] waypoints;
    private int waypointInd;
    

    [SerializeField] TextMeshProUGUI FeedbackDisplay;

    [SerializeField] float _VisionConeAngle = 60f;
    [SerializeField] float _VisionConeRange = 30f;
    [SerializeField] Color _VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    [SerializeField] float _HearingRange = 20f;
    [SerializeField] Color _HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);

    [SerializeField] float _ProximityDetectionRange = 3f;
    [SerializeField] Color _ProximityRangeColour = new Color(1f, 1f, 1f, 0.25f);

    public Vector3 EyeLocation => transform.position;
    public Vector3 EyeDirection => transform.forward;

    public float VisionConeAngle => _VisionConeAngle;
    public float VisionConeRange => _VisionConeRange;
    public Color VisionConeColour => _VisionConeColour;

    public float HearingRange => _HearingRange;
    public Color HearingRangeColour => _HearingRangeColour;

    public float ProximityDetectionRange => _ProximityDetectionRange;
    public Color ProximityDetectionColour => _ProximityRangeColour;

    public float CosVisionConeAngle { get; private set; } = 0f;

    AwarenessSystem Awareness;

    void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad);
        Awareness = GetComponent<AwarenessSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();

        agent.updatePosition = true;
      
        AIPos = GameObject.Find("Enemy");
        waypointInd = Random.Range(0, waypoints.Length);
        

        alive = true;

        Patrol();
    }

  
    void Update()
    {
        if (timerStart == true)
        {
            timer -= Time.deltaTime;
        }

       
    }

    public void ReportCanSee(DetectableTarget seen)
    {
        Awareness.ReportCanSee(seen);
    }

    public void ReportCanHear(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        Awareness.ReportCanHear(source, location, category, intensity);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        Awareness.ReportInProximity(target);
    }

    public void OnSuspicious()
    {
        
    }

    public void OnDetected(GameObject target)
    {
        agent.SetDestination(target.transform.position);
    }

    public void OnFullyDetected(GameObject target)
    {
       
        agent.speed = ChaseSpeed;
        agent.SetDestination(target.transform.position);

      
    }

    public void OnLostDetect(GameObject target)
    {
       
        agent.speed = patrolSpeed;
        agent.SetDestination(agent.transform.position);
        timerStart = true;
    }

    public void OnLostSuspicion()
    {
   
        agent.speed = patrolSpeed;
    }

    public void OnFullyLost()
    {
      
        Patrol();
    }

    public void Patrol()
    {
        agent.speed = patrolSpeed;
        timerStart = false;
        timer = 3f;
        if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) > 2)
        {
            agent.SetDestination(waypoints[waypointInd].transform.position);

           

        }
        else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
        {

            waypointInd = Random.Range(0, waypoints.Length);
        }
    }
    private void OnTriggerExit(Collider coll)
    {
       if (coll.gameObject.tag == "Stone")
        {
            Destroy(coll);
            Patrol();
        } 
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as EnemyAI;

        // draw the detectopm range
        Handles.color = ai.ProximityDetectionColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // draw the hearing range
        Handles.color = ai.HearingRangeColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.HearingRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = ai.VisionConeColour;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);
    }
}
#endif // UNITY_EDITOR