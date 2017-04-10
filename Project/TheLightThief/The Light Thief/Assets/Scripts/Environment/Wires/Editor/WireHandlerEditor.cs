using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WireHandler))]
[CanEditMultipleObjects]
public class WireHandlerEditor : Editor
{
    SerializedProperty wireHolder;

    public void OnEnable()
    {
        wireHolder = serializedObject.FindProperty("wireHolder");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        WireHandler script = (WireHandler)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Wires"))
        {
            if(wireHolder.objectReferenceValue != null)
            {
                DestroyImmediate(wireHolder.objectReferenceValue);
            }

            wireHolder.objectReferenceValue = new GameObject();            
            script.SetupWireHolder(wireHolder.objectReferenceValue as GameObject);
            script.CreateWiring();
            

        }

        serializedObject.ApplyModifiedProperties();
    }

}
