using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchState : ICameraState
{
    //Lerping Values
    private Vector3 startingPoint;
    private Vector3 targetPoint;
    private const float lerpSpeed = 1.0f;
    private float timeStarted;

    private readonly CameraStateController camera;
    public CameraSwitchState(CameraStateController cameraStateController)
    {
        camera = cameraStateController;
    }

    public void OnEnterState()
    {
        startingPoint = camera.transform.position;
        targetPoint = new Vector3(camera.Target.transform.position.x,
                                  camera.Target.transform.position.y,
                                  -10.0f);

        timeStarted = Time.time;
    }

    public void OnUpdateState()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / lerpSpeed;

        Vector3 newPos = Vector3.Lerp(startingPoint, targetPoint, percentageComplete);
        camera.transform.position = newPos;

        if(percentageComplete >= 1.0f)
        {
            OnExitState(camera.followState);
        }
    }

    public void OnExitState(ICameraState newState)
    {
        camera.CurrentState = newState;
    }
}
