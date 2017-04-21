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

    //Lerping Variables
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeStartedLerping;
    private bool isLerping;    

    public void OnEnterState()
    {
        //Init Lerp To Player's Position
        startPosition = camera.transform.position;
        endPosition = new Vector3(camera.Target.transform.position.x,
                                  camera.Target.transform.position.y,
                                  -10.0f);

        timeStartedLerping = Time.time;
        isLerping = true;
        Debug.Log("Started Tracking");

    }

    public void OnUpdateState()
    {
        if (isLerping)
        {
            TrackPlayersPosition();
        }
        else
        {
            TrackPlayersPosition();
        }
            
    }

    private void LerpToPosition()
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / 0.5f;

        Vector3 newPos = Vector3.Lerp(startPosition, endPosition, percentageComplete);
        camera.transform.position = newPos;

        if(percentageComplete >= 1.0f)
        {
            isLerping = false;
        }
    }

    private void TrackPlayersPosition()
    {
        Vector3 trackingPosition = new Vector3(camera.Target.transform.position.x,
                                               camera.Target.transform.position.y,
                                               -10.0f);

        camera.transform.position = trackingPosition;
    }

    public void OnExitState(ICameraState newState)
    {

    }
}
