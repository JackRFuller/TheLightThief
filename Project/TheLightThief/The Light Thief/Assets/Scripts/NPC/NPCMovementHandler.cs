using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementHandler : BaseMonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform NPCMesh;
    private Animator NPCAnim;
    private Rigidbody rb;
    private Collider NPCCollider;
    private NPCPathFindingHandler NPCPathHandler;

    [Header("Movement")]
    [SerializeField]
    private float movementSpeed;
    private Vector3 targetPoint;
    private Vector3 targetPointA;
    private Vector3 targetPointB;
    private int targetIndex = 0;

    private Vector3 desiredVelocity;
    private float lastSqrMag;
    private bool isMoving;

    private float NPCRotation;
    
    public NPCState npcState;
    public enum NPCState
    {
        OnMovingPlatform,
        Moving,
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.StartNPCMovement, RestartMovement);
        EventManager.StartListening(Events.NPCRecalibratePath, RestartMovement);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.StartNPCMovement, RestartMovement);
        EventManager.StopListening(Events.NPCRecalibratePath, RestartMovement);
    }

    private void Start()
    {
        //Get Components
        NPCPathHandler = this.GetComponent<NPCPathFindingHandler>();
        rb = this.GetComponent<Rigidbody>();
        NPCAnim = NPCMesh.GetComponent<Animator>();
        NPCCollider = this.GetComponent<Collider>();

        npcState = NPCState.Moving;
    }

    public void StartMoving(Vector3 pointOne, Vector3 pointTwo)
    {
        targetPointA = pointOne;
        targetPointB = pointTwo;

        //Determine Target Point
        if (targetIndex == 0)
        {
            targetPoint = targetPointA;
        }
        else
        {
            targetPoint = targetPointB;
        }

        InitiateMovement();
    }

    private void InitiateMovement()
    {
        //Get Current World Rotation
        NPCRotation = Utilities.GetObjectZWorldRotation(this.transform);

        //Adjust Look At Rotation
        Vector3 lookDirection = CalculateMeshLookAtVector();

        NPCMesh.localRotation = Quaternion.Euler(lookDirection);

        //Calculate Movement
        Vector3 directionalVector = (targetPoint - this.transform.position).normalized * movementSpeed;
        lastSqrMag = Mathf.Infinity;
        desiredVelocity = directionalVector;
        isMoving = true;
        if(!NPCAnim)
            NPCAnim = NPCMesh.GetComponent<Animator>();

        NPCAnim.SetInteger("movement", 1);

    }

    public override void UpdateNormal()
    {
        if (isMoving)
            CalculateMovementVector();
    }

    private void CalculateMovementVector()
    {
        float sqrMag = (targetPoint - this.transform.position).sqrMagnitude;

        //If NPC is at Destination
        if(sqrMag > lastSqrMag)
        {
            NPCAnim.SetInteger("movement", 0);
            desiredVelocity = Vector3.zero;
            isMoving = false;

            StartCoroutine(WaitToStartMovement(1.0f));
        }

        lastSqrMag = sqrMag;
    }  
    

    private void FixedUpdate()
    {
        if(isMoving)
        {
            rb.velocity = desiredVelocity;
        }
    }

    //Called when changing direction
    private IEnumerator WaitToStartMovement(float waitTime)
    {
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(waitTime);
        targetIndex++;
        if (targetIndex > 1)
            targetIndex = 0;

        if(targetIndex == 0)
        {
            targetPoint = targetPointA;
        }
        else
        {
            targetPoint = targetPointB;
        }

        InitiateMovement();
    }

    private Vector3 CalculateMeshLookAtVector()
    {
        Vector3 lookAtDir = Vector3.zero;

        Quaternion rot = new Quaternion();
        rot = Quaternion.Euler(transform.eulerAngles);
        float zAxis = Mathf.Abs(Mathf.Round(rot.eulerAngles.z));

        if (zAxis == 0)
        {
            if (targetPoint.x > transform.position.x)
            {
                lookAtDir = new Vector3(0, 90, 0);
            }
            else
            {
                lookAtDir = new Vector3(0, 270, 0);
            }
        }
        else if (zAxis == 180)
        {
            if (targetPoint.x > transform.position.x)
            {
                lookAtDir = new Vector3(0, 270, 0);
            }
            else
            {
                lookAtDir = new Vector3(0, 90, 0);
            }
        }
        else if (zAxis == 90)
        {
            if (targetPoint.y > transform.position.y)
            {
                lookAtDir = new Vector3(0, 90, 0);
            }
            else
            {
                lookAtDir = new Vector3(0, 270, 0);
            }
        }
        else if (zAxis == 270)
        {
            if (targetPoint.y > transform.position.y)
            {
                lookAtDir = new Vector3(0, 270, 0);
            }
            else
            {
                lookAtDir = new Vector3(0, 90, 0);
            }
        }

        return lookAtDir;
    }

    /// <summary>
    /// Called when the enemy is on a moving platform that has stoppped
    /// starts recalculating path
    /// </summary>
    public void RestartMovement()
    {
        if(npcState == NPCState.Moving)
        {
            if (NPCPathHandler)
            {
                NPCCollider.enabled = true;
                NPCPathHandler.FindNPCPath();
                Debug.Log("Looking for Path");
            }
        }
    }

    /// <summary>
    /// Used when an enemy is on a moving platform
    /// </summary>
    public void KillMovement()
    {
        NPCCollider.enabled = false;
        NPCAnim.SetInteger("movement", 0);
        rb.velocity = Vector3.zero;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Node"))
        {
            this.transform.parent = other.transform;
        }

        if(other.tag.Equals("BreathingRing"))
        {
            Debug.Log("Detected PC");
        }
    }

}
