using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatformHandler))]
[CanEditMultipleObjects]
public class MovingPlatformEditor : Editor
{
    SerializedProperty positionOne;
    SerializedProperty positionTwo;

    public void OnEnable()
    {
        positionOne = serializedObject.FindProperty("positionOne");
        positionTwo = serializedObject.FindProperty("positionTwo");
    }

    public override void OnInspectorGUI()
    {
        MovingPlatformHandler movingPlatform = (MovingPlatformHandler)target;

        serializedObject.Update();

        DrawDefaultInspector();

        if(GUILayout.Button("Set Position One"))
        {
            positionOne.vector3Value = movingPlatform.transform.position;
        }

        if (GUILayout.Button("Set Position Two"))
        {
            positionTwo.vector3Value = movingPlatform.transform.position;
        }

        if(GUILayout.Button("Set To Position One"))
        {
            movingPlatform.transform.position = positionOne.vector3Value;
        }

        if(GUILayout.Button("Set Path"))
        {
            movingPlatform.SetPath();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
