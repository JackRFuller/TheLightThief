using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoSingleton<NodeManager>
{
    private static List<Vector3> nodes = new List<Vector3>();
    public static List<Vector3> Nodes { get { return nodes; } }

    public List<Vector3> debugNodes = new List<Vector3>();

    private void Awake()
    {
        GetNodes();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.RecalibrateNodes, GetNodes);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.RecalibrateNodes, GetNodes);
    }

    private void GetNodes()
    {
        nodes.Clear();

        NodeHandler[] tempNodes = FindObjectsOfType<NodeHandler>();

        for (int i = 0; i < tempNodes.Length; i++)
        {
            //Round Node Position to avoid any floating point errors
            Vector3 nodePos = new Vector3(Mathf.Round(tempNodes[i].transform.position.x), 
                                                      Mathf.Round(tempNodes[i].transform.position.y),
                                                      0);            

            nodes.Add(nodePos);
        }
        
        debugNodes = nodes;
    }

}
