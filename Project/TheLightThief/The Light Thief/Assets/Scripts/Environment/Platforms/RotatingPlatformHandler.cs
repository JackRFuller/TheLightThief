using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformHandler : NonStaticPlatform
{
    [Header("Movement Attributes")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private AnimationCurve movementCurve;

    //Lerping Attributes
    private Quaternion startingRot;
    private Quaternion targetRot;
    private float timeStarted;
    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    public void StartPlatformRotation()
    {
        //Check if PC is on Platform
        if(hasPlayerOnPlatform)
        {
            if (pcMovementController)
            {
                pcMovementController.KillPlayerMovement();
                pcMovementController.MakePlayerNonColliable();
            }
            DisablePlayerInput();
        }

        startingRot = transform.rotation;

        targetRot = Quaternion.Euler(new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y,
                                                 transform.localEulerAngles.z + 90.0f));

        timeStarted = Time.time;
        isMoving = true;
        platformCollider.enabled = false;
    }

    private void Update()
    {
        if (isMoving)
            RotatePlatform();
    }

    private void RotatePlatform()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / movementSpeed;

        Quaternion newRot = Quaternion.Lerp(startingRot, targetRot, movementCurve.Evaluate(percentageComplete));

        transform.rotation = newRot;

        if(percentageComplete >= 1.0f)
        {
            StopPlatformRotation();
        }
    }

    private void StopPlatformRotation()
    {
        platformCollider.enabled = true;
        platformAudio.PlayOneShot(platformAudio.clip);

        isMoving = false;

        if (transform.localEulerAngles.z >= 360)
            transform.localEulerAngles = Vector3.zero;

        if(pcMovementController)
            pcMovementController.MakePlayerCollidable();

        EnablePlayerInput();
        EventManager.TriggerEvent(Events.RecalibrateNodes);
    }

}
