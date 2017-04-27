using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeState : ICameraState
{
    private CameraShakeProperties shakeProperties;
    private const float maxAngle = 10f;

    private readonly CameraStateController camera;
    public CameraShakeState(CameraStateController cameraStateController)
    {
        camera = cameraStateController;
    }

    public void OnEnterState()
    {
        shakeProperties = camera.ShakeProperties;
        Shake();
        Debug.Log("Started Shaking");
    }

    public void OnUpdateState()
    {

    }

    private void Shake()
    {
        float completionPercent = 0;
        float movePercent = 0;

        float angle_radians = shakeProperties.angle * Mathf.Deg2Rad - Mathf.PI;
        Vector3 previousWaypoint = Vector3.zero;
        Vector3 currentWaypoint = Vector3.zero;
        float moveDistance = 0;
        float speed = 0;

        Quaternion targetRotation = Quaternion.identity;
        Quaternion previousRotation = Quaternion.identity;

        do
        {
            if (movePercent >= 1 || completionPercent == 0)
            {
                float dampingFactor = DampingCurve(completionPercent, shakeProperties.dampingPercent);
                float noiseAngle = (Random.value - .5f) * Mathf.PI;
                angle_radians += Mathf.PI + noiseAngle * shakeProperties.noisePercent;
                currentWaypoint = new Vector3(Mathf.Cos(angle_radians), Mathf.Sin(angle_radians)) * shakeProperties.strength * dampingFactor;
                previousWaypoint = camera.transform.localPosition;
                moveDistance = Vector3.Distance(currentWaypoint, previousWaypoint);

                targetRotation = Quaternion.Euler(new Vector3(currentWaypoint.y, currentWaypoint.x).normalized * shakeProperties.rotationPercent * dampingFactor * maxAngle);
                previousRotation = camera.transform.localRotation;

                speed = Mathf.Lerp(shakeProperties.minSpeed, shakeProperties.maxSpeed, dampingFactor);

                movePercent = 0;
            }

            completionPercent += Time.deltaTime / shakeProperties.duration;
            movePercent += Time.deltaTime / moveDistance * speed;
            camera.transform.localPosition = Vector3.Lerp(previousWaypoint, currentWaypoint, movePercent);
            camera.transform.localRotation = Quaternion.Slerp(previousRotation, targetRotation, movePercent);            

        } while (moveDistance > 0);

        OnExitState(camera.followState);
    }

    float DampingCurve(float x, float dampingPercent)
    {
        x = Mathf.Clamp01(x);
        float a = Mathf.Lerp(2, .25f, dampingPercent);
        float b = 1 - Mathf.Pow(x, a);
        return b * b * b;
    }

    public void OnExitState(ICameraState newState)
    {
        camera.CurrentState = newState;
        Debug.Log("lEFT sHAKING sTATE");
    }
	
}
