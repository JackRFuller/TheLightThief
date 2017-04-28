using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaypointInversion : Invertable
{
    [Header("Components")]
    [SerializeField]
    private MeshRenderer waypointMesh;
    private Camera mainCamera;

    [Header("Materials")]
    [SerializeField]
    private Material whiteMat;
    [SerializeField]
    private Material blackMat;

    private bool isWhite;

    private void Start()
    {
        mainCamera = Camera.main;

        waypointMesh.material = whiteMat;
        isWhite = true;
    }

    protected override void Invert()
    {
        if (isWhite)
        {
            waypointMesh.material = blackMat;
            isWhite = false;
        }
        else
        {
            waypointMesh.material = whiteMat;
            isWhite = true;
        }
    }
}
