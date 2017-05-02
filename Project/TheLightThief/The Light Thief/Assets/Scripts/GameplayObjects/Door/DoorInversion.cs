using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInversion : Invertable
{
    private DoorHandler doorHandler;
    private Collider doorCollider;
    private List<GameObject> objs;

    [SerializeField]
    private StartingState startingState;
    private enum StartingState
    {
        On,
        Off,
    }

    private void Start()
    {
        doorHandler = this.GetComponent<DoorHandler>();
        doorCollider = this.GetComponent<Collider>();

        for(int i = 0; i < this.transform.childCount; i++)
        {
            objs.Add(this.transform.GetChild(i).gameObject);
        }
    }

    protected override void Invert()
    {
       switch(startingState)
        {
            case StartingState.On:
                TurnOff();
                break;
            case StartingState.Off:
                TurnOn();
                break;
        }
    }

    private void TurnOff()
    {
        for(int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(false);
        }

        doorCollider.enabled = false;
        startingState = StartingState.Off;
    }

    private void TurnOn()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(true);
        }

        if (doorHandler.IsActivated)
            doorCollider.enabled = true;
        else
            doorCollider.enabled = false;

        startingState = StartingState.On;
    }
}
