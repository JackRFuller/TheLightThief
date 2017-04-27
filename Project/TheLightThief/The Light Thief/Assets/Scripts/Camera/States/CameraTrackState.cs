using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackState : ICameraState
{
    private readonly CameraStateController camera;
    public CameraTrackState(CameraStateController cameraStateController)
    {
        camera = cameraStateController;
    }
    
    private Vector3 originalPosition;

    public void OnEnterState()
    {
        //originalPosition = camera.transform.position;
        //camera.transform.position = originalPosition;
    }

    public void OnUpdateState()
    {
        //camera.transform.position = originalPosition;
    }

    public void OnExitState(ICameraState newState)
    {

    }
}
