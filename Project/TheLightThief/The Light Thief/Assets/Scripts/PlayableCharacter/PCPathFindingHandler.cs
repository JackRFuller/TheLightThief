using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPathFindingHandler : MonoSingleton<PCPathFindingHandler>
{
    private Vector3 startingNode; //Holds to closest node to the player
    private float playerRotation; //Holds the player's current Rotation
    private Vector3 playerPosition;

    private void Start()
    {
        FindClosestNode();
    }

    private void FindClosestNode()
    {
        Vector3 startingPos = this.transform.position;

        float dist = 0;

        for (int i = 0; i < NodeManager.Nodes.Count; i++)
        {
            float newDist = Vector3.Distance(this.transform.position, NodeManager.Nodes[i]);

            if (newDist < dist || i == 0)
            {
                dist = newDist;
                startingNode = NodeManager.Nodes[i];
            }
        }
    }

    /// <summary>
    /// Takes in Input from when the mouse clicks on a node
    /// </summary>
    /// <param name="clickPoint"></param>
    /// <param name="endNode"></param>
    public bool CheckIfPathIsValid(Vector3 clickPoint, Transform endNode)
    {
        //Need Orientation of the player
        playerRotation = transform.eulerAngles.z;
        Debug.Log(transform.eulerAngles);
        playerPosition = this.transform.position;

        //Check if click is on the same axis as the player
        if(!IsMouseClickOnSameAxisAsPlayer(clickPoint))
        {
            Debug.Log("Same Axis");
            return false;
        }
        else
        {
            //Check if there is an uninterrupted path to the point

            //Check what Direction We're Cycling
            if(playerRotation == 0 || playerRotation == 180)
            {
                //Controls which direction we're travelling in
                int numToAddOn = 0;

                //We're Travelling Right
                if(clickPoint.x > playerPosition.x)
                {
                    numToAddOn = 1;
                    Debug.Log("Travelling Right");
                }
                else //We're Travelling Left
                {
                    numToAddOn = -1;
                    Debug.Log("Travelling Left");
                }

                //Work Out Difference Between Start Node and End Node
                int difference = (int)Mathf.Abs(startingNode.x - endNode.position.x);
                Debug.Log("Difference To Travel " + difference);

                Vector3 startingPoint = startingNode;

                for(int i = 0; i < difference; i++)
                {
                    startingPoint = new Vector3(startingPoint.x + numToAddOn,
                                                startingPoint.y,
                                                startingPoint.z);

                    Debug.Log("Looking for " + startingPoint);

                    //Check if that node exists
                    if(NodeManager.Nodes.Contains(startingPoint))
                    {
                        Debug.Log("Found Node");
                    }
                    else
                    {
                        Debug.Log("No Node Found - False Path");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                int numToAddOn = 0;
                if (clickPoint.y > playerPosition.y)
                {
                    numToAddOn = 1;
                    Debug.Log("Travelling Up");
                }
                else //We're Travelling Left
                {
                    numToAddOn = -1;
                    Debug.Log("Travelling Down");
                }

                //Work Out Difference Between Start Node and End Node
                int difference = (int)Mathf.Abs(startingNode.y - endNode.position.y);
                Debug.Log("Difference " + difference);

                Vector3 startingPoint = startingNode;
                Debug.Log("Starting Point " + startingNode);

                for (int i = 0; i < difference; i++)
                {
                    startingPoint = new Vector3(startingPoint.x,
                                                startingPoint.y + numToAddOn,
                                                startingPoint.z);

                    Debug.Log("Looking for " + startingPoint);

                    //Check if that node exists
                    if (NodeManager.Nodes.Contains(startingPoint))
                    {
                        Debug.Log("Found Node");
                    }
                    else
                    {
                        Debug.Log("No Node Found - False Path");
                        return false;
                    }
                }
                return true;
            }
        }


    }

    private bool IsMouseClickOnSameAxisAsPlayer(Vector3 mouseClick)
    {
        if(playerRotation == 0 || playerRotation == 180)
        {
            //Find the difference between the height of the click and the player
            if(mouseClick.y < playerPosition.y + 0.5f && mouseClick.y > playerPosition.y - 0.5f)
            {
                return true;
            }
            
        }

        return true;
    }



}
