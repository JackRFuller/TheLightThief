using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoSingleton<NodeManager>
{
    private static List<Vector3> nodes = new List<Vector3>();
    public static List<Vector3> Nodes { get { return nodes; } }

    private void Awake()
    {
        GetNodes();
    }

    private void GetNodes()
    {
        nodes.Clear();

        NodeHandler[] tempNodes = FindObjectsOfType<NodeHandler>();

        for (int i = 0; i < tempNodes.Length; i++)
        {
            //Round and Reposition Nodes
            Vector3 newNodePos = new Vector3(Mathf.Round(tempNodes[i].transform.position.x),
                                             Mathf.Round(tempNodes[i].transform.position.y),
                                             Mathf.Round(tempNodes[i].transform.position.z));

            tempNodes[i].transform.position = newNodePos;

            nodes.Add(tempNodes[i].transform.position);
        }
    }

}
