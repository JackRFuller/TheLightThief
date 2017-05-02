using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformSwitchInversion : Invertable
{
    private RotatingPlatformSwitch rotatingSwitchHandler;

    [SerializeField]
    private Collider[] switchColliders;
    [SerializeField]
    private MeshRenderer switchMesh;
    [SerializeField]
    private SpriteRenderer centerPoint;

    [SerializeField]
    private StartingState startingState;
    private enum StartingState
    {
        On,
        Off,
    }   

    private void Start()
    {
        rotatingSwitchHandler = this.GetComponent<RotatingPlatformSwitch>();

        if(startingState == StartingState.Off)
        {
            TurnOffSwitch();
        }
    }

    protected override void Invert()
    {
        switch(startingState)
        {
            case StartingState.On:
                TurnOffSwitch();
                break;
            case StartingState.Off:
                TurnOnObject();
                break;
        }
    }



    private void TurnOnObject()
    {
        switchMesh.enabled = true;
        centerPoint.enabled = true;
       
        for(int i = 0; i < switchColliders.Length; i++)
        {
            if(rotatingSwitchHandler.IsActivated)
            {
                switchColliders[i].enabled = true;
            }
            else
            {
                switchColliders[i].enabled = false;
            }            
        }

        startingState = StartingState.On;
    }

    private void TurnOffSwitch()
    {
        switchMesh.enabled = false;
        centerPoint.enabled = false;

        for (int i = 0; i < switchColliders.Length; i++)
        {
            switchColliders[i].enabled = false;
        }

        startingState = StartingState.Off;
    }


}
