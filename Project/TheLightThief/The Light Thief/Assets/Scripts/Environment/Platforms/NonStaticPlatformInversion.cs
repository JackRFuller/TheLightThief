using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticPlatformInversion : Invertable
{
    private Collider platformCollider;
    private List<GameObject> platformObjs;

    [SerializeField]
    private StartingState startingState;
    private enum StartingState
    {
        On,
        Off,
    }

    public void LateStart()
    {
        platformCollider = this.GetComponent<Collider>();
        platformObjs = new List<GameObject>();

        for(int i = 0; i < transform.childCount; i++)
        {
            platformObjs.Add(this.transform.GetChild(i).gameObject);
        }

        if (startingState == StartingState.Off)
        {
            TurnObjOff();
        }

        EventManager.TriggerEvent(Events.RecalibrateNodes);
    }

    protected override void Invert()
    {
        switch(startingState)
        {
            case StartingState.On:
                TurnObjOff();
                break;
            case StartingState.Off:
                TurnObjOn();
                break;
        }
    }

    private void TurnObjOff()
    {
        platformCollider.enabled = false;
        for(int i = 0; i < platformObjs.Count; i++)
        {
            platformObjs[i].SetActive(false);
        }

        startingState = StartingState.Off;
    }

    private void TurnObjOn()
    {
        platformCollider.enabled = false;
        for (int i = 0; i < platformObjs.Count; i++)
        {
            platformObjs[i].SetActive(false);
        }

        startingState = StartingState.On;
    }



}
