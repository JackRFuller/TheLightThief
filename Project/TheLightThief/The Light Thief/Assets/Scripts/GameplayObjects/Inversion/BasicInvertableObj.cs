using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInvertableObj : Invertable
{
    [SerializeField]
    private GameObject objMesh;
    private Collider objCollider;

    [SerializeField]
    private StartingState startingState;
    private enum StartingState
    {
        On,
        Off,
    }

    private void Start()
    {
        objCollider = this.GetComponent<Collider>();

        if(startingState == StartingState.Off)
        {
            TurnOffObject();
        }
    }

    protected override void Invert()
    {
        base.Invert();

        switch(startingState)
        {
            case StartingState.On:
                TurnOffObject();
                break;
            case StartingState.Off:
                TurnOnObject();
                break;
        }
    }

    private void TurnOffObject()
    {
        objCollider.enabled = false;
        objMesh.SetActive(false);

        startingState = StartingState.Off;
    }

    private void TurnOnObject()
    {
        objCollider.enabled = true;
        objMesh.SetActive(true);

        startingState = StartingState.On;
    }
}
