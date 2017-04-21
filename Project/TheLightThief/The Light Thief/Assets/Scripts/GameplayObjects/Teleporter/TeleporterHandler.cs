using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterHandler : BaseMonoBehaviour
{
    [SerializeField]
    private TeleporterHandler associatedTeleport;    
    public TeleportType teleporterType;
   

    [HideInInspector]
    public bool hasJustBeenActivated; //Controls whether the teleport is able to send the player to designated position

    private void Start()
    {
        //Make Z Rotation Positive
        float zRot = Utilities.GetObjectZWorldRotation(this.transform);
        Debug.Log(zRot);
        Vector3 newRot = new Vector3(0, 0, zRot);

        transform.rotation = Quaternion.Euler(newRot);
    }

    public enum TeleportType
    {
        Static,
        Dynamic,
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if (associatedTeleport)
            {
                if(!hasJustBeenActivated)
                {
                    //Get Teleport Position
                    Vector3 newPlayerPos = associatedTeleport.transform.position;

                    //Get Target Rotation
                    Quaternion targetRot = Quaternion.identity;

                    if(associatedTeleport.teleporterType == TeleportType.Static)
                    {
                        targetRot = associatedTeleport.transform.rotation;
                        other.transform.parent = null;
                    }
                    else
                    {
                        //Get zRot of parent
                        Transform parentPlatform = associatedTeleport.transform.parent;
                        other.transform.parent = parentPlatform;
                    }

                    targetRot = associatedTeleport.transform.rotation;

                    associatedTeleport.hasJustBeenActivated = true;

                    
                    other.transform.rotation = targetRot;
                    other.transform.position = newPlayerPos;
                }                
            }
            else
            {
                Debug.LogError("No Associated Teleport");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {        
        if (other.tag.Equals("Player"))
        {
            hasJustBeenActivated = false;
        }
    }
}
