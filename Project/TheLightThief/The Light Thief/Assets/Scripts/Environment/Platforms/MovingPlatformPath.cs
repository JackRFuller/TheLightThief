using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPath : MonoBehaviour
{
    [Header("Path Points")]
    [SerializeField]
    private Transform pointOne;
    [SerializeField]
    private Transform pointTwo;
    [SerializeField]
    private Transform path;
    private float pathRotation;

    [Header("Positions")]
    [SerializeField]
    private Vector3 positionOne;
    [SerializeField]
    private Vector3 positionTwo;
    private int positionIndex;

    //Moving Platform
    [Header("Moving Platform")]
    [SerializeField]
    private MovingPlatformHandler movingPlatform;
    [HideInInspector]
    public bool IsMoving;

    private void OnEnable()
    {
        EventManager.StartListening(Events.PlatformsInPlace, FindConnectedPlatform);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PlatformsInPlace, FindConnectedPlatform);
    }

    private void Start()
    {
        FindConnectedPlatform();
    }

    private void FindConnectedPlatform()
    {
        Ray ray = new Ray(pointOne.position, -transform.forward); ;
        RaycastHit hit;

        Debug.Log("Searching For Moving Platforms");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.tag.Equals("Node"))
            {
                movingPlatform = hit.transform.parent.GetComponent<MovingPlatformHandler>();
                movingPlatform.path = this;
                positionIndex = 0;
                return;
            }
        }

        Ray rayTwo = new Ray(pointTwo.position, -transform.forward); 

        if (Physics.Raycast(rayTwo, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag.Equals("Node"))
            {
                Debug.Log("Found Node");
                movingPlatform = hit.transform.parent.GetComponent<MovingPlatformHandler>();
                movingPlatform.path = this;
                positionIndex = 1;
                return;
            }
        }
        
    }

    public void ActivateMovingPlatform()
    {
        if(movingPlatform)
        {
            if(positionIndex == 0)
            {
                movingPlatform.StartMoving(pointOne.position, pointTwo.position);
            }
            else
            {
                movingPlatform.StartMoving(pointTwo.position, pointOne.position);
            }

            positionIndex++;
            if (positionIndex > 1)
                positionIndex = 0;
        }
    } 
    
    public void DisconnectPlatform()
    {
        movingPlatform = null;
        Debug.Log("removed platform");
    } 

    public void CreatePath()
    {
        //Get Platform Rotation
        pathRotation = Utilities.GetObjectZWorldRotation(this.transform);
        

        Vector3 firstPoint = new Vector3(positionOne.x,
                                         positionOne.y,
                                         1.0f);

        pointOne.position = firstPoint;

        Vector3 secondPoint = Vector3.zero;

        secondPoint = new Vector3(positionTwo.x,
                                      positionTwo.y,
                                      1.0f);

        pointTwo.position = secondPoint;

        //Set Path to be in the middle
        path.position = (firstPoint + secondPoint) * 0.5f;

        //Work Out Scale
        Vector3 pathScale = Vector3.zero;
        float pathDiff = 0;

        if (pathRotation == 0 || pathRotation == 180)
        {
            pathDiff = pointOne.position.y - pointTwo.position.y;            
        }
        else
        {
            pathDiff = pointOne.position.x - pointTwo.position.x;
            
        }
        pathDiff *= 100.0f;
        pathScale = new Vector3(5,
                                pathDiff,
                                1);

        path.localScale = pathScale;
    }
}
