using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovementController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform pcMesh;    
    private Animator pcAnim;
    private Rigidbody rb;
    private Collider pcCollider;

    [Header("Movement Attributes")]
    [SerializeField]
    private float movespeed;
    [SerializeField]
    private float sneakSpeed;
    private float currentSpeed;
    [HideInInspector]
    public Vector3 targetPoint;
    [HideInInspector]
    public bool isMoving;
    private Vector3 desiredVelocity;
    private float lastSqrMag;
   

    //Rotation
    private float playerRotation;

    private void Start()
    {
        //Get Components        
        rb = this.GetComponent<Rigidbody>();
        pcAnim = pcMesh.GetComponent<Animator>();
        pcCollider = this.GetComponent<Collider>();
    }

    /// <summary>
    /// Called when the PC Path Finding Handler gets a valid path
    /// </summary>
    /// <param name="destination"></param>
    public void MoveToPosition()
    {
        if(isMoving)
        {
            //Get Current world Rotation
            playerRotation = Utilities.GetObjectZWorldRotation(this.transform);

            //Adjust Target Based On Rotation & Work Out Look Rotation
            if (playerRotation == 0 || playerRotation == 180)
            {
                targetPoint = new Vector3(targetPoint.x,
                                          this.transform.position.y,
                                          this.transform.position.z);
            }
            else
            {
                targetPoint = new Vector3(this.transform.position.x,
                                          targetPoint.y,
                                          this.transform.position.z);
            }

            //Look At Point
            pcMesh.localRotation = Quaternion.Euler(CalculateMeshLookAtVector());

            if (pcAnim.GetBool("isSneaking"))
            {
                currentSpeed = sneakSpeed;
            }
            else
            {
                currentSpeed = movespeed;
            }

            //Calculate Movement
            Vector3 directionalVector = (targetPoint - this.transform.position).normalized * currentSpeed;
            lastSqrMag = Mathf.Infinity;
            desiredVelocity = directionalVector;
        }       
    }

    private void Update()
    {
        CalculateMovementVector();
    }   

    private void CalculateMovementVector()
    {
        float sqrMag = (targetPoint - this.transform.position).sqrMagnitude;

        if(sqrMag > lastSqrMag)
        {
            desiredVelocity = Vector3.zero;
            isMoving = false;
        }

        lastSqrMag = sqrMag;
    }

    private void FixedUpdate()
    {
        if(desiredVelocity == Vector3.zero)
        {
            pcAnim.SetInteger("movement",0);
        }
        else
        {
            pcAnim.SetInteger("movement", 1);
        }
        rb.velocity = desiredVelocity;
    }  

    /// <summary>
    /// Triggered if Player is on a Moving Platform
    /// Or There is No Path Available
    /// </summary>
    public void KillPlayerMovement()
    {
        targetPoint = this.transform.position;
        desiredVelocity = Vector3.zero;
    }

    /// <summary>
    /// Triggered when on rotating/moving platform to make sure player doesn't collide with any other platform
    /// </summary>
    public void MakePlayerNonColliable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ghost");
        pcCollider.enabled = false;

    }

    public void MakePlayerCollidable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        pcCollider.enabled = true;
    }
    
    private Vector3 CalculateMeshLookAtVector()
    {
        Vector3 lookAtDir = Vector3.zero;

        Quaternion rot = new Quaternion();
        rot = Quaternion.Euler(transform.eulerAngles);
        float zAxis = Mathf.Abs(Mathf.Round(rot.eulerAngles.z));        

        if(zAxis == 0)
        {
            if(targetPoint.x > transform.position.x)
            {
                lookAtDir = new Vector3(0, 90, 0);
            }
            else
            {
                lookAtDir = new Vector3(0, 270, 0);
            }
        }
        else if(zAxis == 180)
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
        else if(zAxis == 90)
        {
            if(targetPoint.y > transform.position.y)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Node"))
        {
            this.transform.parent = other.transform;
        }
    }
}
