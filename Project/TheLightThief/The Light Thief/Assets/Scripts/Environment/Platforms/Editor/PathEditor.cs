using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatformPath))]
[CanEditMultipleObjects]
public class PathEditor : Editor
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
        serializedObject.Update();

        DrawDefaultInspector();

        MovingPlatformPath script = (MovingPlatformPath)target;

        if(GUILayout.Button("Set First Position"))
        {
            positionOne.vector3Value = script.transform.position;
        }

        if(GUILayout.Button("Set Second Position"))
        {
            positionTwo.vector3Value = script.transform.position;
        }

        if(GUILayout.Button("Create Path"))
        {
            script.CreatePath();
        }


        serializedObject.ApplyModifiedProperties();
    }



}
