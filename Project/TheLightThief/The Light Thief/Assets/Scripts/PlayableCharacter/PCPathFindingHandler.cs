using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPathFindingHandler : MonoSingleton<PCPathFindingHandler>
{
    //Components
    private PCMovementController pcMovement;

    private Vector3 startingNode; //Holds to closest node to the player
    private float playerRotation; //Holds the player's current Rotation
    private Vector3 playerPosition;

    private Vector3 clickPoint;
    private Transform endNode;

    private void Start()
    {
        pcMovement = this.GetComponent<PCMovementController>();
        playerRotation = Utilities.GetObjectZWorldRotation(this.transform);
        playerPosition = this.transform.position;

        FindClosestNode();
    }

    private void FindClosestNode()
    {
        Vector3 startingPos = this.transform.position;

        float dist = 0;

        float playerRot = Mathf.Abs(Mathf.Round(transform.eulerAngles.z));

        for (int i = 0; i < NodeManager.Nodes.Count; i++)
        {
            float newDist = Vector3.Distance(this.transform.position, NodeManager.Nodes[i]);

            if (newDist < dist || dist == 0)
            {
                if(playerRot == 0)
                {
                    if(Mathf.Round(playerPosition.y) > NodeManager.Nodes[i].y)
                    {
                        dist = newDist;
                        startingNode = NodeManager.Nodes[i];
                    }
                }
                else if(playerRot == 180)
                {
                    if (Mathf.Round(playerPosition.y) < NodeManager.Nodes[i].y)
                    {
                        dist = newDist;
                        startingNode = NodeManager.Nodes[i];
                    }
                }
                else if(playerRot == 90)
                {
                    if (Mathf.Round(playerPosition.x) < NodeManager.Nodes[i].x)
                    {
                        dist = newDist;
                        startingNode = NodeManager.Nodes[i];
                    }
                }
                else if (playerRot == 270)
                {
                    if (Mathf.Round(playerPosition.x) > NodeManager.Nodes[i].x)
                    {
                        dist = newDist;
                        startingNode = NodeManager.Nodes[i];
                    }
                }
            }
        }
    }


    public void CacheClickPointAndTargetPosition(Vector3 _clickPoint, Transform _endNode)
    {
        clickPoint = _clickPoint;
        endNode = _endNode;
    }

    /// <summary>
    /// Takes in Input from when the mouse clicks on a node
    /// </summary>
    /// <param name="clickPoint"></param>
    /// <param name="endNode"></param>
    public bool CheckIfPathIsValid()
    {
        //Need Orientation of the player
        playerRotation = Utilities.GetObjectZWorldRotation(this.transform);       
        playerPosition = this.transform.position;       

        FindClosestNode();
        
        if (!IsMouseClickOnSameAxisAsPlayer(clickPoint))
            return false;

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
            }
            else //We're Travelling Left
            {
                numToAddOn = -1;              
            }

            //Work Out Difference Between Start Node and End Node
            if (endNode == null)
            {
                return false;
            }                

            int difference = (int)Mathf.Abs(startingNode.x - endNode.position.x);           

            Vector3 startingPoint = startingNode;

            for(int i = 0; i < difference; i++)
            {
                startingPoint = new Vector3(startingPoint.x + numToAddOn,
                                            startingPoint.y,
                                            startingPoint.z);
                               

                //Check if that node exists
                if(!NodeManager.Nodes.Contains(startingPoint))
                {
                    pcMovement.KillPlayerMovement();
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
            }
            else //We're Travelling Left
            {
                numToAddOn = -1;                
            }

            //Work Out Difference Between Start Node and End Node
            int difference = (int)Mathf.Abs(startingNode.y - endNode.position.y);         

            Vector3 startingPoint = startingNode;            

            for (int i = 0; i < difference; i++)
            {
                startingPoint = new Vector3(startingPoint.x,
                                            startingPoint.y + numToAddOn,
                                            startingPoint.z);
                //Check if that node exists
                if (!NodeManager.Nodes.Contains(startingPoint))
                {
                    pcMovement.KillPlayerMovement();
                    return false;
                }                
            }
            return true;
        }
    }

    private bool IsMouseClickOnSameAxisAsPlayer(Vector3 mouseClick)
    {
        float player = 0;
        float target = 0;

        if(playerRotation == 0 || playerRotation == 180)
        {
            player = playerPosition.y;
            target = mouseClick.y;

            if (player <= target + 1.0f && player >= target - 1.0f)
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
            player = playerPosition.x;
            target = mouseClick.x;

            if (player <= target + 1.0f && player >= target - 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }



}
