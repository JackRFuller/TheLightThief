using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireInversion : Invertable
{
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
        objs = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++)
        {
            objs.Add(transform.GetChild(i).gameObject);
        }

        if(startingState == StartingState.Off)
        {
            TurnOff();
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

        startingState = StartingState.Off;
    }

    private void TurnOn()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(true);
        }

        startingState = StartingState.On;
    }
}
