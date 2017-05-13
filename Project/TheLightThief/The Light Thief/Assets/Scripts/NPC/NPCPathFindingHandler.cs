 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathFindingHandler : BaseMonoBehaviour
{
    //Components
    private NPCMovementHandler movementHandler;

    private float NPCrotation;
    private Vector3 NPCposition;
    private Vector3 startingNode;

    //Path Points
    private Vector3 pathPointOne;
    private Vector3 pathPointTwo;

    private void Start()
    {
        //Get Components
        movementHandler = this.GetComponent<NPCMovementHandler>();

        FindNPCPath();
    }

    private void FindClosestNode()
    {
        Vector3 startingPoint = this.transform.position;
        float dist = 0;

        float playerRot = Utilities.GetObjectZWorldRotation(this.transform);

        for(int i = 0; i < NodeManager.Nodes.Count; i++)
        {
            float newDist = Vector3.Distance(this.transform.position, NodeManager.Nodes[i]);

            if(newDist < dist || dist == 0)
            {
                dist = newDist;
                startingNode = NodeManager.Nodes[i];
            }
        }
    }

    public void FindNPCPath()
    {
        //Get Orientation of Player
        NPCrotation = Utilities.GetObjectZWorldRotation(this.transform);
        NPCposition = this.transform.position;
        FindClosestNode();

        //CHeck if we're going horizontally
        if(NPCrotation == 0 || NPCrotation == 180)
        {
            //Cycle Through Points To The Right
            Vector3 nextPoint = startingNode;
            for(int i = 0; i < NodeManager.Nodes.Count; i++)
            {
                //Calculate Next Point
                nextPoint = new Vector3(nextPoint.x + 1,
                                        nextPoint.y,
                                        nextPoint.z);
                if(NodeManager.Nodes.Contains(nextPoint))
                {
                    pathPointOne = new Vector3(nextPoint.x,                                                                          
                                               this.transform.position.y,
                                               this.transform.position.z);
                }
                else
                {
                    break;
                }
            }

            //Cycle Through Points to the Left
            nextPoint = startingNode;
            for (int i = 0; i < NodeManager.Nodes.Count; i++)
            {
                //Calculate Next Point
                nextPoint = new Vector3(nextPoint.x - 1,
                                        nextPoint.y,
                                        nextPoint.z);
                if (NodeManager.Nodes.Contains(nextPoint))
                {
                    pathPointTwo = new Vector3(nextPoint.x,
                                               this.transform.position.y,
                                               this.transform.position.z);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            //Cycle Through Points Up
            Vector3 nextPoint = startingNode;
            for (int i = 0; i < NodeManager.Nodes.Count; i++)
            {
                //Calculate Next Point
                nextPoint = new Vector3(nextPoint.x,
                                        nextPoint.y + 1,
                                        nextPoint.z);

                Debug.Log("Searching For " + nextPoint);

                if (NodeManager.Nodes.Contains(nextPoint))
                {
                    pathPointOne = new Vector3(this.transform.position.x,
                                               nextPoint.y,
                                               this.transform.position.z);

                    Debug.Log("Found point " + nextPoint);
                }
                else
                {
                    break;
                }
            }

            //Cycle Through Points Down
            nextPoint = startingNode;

            for (int i = 0; i < NodeManager.Nodes.Count; i++)
            {
                //Calculate Next Point
                nextPoint = new Vector3(nextPoint.x,
                                        nextPoint.y - 1,
                                        nextPoint.z);

                Debug.Log("Searching For " + nextPoint);

                if (NodeManager.Nodes.Contains(nextPoint))
                {
                    pathPointTwo = new Vector3(this.transform.position.x,
                                               nextPoint.y,
                                               this.transform.position.z);
                }
                else
                {
                    break;
                }
            }
            Debug.Log(pathPointOne);
            Debug.Log(pathPointTwo);
        }

        //Start Moving
        movementHandler.StartMoving(pathPointOne, pathPointTwo);

    }
	
}
