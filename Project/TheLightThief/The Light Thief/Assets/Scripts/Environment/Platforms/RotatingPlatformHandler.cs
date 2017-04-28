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

    private bool hasSentMessageToNPCs;

    private void OnEnable()
    {
        EventManager.StartListening(Events.TurnOnPlatforms, TurnOnColliders);
        EventManager.StartListening(Events.TurnOffPlatforms, TurnOffColliders);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.TurnOnPlatforms, TurnOnColliders);
        EventManager.StopListening(Events.TurnOffPlatforms, TurnOffColliders);
    }

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
                if(colliders[i].tag == "Node" || colliders[i].transform.parent.tag == "Node")
                {
                    platformColliders.Add(colliders[i]);
                }
            }
        }

        platformColliders.Add(this.GetComponent<Collider>());
        platformColliders.Add(platformCollider);
    }

    /// <summary>
    /// STarts Rotating the Platform
    /// </summary>
    /// <param name="direction"> 0 = rotate right, 1 = rotate left</param>
    public void StartPlatformRotation(int direction)
    {
        //Check if PC is on Platform
        if(hasPlayerOnPlatform)
        {
            if (pcMovementController)
            {
                pcMovementController.KillPlayerMovement();
                pcMovementController.MakePlayerNonColliable();

                EventManager.TriggerEvent(Events.StartedMoving);
            }
            DisablePlayerInput();
        }

        //Check if NPC is on Platform
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].npcState = NPCMovementHandler.NPCState.OnMovingPlatform;
            enemies[i].KillMovement();
        }

        EventManager.TriggerEvent(Events.TurnOffPlatforms);
        

        //Check if this Platform Has COnnected Platform
        if (platformAddOn)
            platformAddOn.DisconnectMovingPlatformsFromPath();

        startingRot = transform.rotation;

        Vector3 rot = Vector3.zero;

        if(direction == 0)
        {
            rot = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y,
                                                 transform.localEulerAngles.z - 90.0f);
        }
        else
        {
            rot = new Vector3(transform.localEulerAngles.x,
                                                    transform.localEulerAngles.y,
                                                    transform.localEulerAngles.z + 90.0f);
        }


        targetRot = Quaternion.Euler(rot);

        timeStarted = Time.time;
        isMoving = true;
        platformCollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isMoving)
            RotatePlatform();
    }

    private void RotatePlatform()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / movementSpeed;

        Quaternion newRot = Quaternion.Lerp(startingRot, targetRot, movementCurve.Evaluate(percentageComplete));

        transform.rotation = newRot;

        if(percentageComplete >= 0.5f)
        {
            if (!hasSentMessageToNPCs)
            {
                EventManager.TriggerEvent(Events.RecalibrateNodes);
                hasSentMessageToNPCs = true;
            }
        }

        if (percentageComplete >= 1.0f)
        {
            StopPlatformRotation();
        }
    }

    private void TurnOffColliders()
    {
        //Turn ON Colliders
        for (int i = 0; i < platformColliders.Count; i++)
        {
            platformColliders[i].enabled = false;
        }
    }

    private void TurnOnColliders()
    {
        //Turn ON Colliders
        for (int i = 0; i < platformColliders.Count; i++)
        {
            platformColliders[i].enabled = true;
        }
    }

    private void StopPlatformRotation()
    {
        hasSentMessageToNPCs = false;

        platformCollider.enabled = true;
        platformAudio.PlayOneShot(platformAudio.clip);

        isMoving = false;

        EventManager.TriggerEvent(Events.TurnOnPlatforms);


        if (transform.localEulerAngles.z >= 360)
            transform.localEulerAngles = Vector3.zero;

        if(pcMovementController)
        {
            pcMovementController.MakePlayerCollidable();
            EventManager.TriggerEvent(Events.EndedMoving);
        }

        //Check if NPC is on Platform
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].npcState = NPCMovementHandler.NPCState.Moving;
            enemies[i].KillMovement();
        }        

        EnablePlayerInput();
        EventManager.TriggerEvent(Events.RecalibrateNodes);
        EventManager.TriggerEvent(Events.PlatformsInPlace);
    }

}
