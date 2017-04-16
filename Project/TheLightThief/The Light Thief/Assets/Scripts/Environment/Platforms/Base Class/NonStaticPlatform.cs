using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticPlatform : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField]
    protected Color startingColor;
    [SerializeField]
    protected Color targetColor;
    private Material newMaterial;

    protected MeshRenderer[] platformMeshes;
    protected Collider platformCollider;
    protected AudioSource platformAudio;
    protected PCMovementController pcMovementController;
    protected bool hasPlayerOnPlatform = false;

    //Color Lerping 
    private float timeStartedLerpingColor;
    private bool isLerpingColor;
    private float colorLerpingSpeed = 1;

    protected virtual void Start()
    {
        //Get Components
        platformMeshes = this.GetComponentsInChildren<MeshRenderer>();
        platformAudio = this.GetComponent<AudioSource>();
        platformCollider = this.GetComponent<Collider>();

        //Setup New Materials
        newMaterial = platformMeshes[0].material;
        newMaterial.color = startingColor;
        newMaterial.SetColor("_EmissionColor", Color.black);

        for(int i = 0; i < platformMeshes.Length; i++)
        {
            //Check that the mesh is a node
            if(platformMeshes[i].tag == "Node" || platformMeshes[i].transform.parent.tag == "Node")
            {
                platformMeshes[i].material = newMaterial;
            }
        }
    }

    protected void EnablePlayerInput()
    {
        EventManager.TriggerEvent(Events.EnablePlayerMovement);
    }

    protected void DisablePlayerInput()
    {
        EventManager.TriggerEvent(Events.DisablePlayerMovement);
    }

    public void ActivatePlatform()
    {
        timeStartedLerpingColor = Time.time;
        isLerpingColor = true;
    } 

    protected virtual void Update()
    {
        if (isLerpingColor)
            LerpColor();
    }

    private void LerpColor()
    {
        float timeSinceStarted = Time.time - timeStartedLerpingColor;
        float percentageComplete = timeSinceStarted / colorLerpingSpeed;

        Color newColor = Color.Lerp(startingColor, targetColor, percentageComplete);
        float emissionValue = Mathf.Lerp(0, 1, percentageComplete);
        newMaterial.color = newColor;
        newMaterial.SetColor("_EmissionColor", newColor);

        if (percentageComplete >= 1.0f)
        {
            isLerpingColor = false;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //Debug.Log("Parented " + this.gameObject.name);
            //other.transform.parent = this.transform;

            if (pcMovementController == null)
            {
                pcMovementController = other.GetComponent<PCMovementController>();
            }

            hasPlayerOnPlatform = true;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //other.transform.parent = null;
            hasPlayerOnPlatform = false;            
        }
    }
}
