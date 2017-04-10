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
    public Vector3 PositionTwo { get { return positionTwo; } }

    private Vector3 startingPosition;
    private Vector3 targetPosition;

    [Header("Path Components")]
    [SerializeField]
    private GameObject pathPrefab;
    private GameObject path;

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

        platformCollider.enabled = false;
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
    }

    #region EditorScripts

    public void CreatePath()
    {
        if(path != null)
        {
            DestroyImmediate(path);
        }

        path = Instantiate(pathPrefab) as GameObject;

        path.GetComponent<MovingPlatformPath>().SetupPath(this);
    }

#endregion
}
