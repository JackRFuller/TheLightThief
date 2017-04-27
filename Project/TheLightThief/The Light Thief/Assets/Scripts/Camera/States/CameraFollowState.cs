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

    //Lerping Variables
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timeStartedLerping;
    private float lerpSpeed = 0.5f;
    private bool isLerping;

    private bool setupCam;
    private Vector3 velocity = Vector3.zero;

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
        //Init Lerp To Player's Center Point
        startPosition = camera.transform.position;
        targetPosition = new Vector3(camera.Target.transform.position.x, camera.Target.transform.position.y, -10.0f);

        timeStartedLerping = Time.time;
        //isLerping = true;

        if (!setupCam)
        {
            setupCam = true;
            focusArea = new FocusArea(camera.Target.bounds, camera.FocusAreaSize);
        }
    }

    public void OnUpdateState()
    {
        //if(isLerping)
        //{
        //    float timeSinceStarted = Time.time - timeStartedLerping;
        //    float percentageComplete = timeSinceStarted / lerpSpeed;

        //    Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, percentageComplete);

        //    camera.transform.position = newPos;

        //    if(percentageComplete >= 1.0f)
        //    {
        //        isLerping = false;
        //    }
        //}
       
    }

    public void OnLateUpdateState()
    {
        focusArea.Update(camera.Target.bounds);

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
        }

        Vector2 midPoint = ((Vector2)camera.transform.position - focusArea.centre) * 0.5f + focusArea.centre;

        Vector2 focusPosition = focusArea.centre + Vector2.up * camera.VerticalOffset;

       
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, camera.LookSmoothTimeX);

        //focusPosition.x = Mathf.SmoothDamp(camera.transform.position.x, focusPosition.x, ref smoothVelocityY, camera.VerticalSmoothTime);
        focusPosition.y = Mathf.SmoothDamp(camera.transform.position.y, focusPosition.y, ref smoothVelocityY, camera.VerticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;

        Vector3 newPos = (Vector3)focusPosition + Vector3.forward * -10.0f;

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, newPos, ref velocity, 0.2f);
    }    

    public void OnExitState(ICameraState newState)
    {

    }

}
