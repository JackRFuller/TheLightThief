using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invertable : BaseMonoBehaviour
{
    protected virtual void OnEnable()
    {
        EventManager.StartListening(Events.Invert, Invert);
    }

    protected virtual void OnDisable()
    {
        EventManager.StopListening(Events.Invert, Invert);
    }

    protected virtual void Invert()
    {

    }
	
}
