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

    private void Update()
    {
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
