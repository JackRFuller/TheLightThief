using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static float GetObjectZWorldRotation(Transform objectRot)
    {
        float rotation = Mathf.Abs(Quaternion.Angle(objectRot.rotation, Quaternion.identity));
        rotation = Mathf.Round(rotation);
        return rotation;
    }
}
