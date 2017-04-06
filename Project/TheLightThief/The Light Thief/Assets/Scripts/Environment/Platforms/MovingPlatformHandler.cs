﻿using System.Collections;
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
                pcMovementController.KillPlayerMovement();

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

        EnablePlayerInput();
    }
}