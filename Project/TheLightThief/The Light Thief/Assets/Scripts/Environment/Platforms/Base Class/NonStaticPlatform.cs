using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticPlatform : MonoBehaviour
{
    protected PCMovementController pcMovementController;
    protected bool hasPlayerOnPlatform = false;

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
            other.transform.parent = this.transform;

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
            other.transform.parent = null;
            hasPlayerOnPlatform = false;            
        }
    }
}
