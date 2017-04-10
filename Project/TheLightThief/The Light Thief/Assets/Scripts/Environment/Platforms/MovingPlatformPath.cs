using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPath : MonoBehaviour
{
    private MovingPlatformHandler movingPlatform;

    [Header("Path Points")]
    [SerializeField]
    private Transform pointOne;
    [SerializeField]
    private Transform pointTwo;
    [SerializeField]
    private Transform path;

    private float platformRotation;

    public void SetupPath(MovingPlatformHandler _platform)
    {
        movingPlatform = _platform;

        //Get Platform Rotation
        platformRotation = Utilities.GetObjectZWorldRotation(movingPlatform.transform);

        //Set First Point Position
        Vector3 centerPoint = movingPlatform.GetComponent<Collider>().bounds.center;

        Vector3 firstPoint = new Vector3(centerPoint.x,
                                         centerPoint.y,
                                         1.0f);

        pointOne.position = firstPoint;

        Vector3 secondPoint = Vector3.zero;

        //Check Platform Rotation
        if(platformRotation == 0 || platformRotation == 180)
        {
            secondPoint = new Vector3(centerPoint.x,
                                      movingPlatform.PositionTwo.y,
                                      1);
        }
        else
        {
            secondPoint = new Vector3(movingPlatform.PositionTwo.x,
                                      centerPoint.y,
                                      1);
        }

        pointTwo.position = secondPoint;

        //Set Path to be in the middle
        path.position = (firstPoint + secondPoint) * 0.5f;

        //Work Out Scale
        Vector3 pathScale = Vector3.zero;
        float pathDiff = 0;

        if (platformRotation == 0 || platformRotation == 180)
        {
            pathDiff = pointOne.position.y - pointTwo.position.y;
            pathDiff *= 100.0f;

            pathScale = new Vector3(path.localScale.x,
                                    pathDiff,
                                    1);
        }
        else
        {
            pathDiff = pointOne.position.x - pointTwo.position.x;
            pathDiff *= 100.0f;

            pathScale = new Vector3(pathDiff,
                                    path.localScale.y,
                                    1);
        }

        path.localScale = pathScale;
    }
}
