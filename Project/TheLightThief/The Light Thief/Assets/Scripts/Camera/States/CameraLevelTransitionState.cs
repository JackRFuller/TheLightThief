using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLevelTransitionState : ICameraState
{
    private readonly CameraStateController camera;
    public CameraLevelTransitionState(CameraStateController cameraStateController)
    {
        camera = cameraStateController;
    }

    //Movement Variables
    private Vector3 startPos;
    private Vector3 targetPos;

    //Rotation Variables
   
    private Quaternion startRot;
    private Quaternion targetRot;

    //Lerp Variables
    private const float lerpingSpeed = 2.5f;
    private float timeStartedLerping;
    private bool isLerping;

    //Level Loading Variables
    private TransitionState transitionState;
    private bool hasLoadedLevel;

    private enum TransitionState
    {
        Moving,
        Rotating,
        Zooming,
    }

    public void OnEnterState()
    {
        startPos = camera.transform.position;
        startRot = camera.transform.rotation;

        Transform targetPoint = LevelHandler.Instance.NextLevelTarget;

        targetPos = new Vector3(targetPoint.position.x, targetPoint.position.y, -10.0f);
        targetRot = Quaternion.Euler(LevelHandler.Instance.CameraRotationTarget);

        timeStartedLerping = Time.time;
        hasLoadedLevel = false;
        isLerping = true;
    }

    public void OnUpdateState()
    {
        if(isLerping)
        {
            float timeSInceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSInceStarted / lerpingSpeed;

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, percentageComplete);
            Quaternion newRot = Quaternion.Lerp(startRot, targetRot, percentageComplete);

            camera.transform.position = newPos;
            camera.transform.rotation = newRot;

            if(percentageComplete >= 0.9f && !hasLoadedLevel)
            {
                EventManager.TriggerEvent(Events.WhiteFlash);
                LevelManager.Instance.LoadInNextLevel();
                hasLoadedLevel = true;
            }

            if(percentageComplete >= 1.0f)
            {
                isLerping = false;
            }
        }
    }

    public void OnExitState(ICameraState newState)
    {

    }

}
