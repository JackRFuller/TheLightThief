﻿using System.Collections;
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

    //Enemies
    protected List<NPCMovementHandler> enemies;

    protected NonStaticPlatformInversion inversionHandler;

    protected virtual void Start()
    {
        //Get Components
        inversionHandler = this.GetComponent<NonStaticPlatformInversion>();
        platformMeshes = this.GetComponentsInChildren<MeshRenderer>();
        platformAudio = this.GetComponent<AudioSource>();
        platformCollider = this.GetComponent<Collider>();

        //Setup New Materials
        newMaterial = platformMeshes[0].material;
        newMaterial.color = startingColor;
        newMaterial.SetColor("_EmissionColor", Color.black);

        //Setup Enemy List
        enemies = new List<NPCMovementHandler>();

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

        if(other.tag.Equals("Enemy"))
        {
            NPCMovementHandler enemy = other.GetComponent<NPCMovementHandler>();
            if(!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //other.transform.parent = null;
            hasPlayerOnPlatform = false;            
        }

        if(other.tag.Equals("Enemy"))
        {
            NPCMovementHandler enemy = other.GetComponent<NPCMovementHandler>();
            enemies.Remove(enemy);
            enemies.TrimExcess();
        }
    }
}
