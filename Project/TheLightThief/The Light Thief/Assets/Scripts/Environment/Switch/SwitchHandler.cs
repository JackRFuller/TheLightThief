﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHandler : MonoBehaviour
{
    //Components 
    [Header("Components")]
    [SerializeField]
    private Transform wheelMesh;
    [SerializeField]
    private Material wheelMaterial;
    private Animation switchAnim;

    [Header("Associated Platforms")]
    [SerializeField]
    private MovingPlatformHandler[] movingPlatforms;
    [SerializeField]
    private RotatingPlatformHandler[] rotatingPlatforms;

    //Colour Lerping
    private float startingAlphaValue;
    private float timeStarted;
    private bool isActivating;

    private void Start()
    {
        switchAnim = this.GetComponent<Animation>();
    }

    /// <summary>
    /// Triggered by associated pressure plate
    /// </summary>
    public void StartActivatingSwitch()
    {
        startingAlphaValue = wheelMaterial.color.a;
        timeStarted = Time.time;
        isActivating = true;
    }

    private void Update()
    {
        if (isActivating)
            ActivateSwitch();
    }

    private void ActivateSwitch()
    {
        //float timeSinceStarted = Time.time - timeStarted;
        //float percentageComplete = timeSinceStarted / 5;

        //Material newMat = new Material(Shader.Find("Diffuse"));
        
        

        //newMat.color = newColor;
        //foreach(Transform part in wheelMesh)
        //{
        //    part.GetComponent<MeshRenderer>().material = newMat;
        //}

        //if(percentageComplete >= 1.0f)
        //{
        //    isActivating = false;
        //}

    }


    private void ActivatePlatformBehaviour()
    {
        //Check that Platforms aren't already moving
        for(int i = 0; i < movingPlatforms.Length; i++)
        {
            if (movingPlatforms[i].IsMoving)
                return;
        }

        for(int i = 0; i < rotatingPlatforms.Length; i++)
        {
            if (rotatingPlatforms[i].IsMoving)
                return;
        }

        for(int i = 0; i < rotatingPlatforms.Length; i++)
        {
            rotatingPlatforms[i].StartPlatformRotation();
        }

        for (int i = 0; i < movingPlatforms.Length; i++)
        {
            movingPlatforms[i].StartMoving();
        }

        if(!switchAnim.isPlaying)
            switchAnim.Play();

        StartCoroutine(WaitToReCalibrateNodePositions());

    }

    IEnumerator WaitToReCalibrateNodePositions()
    {
        yield return new WaitForSeconds(0.2f);
        EventManager.TriggerEvent(Events.RecalibrateNodes);
        PCPathFindingHandler.Instance.CheckIfPathIsValid();
    }
	
}
