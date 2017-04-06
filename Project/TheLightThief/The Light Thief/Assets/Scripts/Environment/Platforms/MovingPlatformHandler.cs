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
    [SerializeField]
    private Vector3 positionOne;
    [SerializeField]
    private Vector3 positionTwo;

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    [Header("Path Components")]
    [SerializeField]
    private Transform pointOne;
    [SerializeField]
    private Transform pointTwo;
    [SerializeField]
    private Transform path;

    //Lerping Attributes
    private float timeStarted;
    private bool isMoving;
    public bool IsMoving { get { return isMoving; } } //Used by Switches to determine whether to move
    private int movementCount = 0;    

    public void StartMoving()
    {
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

        if (movementCount == 0)
            targetPosition = positionTwo;
        else
            targetPosition = positionOne;

        startingPosition = this.transform.position;

        timeStarted = Time.time;
        isMoving = true;
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
        this.transform.position = targetPosition;

        isMoving = false;

        movementCount++;
        if (movementCount > 1)
            movementCount = 0;

        if(pcMovementController)
            pcMovementController.MakePlayerCollidable();
        EnablePlayerInput();
        EventManager.TriggerEvent(Events.RecalibrateNodes);
    }

    #region EditorScripts

    public void SetPath()
    {
        //Get Center Point
        Vector3 centerPoint = this.GetComponent<Collider>().bounds.center;

        Vector3 pointOnePos = new Vector3(centerPoint.x,
                                          centerPoint.y,
                                          1.0f);

        Vector3 pointTwoPos = new Vector3(centerPoint.x,
                                         positionTwo.y,
                                         1.0f);

        pointOne.position = pointOnePos;
        pointTwo.position = pointTwoPos;

        //Work out difference
        float diff = (pointOnePos.y + pointTwoPos.y) * 0.5f;

        Vector3 pathPos = new Vector3(centerPoint.x,
                                      diff,
                                      1);

        path.position = pathPos;
    }


#endregion
}
