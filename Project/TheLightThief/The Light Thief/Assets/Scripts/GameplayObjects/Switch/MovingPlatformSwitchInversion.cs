using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSwitchInversion : Invertable
{
    private MovingPlatformSwitch movingSwitchHandler;
    private Collider switchCollider;
    [SerializeField]
    private MeshRenderer switchMesh;
    [SerializeField]
    private SpriteRenderer switchSprite;

    [SerializeField]
    private StartingState startingState;
    private enum StartingState
    {
        On,
        Off,
    }

    private void Start()
    {
        movingSwitchHandler = this.GetComponent<MovingPlatformSwitch>();
        switchCollider = this.GetComponent<Collider>();

        if(startingState == StartingState.Off)
        {
            TurnObjectOff();
        }
    }

    protected override void Invert()
    {
        switch(startingState)
        {
            case StartingState.On:
                TurnObjectOff();
                break;
            case StartingState.Off:
                TurnObjectOn();
                break;
        }
    }

    private void TurnObjectOff()
    {
        switchCollider.enabled = false;
        switchMesh.enabled = false;
        switchSprite.enabled = false;

        startingState = StartingState.Off;

    }

    private void TurnObjectOn()
    {
        switchMesh.enabled = true;
        switchSprite.enabled = true;

        if(movingSwitchHandler.IsActivated)
        {
            switchCollider.enabled = true;
        }
        else
        {
            switchCollider.enabled = false;
        }

        startingState = StartingState.On;
    }

}
