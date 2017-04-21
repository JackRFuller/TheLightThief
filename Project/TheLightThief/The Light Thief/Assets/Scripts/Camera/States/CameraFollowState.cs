using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowState : ICameraState
{
    private readonly CameraStateController camera;
    public CameraFollowState(CameraStateController cameraStateController)
    {
        camera = cameraStateController;
    }

    public FocusArea focusArea;

    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;

    public struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x * 0.5f;
            right = targetBounds.center.x + size.x * 0.5f;

            bottom = targetBounds.center.y - size.y * 0.5f;
            top = targetBounds.center.y + size.y * 0.5f;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            centre = new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f);
            velocity = new Vector2(shiftX, shiftY);
        }

    }   

    public void OnEnterState()
    {
        focusArea = new FocusArea(camera.Target.bounds, camera.FocusAreaSize);
        //Vector2 focusPosition = focusArea.centre;
        //camera.transform.position = (Vector3)focusPosition + Vector3.forward * -10.0f;
    }

    public void OnUpdateState()
    {

    }

    public void OnLateUpdateState()
    {
        focusArea.Update(camera.Target.bounds);
        Vector2 focusPosition = focusArea.centre;

        if(focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sin(focusArea.velocity.x);
        }

       // //targetLookAheadX = lookAheadDirX * camera.LookAheadDistX;
        //currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, camera.LookSmoothTimeX);

       // focusPosition += Vector2.right * currentLookAheadX;
        camera.transform.position = (Vector3)focusPosition + Vector3.forward * -10.0f;
    }    

    public void OnExitState(ICameraState newState)
    {

    }

}
