using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformHandler : NonStaticPlatform
{
    private RotatingPlatformAddOn platformAddOn;
    private List<Collider> platformColliders = new List<Collider>();

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

    protected override void Start()
    {
        base.Start();

        platformAddOn = this.GetComponent<RotatingPlatformAddOn>();

        //Find All Colliders
        foreach (Transform child in this.transform)
        {
            Collider[] colliders = child.GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                platformColliders.Add(colliders[i]);
            }
        }

        platformColliders.Add(this.GetComponent<Collider>());
        platformColliders.Add(platformCollider);
    }

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
                
        //Turn Off Colliders
        for (int i = 0; i < platformColliders.Count; i++)
        {
            platformColliders[i].enabled = false;
        }

        //Check if this Platform Has COnnected Platform
        if (platformAddOn)
            platformAddOn.DisconnectMovingPlatformsFromPath();

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

        //Turn Off Colliders
        for (int i = 0; i < platformColliders.Count; i++)
        {
            platformColliders[i].enabled = true;
        }

        if (transform.localEulerAngles.z >= 360)
            transform.localEulerAngles = Vector3.zero;

        if(pcMovementController)
            pcMovementController.MakePlayerCollidable();

        EnablePlayerInput();
        EventManager.TriggerEvent(Events.RecalibrateNodes);
        EventManager.TriggerEvent(Events.PlatformsInPlace);
    }

}
