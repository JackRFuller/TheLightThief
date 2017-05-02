using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilMovement : BaseMonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform devilMesh;
    private Animator devilAnim;
    private Rigidbody rb;
    private Collider devilCollider;

    [Header("Movement Attributes")]
    [SerializeField]
    private float walkSpeed;
    [HideInInspector]
    public Vector3 targetPoint;
    [HideInInspector]
    public bool isMoving;

    private Vector3 desiredVelocity;
    private float lastSqrMag;
    private float playerRotation;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        devilCollider = this.GetComponent<Collider>();
    }

    /// <summary>
    /// Called from Input Manager
    /// </summary>
    public void MoveToPosition()
    {
        if(isMoving)
        {
            if(isMoving)
            {
                //Update World Rotation
                playerRotation = Utilities.GetObjectZWorldRotation(this.transform);

                if(playerRotation == 0 || playerRotation == 180)
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

                devilMesh.localRotation = Quaternion.Euler(CalculateMeshLookAtVector());

                //Calculate Movement
                Vector3 directionalVector = (targetPoint - this.transform.position).normalized * walkSpeed;
                lastSqrMag = Mathf.Infinity;
                desiredVelocity = directionalVector;
            }
        }
    }

    public override void UpdateNormal()
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

    public override void UpdateFixed()
    {
        rb.velocity = desiredVelocity;
    }

    private Vector3 CalculateMeshLookAtVector()
    {
        Vector3 lookAtDir = Vector3.zero;

        Quaternion rot = new Quaternion();
        rot = Quaternion.Euler(transform.eulerAngles);
        float zAxis = Mathf.Abs(Mathf.Round(rot.eulerAngles.z));

        if(zAxis == 0)
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
            if(targetPoint.x > transform.position.x)
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
            if (targetPoint.y > transform.position.y)
            {
                lookAtDir = new Vector3(0, 90, 0);
            }
            else
            {
                lookAtDir = new Vector3(0, 270, 0);
            }
        }
        else if(zAxis == 270)
        {
            if(targetPoint.y > transform.position.y)
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

}
