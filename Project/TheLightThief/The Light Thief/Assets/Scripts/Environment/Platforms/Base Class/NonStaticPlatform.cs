using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticPlatform : MonoBehaviour
{
    protected Collider platformCollider;
    protected AudioSource platformAudio;
    protected PCMovementController pcMovementController;
    protected bool hasPlayerOnPlatform = false;

    protected virtual void Start()
    {
        //Get Components
        platformAudio = this.GetComponent<AudioSource>();
        platformCollider = this.GetComponent<Collider>();
    }

    protected void EnablePlayerInput()
    {
        EventManager.TriggerEvent(Events.EnablePlayerMovement);
    }

    protected void DisablePlayerInput()
    {
        EventManager.TriggerEvent(Events.DisablePlayerMovement);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("Parented " + this.gameObject.name);
            //other.transform.parent = this.transform;

            if (pcMovementController == null)
            {
                pcMovementController = other.GetComponent<PCMovementController>();
            }

            hasPlayerOnPlatform = true;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("UnParented " + this.gameObject.name);
            //other.transform.parent = null;
            hasPlayerOnPlatform = false;            
        }
    }
}
