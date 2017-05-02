using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHandler : NonStaticPlatform
{
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private AnimationCurve movementCurve;    

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    private List<Collider> platformColliders = new List<Collider>();
   
    //Lerping Attributes
    private float timeStarted;
    private bool isMoving;
    public bool IsMoving { get { return isMoving; } } //Used by Switches to determine whether to move
    private int movementCount = 0;

    [HideInInspector]
    public MovingPlatformPath path;
    private Transform originalParent;

    protected override void Start()
    {
        base.Start();
      
        originalParent = this.transform.parent;

        //Find All Colliders
        foreach(Transform child in this.transform)
        {
            Collider[] colliders = child.GetComponents<Collider>();
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].tag == "Node" || colliders[i].transform.parent.tag == "Node")
                {
                    platformColliders.Add(colliders[i]);
                }
            }
        }

        platformColliders.Add(this.GetComponent<Collider>());

        if (inversionHandler)
            inversionHandler.LateStart();
    }


    public void StartMoving(Vector3 startPosition, Vector3 endPosition)
    {
        
        //check if We're Connected to a Rotating Platform
        if(this.transform.parent != null && originalParent != null)
        {
            if (this.transform.parent != originalParent)
            {
                this.transform.parent = originalParent;
            }
        }

        //Kill Player Movement
        if(hasPlayerOnPlatform)
        {
            if (pcMovementController)
            {
                pcMovementController.KillPlayerMovement();
                pcMovementController.MakePlayerNonColliable();
            }
            DisablePlayerInput();
        }
        else
        {
            //Turn off Colliders
            for(int i = 0; i < platformColliders.Count; i++)
            {
                platformColliders[i].enabled = false;
            }
        }

        startingPosition = new Vector3(startPosition.x,
                                       startPosition.y,
                                       0);
        targetPosition = new Vector3(endPosition.x,
                                     endPosition.y,
                                     0);

        timeStarted = Time.time;
        isMoving = true;

        platformCollider.enabled = false;

        path.IsMoving = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isMoving)
            MoveTowardsDestination();  
    }

    private void MoveTowardsDestination()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / movementSpeed;

        Vector3 newPos = Vector3.Lerp(startingPosition, targetPosition, movementCurve.Evaluate(percentageComplete));
        this.transform.position = newPos;

        if(percentageComplete >= 1.0f)
        {
            StopPlatform();
        }
    }

    private void StopPlatform()
    {
        path.IsMoving = false;

        platformCollider.enabled = true;


        //Turn On Colliders
        for (int i = 0; i < platformColliders.Count; i++)
        {
            platformColliders[i].enabled = true;
        }

        platformAudio.PlayOneShot(platformAudio.clip);

        this.transform.position = targetPosition;

        isMoving = false;

        movementCount++;
        if (movementCount > 1)
            movementCount = 0;

        if(pcMovementController)
            pcMovementController.MakePlayerCollidable();
        EnablePlayerInput();
        EventManager.TriggerEvent(Events.RecalibrateNodes);
        EventManager.TriggerEvent(Events.PlatformsInPlace);
    }


    /// <summary>
    /// Triggered by Rotating Platfrom Addon
    /// Tells Platform it is no longer with path
    /// </summary>
    public void DisconnectFromPath()
    {
        if(path)
        {
            path.DisconnectPlatform();
            path = null;
        }
            
    }
}
