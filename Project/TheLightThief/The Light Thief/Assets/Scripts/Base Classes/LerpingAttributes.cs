using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LerpingAttributes
{
    public Vector3 pointOne;
    public Vector3 pointTwo;
    public float lerpSpeed;
    public AnimationCurve lerpCurve;	
}

[System.Serializable]
public class MathfLerpingAttributes
{
    public float pointOne;
    public float pointTwo;
    public float lerpSpeed;
    public AnimationCurve lerpCurve;
}
