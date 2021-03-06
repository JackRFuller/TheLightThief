﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementHandler : MonoSingleton<CameraMovementHandler>
{
    [Header("Movement Attributes")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private AnimationCurve movementCurve;

    //Lerping Variables
    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private bool isMoving;
    private float timeStartedMoving;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction">false is going down, true is going up</param>
    public void InitCameraMovement(bool direction)
    {
        startingPosition = this.transform.position;

        if(!direction)
        {
            targetPosition = new Vector3(this.transform.position.x,
                                     this.transform.position.y - 36.0f,
                                     this.transform.position.z);
        }
        else
        {
            targetPosition = new Vector3(this.transform.position.x,
                                     this.transform.position.y + 36.0f,
                                     this.transform.position.z);
        }
        

        timeStartedMoving = Time.time;
        isMoving = true;

        //Disable Player Input
        EventManager.TriggerEvent(Events.DisablePlayerMovement);
    }

    private void Update()
    {
        if (isMoving)
            MoveCamera();
    }

    private void MoveCamera()
    {
        float timeSinceStarted = Time.time - timeStartedMoving;
        float percentageComplete = timeSinceStarted / movementSpeed;

        Vector3 newPos = Vector3.Lerp(startingPosition, targetPosition, movementCurve.Evaluate(percentageComplete));
        this.transform.position = newPos;

        if(percentageComplete >= 1.0f)
        {
            StopCameraMovement();
        }
    }

    private void StopCameraMovement()
    {
        isMoving = false;
        EventManager.TriggerEvent(Events.EnablePlayerMovement);
    }
}
