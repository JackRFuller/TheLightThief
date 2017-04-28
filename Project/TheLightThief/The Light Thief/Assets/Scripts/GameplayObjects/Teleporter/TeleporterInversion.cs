using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterInversion : Invertable
{
    //Components
    [SerializeField]
    private GameObject teleporterObj;
    private Collider teleportCollider;

    [SerializeField]
    private StartingState startingState;
    private enum StartingState
    {
        On,
        Off,
    }

    private void Start()
    {
        teleportCollider = this.GetComponent<Collider>();

        if(startingState == StartingState.Off)
        {
            TurnTeleporterOff();
        }
    }

    protected override void Invert()
    {
        base.Invert();
        switch(startingState)
        {
            //Turn Off
            case StartingState.On:
                TurnTeleporterOff();
                break;
            case StartingState.Off:
                TurnTeleporterOn();
                break;
        }
    }

    private void TurnTeleporterOff()
    {
        teleportCollider.enabled = false;
        teleporterObj.SetActive(false);
        startingState = StartingState.Off;
    }

    private void TurnTeleporterOn()
    {
        teleportCollider.enabled = true;
        teleporterObj.SetActive(true);
        startingState = StartingState.On;
    }
}
