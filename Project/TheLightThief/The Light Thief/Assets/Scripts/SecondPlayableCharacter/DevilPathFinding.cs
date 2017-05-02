using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilPathFinding : MonoSingleton<DevilPathFinding>
{
    private Vector3 startingPoint; //Holds the position of the closest node to the player

    private float playerRotation;
    private Vector3 playerPosition;

    private Vector3 clickPoint;
    private Transform endNode;

    public bool CheckIfPathIsValid()
    {
        //Update Positioning Values
        playerRotation = Utilities.GetObjectZWorldRotation(this.transform);
        playerPosition = this.transform.position;

        FindClosestNode();

        if (!IsMouseClickOnSameAxisAsPlayer(clickPoint))
        {
            Debug.Log("Mouse Click Not on Same Axis");
            return false;
        }

        if(playerRotation == 0 || playerRotation == 180)
        {
            int numToAddOn = 0;

            //Check our Travelling Direction
            if (clickPoint.x > playerPosition.x)
            {
                numToAddOn = 1;
            }
            else
            {
                numToAddOn = -1;
            }

            //Work Out Difference Between Start and End Node
            if (endNode == null)
                return false;

            int difference = (int)Mathf.Abs(startingPoint.x - endNode.position.x);
            Vector3 startPoint = startingPoint;

            for(int i = 0; i < difference; i++)
            {
                startPoint = new Vector3(startPoint.x + numToAddOn,
                                            startPoint.y,
                                            startPoint.z);

                //Check if that node exists
                if(!NodeManager.Nodes.Contains(startPoint))
                {
                    //Kill PC Movement
                    return false;
               
                }
            }

            return true;
        }
        else
        {
            int numToAddOn = 0;
            if(clickPoint.y > playerPosition.y)
            {
                numToAddOn = 1;
            }
            else
            {
                numToAddOn = -1;
            }
            int difference = (int)Mathf.Abs(startingPoint.y - endNode.position.y);

            Vector3 startPoint = startingPoint;

            for(int i = 0; i < difference; i++)
            {
                startPoint = new Vector3(startingPoint.x,
                                         startingPoint.y + numToAddOn,
                                         startingPoint.z);

                //Check if that node exists
                if (!NodeManager.Nodes.Contains(startPoint))
                {
                    //Kill PC Movement
                    return false;

                }
            }

            return true;

        }
    }

    private bool IsMouseClickOnSameAxisAsPlayer(Vector3 mouseClick)
    {
        float playerPos = 0;
        float targetPos = 0;

        if (playerRotation == 0 || playerRotation == 180)
        {
            playerPos = playerPosition.y;
            targetPos = mouseClick.y;

            if(playerPos <= targetPos + 1.0f && playerPos >= targetPos - 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            playerPos = playerPosition.x;
            targetPos = mouseClick.x;

            if(playerPos <= targetPos + 1.0f && playerPos >= targetPos - 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void CacheClickPointAndTargetPosition(Vector3 _clickPoint, Transform _endNode)
    {
        clickPoint = _clickPoint;
        endNode = _endNode;
    }

    private void FindClosestNode()
    {
        playerPosition = this.transform.position;
        float x = Mathf.Round(playerPosition.x);
        float y = Mathf.Round(playerPosition.y);

        float dist = 0;
        float playerRot = Mathf.Abs(Mathf.Round(transform.eulerAngles.z));

        for(int i = 0; i < NodeManager.Nodes.Count; i++)
        {
            float newDist = Vector3.Distance(this.transform.position, NodeManager.Nodes[i]);

            if(newDist < dist || dist == 0)
            {
                if (playerRot == 0)
                {
                    if (y > NodeManager.Nodes[i].y)
                    {
                        dist = newDist;
                        startingPoint = NodeManager.Nodes[i];
                    }
                }
                else if (playerRot == 180)
                {
                    if (y < NodeManager.Nodes[i].y)
                    {
                        dist = newDist;
                        startingPoint = NodeManager.Nodes[i];
                    }
                }
                else if (playerRot == 90)
                {
                    if (x < NodeManager.Nodes[i].x)
                    {
                        dist = newDist;
                        startingPoint = NodeManager.Nodes[i];
                    }
                }
                else if (playerRot == 270)
                {
                    if (x > NodeManager.Nodes[i].x)
                    {
                        dist = newDist;
                        startingPoint = NodeManager.Nodes[i];
                    }
                }
            }
        }
    }
	
}
