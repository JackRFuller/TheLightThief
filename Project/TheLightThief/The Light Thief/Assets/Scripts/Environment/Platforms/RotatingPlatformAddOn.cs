using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformAddOn : MonoBehaviour
{
    private BoxCollider platformCollider;
    private List<MovingPlatformHandler> movingPlatforms = new List<MovingPlatformHandler>();

    private int platformWidth = 1;

    private void OnEnable()
    {
        EventManager.StartListening(Events.PlatformsInPlace, FindConnectingPlatforms);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PlatformsInPlace, FindConnectingPlatforms);
    }

    private void Start()
    {
        platformCollider = this.GetComponent<BoxCollider>();

        FindConnectingPlatforms();
    }

    public void DisconnectMovingPlatformsFromPath()
    {
        for(int i = 0; i < movingPlatforms.Count; i++)
        {
            movingPlatforms[i].DisconnectFromPath();            
            Debug.Log("Start Disconnecting");
        }
    }

    private void FindConnectingPlatforms()
    {
        movingPlatforms.Clear();

        //Check Left Side
        Ray rayLeft = new Ray(this.transform.position, -transform.right);
        Debug.DrawRay(this.transform.position, -transform.right, Color.red, 10.0f);

        RaycastHit hit;
        if (Physics.Raycast(rayLeft, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag.Equals("Node"))
            {
                Transform platform = hit.transform.parent;
                if(platform.GetComponent<MovingPlatformHandler>())
                {
                    movingPlatforms.Add(platform.GetComponent<MovingPlatformHandler>());
                    //Set Parent
                    platform.parent = this.transform;

                    //CHange Collider so it's same size as the platform
                    platformWidth = platform.childCount + 1;

                    //Change COllider Centre Point
                    Vector3 centerPoint = new Vector3((platformWidth * -0.5f) + 0.5f, 0, 0);
                    platformCollider.center = centerPoint;

                    //CHange Collider Size
                    Vector3 colliderWidth = new Vector3(platformWidth * 0.5f, 1, 1);
                    platformCollider.extents = colliderWidth;
                }
            }
        }

        //Check Right Side
        Ray rightSide = new Ray(this.transform.position, transform.right);
        
        if (Physics.Raycast(rightSide, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag.Equals("Node"))
            {
                Transform platform = hit.transform.parent;
                if (platform.GetComponent<MovingPlatformHandler>())
                {
                    movingPlatforms.Add(platform.GetComponent<MovingPlatformHandler>());

                    //Set Parent
                    platform.parent = this.transform;

                    //Check How Many Platforms Are Attached
                    //If There is already a platform
                    if (movingPlatforms.Count == 2)
                    {
                        platformWidth += platform.childCount;

                        //Change COllider Centre Point                    
                        platformCollider.center = Vector3.zero;

                        //CHange Collider Size
                        Vector3 colliderWidth = new Vector3(platformWidth * 0.5f, 1, 1);
                        platformCollider.extents = colliderWidth;

                    }
                    else
                    {
                        //CHange Collider so it's same size as the platform
                        platformWidth = platform.childCount + 1;

                        //Change COllider Centre Point
                        Vector3 centerPoint = new Vector3((platformWidth * 0.5f) - 0.5f, 0, 0);
                        platformCollider.center = centerPoint;

                        //CHange Collider Size
                        Vector3 colliderWidth = new Vector3(platformWidth * 0.5f, 1, 1);
                        platformCollider.extents = colliderWidth;
                    }
                }
            }
        }
    }
}
