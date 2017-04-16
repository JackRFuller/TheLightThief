using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireHandler : MonoBehaviour
{
    [SerializeField]
    private Transform wireWaypointHolder;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject wireConnector;
    [SerializeField]
    private GameObject wire;
    [SerializeField]
    private GameObject wireConnectorBG;
    [SerializeField]
    private GameObject wireBG;
    private GameObject wireBGHolder;
    [SerializeField]
    private GameObject wireHolder;

    [Header("Wire Elements")]
    [SerializeField]
    private float fillSpeed;

    private List<Transform> waypoints = new List<Transform>();

    public List<Image> wireImages;
    private bool cycleThroughWires;
    private int wireIndex = 0;
    private SwitchHandler associatedSwitch; //Used to trigger switch

    private void Start()
    {
        wireImages = new List<Image>();

        //Add Wires to List
        foreach(RectTransform child in wireHolder.transform)
        {
            wireImages.Add(child.GetComponent<Image>());
        }
    }

    public void TriggerWires(SwitchHandler trigger)
    {
        associatedSwitch = trigger;
        cycleThroughWires = true;    
    }

    private void Update()
    {
        if(cycleThroughWires)
        {
            ShowWires();
        }
    }

    private void ShowWires()
    {
        if(wireImages[wireIndex].fillAmount < 1)
        {
            wireImages[wireIndex].fillAmount += fillSpeed * Time.deltaTime;

            if(wireImages[wireIndex].fillAmount >= 1.0f)
            {
                if (wireIndex < wireImages.Count - 1)
                {
                    wireIndex++;
                }
                else
                {
                    cycleThroughWires = false;
                    associatedSwitch.IncremementTrigger();
                }
            }
        }
    }

    public void SetupWireHolder(GameObject _wire)
    {
        wireHolder = _wire;
        wireHolder.name = "Wires";
        wireHolder.transform.parent = this.transform;
    }

    //Triggered From Editor Script
    public void CreateWiring()
    {
        //Create Wire Holders
        wireBGHolder = new GameObject();
        wireBGHolder.name = "Wire BG";
       
        wireBGHolder.transform.parent = this.transform;

        //Get Waypoints
        waypoints.Clear();
        if (wireImages == null)
            wireImages = new List<Image>();

        wireImages.Clear();

        wireHolder.transform.SetAsLastSibling();

        foreach(Transform child in wireWaypointHolder)
        {
            waypoints.Add(child);
        }

        for(int j = 0; j < 2; j++)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                if(j == 0)
                {
                    //Place Waypoints
                    GameObject newWireConnector = Instantiate(wireConnectorBG, waypoints[i].position, Quaternion.identity) as GameObject;

                    //Look At Last Wire
                    if (i > 0)
                    {
                        Vector3 difference = waypoints[i - 1].position - newWireConnector.transform.position;
                        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                        newWireConnector.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationZ - 90));
                    }

                    newWireConnector.transform.parent = wireBGHolder.transform;

                    if (i < waypoints.Count - 1)
                    {
                        //Spawn In Wires
                        GameObject newWire = Instantiate(wireBG) as GameObject;

                        //Position Wire
                        Vector3 newWirePos = (waypoints[i + 1].transform.position + waypoints[i].transform.position) * 0.5F;
                        newWire.transform.position = newWirePos;

                        //Rotate Wire
                        Vector3 difference = waypoints[i + 1].position - waypoints[i].position;
                        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                        newWire.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationZ));

                        //Update Wire Size
                        float xSize = Vector3.Distance(waypoints[i + 1].position, waypoints[i].position);

                        Vector3 newSize = new Vector3(xSize - 0.15f,
                                                      newWire.transform.localScale.y,
                                                      1);

                        newWire.transform.localScale = newSize;

                        newWire.transform.parent = wireBGHolder.transform;
                    }
                }
                else if(j == 1)
                {
                    //Place Waypoints
                    GameObject newWireConnector = Instantiate(wireConnector, waypoints[i].position, Quaternion.identity) as GameObject;

                    //Look At Last Wire
                    if (i > 0)
                    {
                        Vector3 difference = waypoints[i - 1].position - newWireConnector.transform.position;
                        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                        newWireConnector.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationZ - 90));
                    }

                    newWireConnector.transform.parent = wireHolder.transform;                    

                    if (i < waypoints.Count - 1)
                    {
                        //Spawn In Wires
                        GameObject newWire = Instantiate(wire) as GameObject;

                        //Position Wire
                        Vector3 newWirePos = (waypoints[i + 1].transform.position + waypoints[i].transform.position) * 0.5F;
                        newWire.transform.position = newWirePos;

                        //Rotate Wire
                        Vector3 difference = waypoints[i + 1].position - waypoints[i].position;
                        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                        newWire.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationZ));

                        //Update Wire Size
                        float xSize = Vector3.Distance(waypoints[i + 1].position, waypoints[i].position);

                        Vector3 newSize = new Vector3(xSize,
                                                      newWire.transform.localScale.y,
                                                      1);

                        newWire.transform.localScale = newSize;

                        newWire.transform.parent = wireHolder.transform;
                    }
                }
            }
        }

        
    }
}
