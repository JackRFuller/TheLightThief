﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCStealthHandler : BaseMonoBehaviour
{

    //Components
    private Animator enemyAnim;
    private PCMovementController pcMovementHandler;
    private List<Transform> enemies;

    [Header("Breathing Ring")]
    [SerializeField]
    private GameObject breathingRingPrefab;    

    private GameObject breathingRing;
    private BreathingRingHandler breathingRingHandler;

    [Header("Breathing Ring Movement")]
    [SerializeField]
    private Transform followPoint;
    [SerializeField]
    private LerpingAttributes lerpValues;

    private bool hasReAdjustedSpeed;

    private void Start()
    {
        //Get Components
        pcMovementHandler = this.GetComponent<PCMovementController>();
        enemyAnim = this.transform.GetChild(0).GetComponent<Animator>();

        //Get Enemies in the Level
        enemies = new List<Transform>();
        NPCMovementHandler[] NPCs = FindObjectsOfType<NPCMovementHandler>();

        for(int i = 0; i < NPCs.Length; i++)
        {
            enemies.Add(NPCs[i].transform);
        }

        //Create Breathing Ring
        if (enemies.Count > 0)
        {
            breathingRing = Instantiate(breathingRingPrefab) as GameObject;
            breathingRingHandler = breathingRing.GetComponent<BreathingRingHandler>();
            breathingRingHandler.SetupBreathingRing(followPoint, lerpValues);
        }         
    }    

    public override void UpdateNormal()
    {
        if(enemies.Count > 0)
        {
            DetectEnemy();
        }
        
    }   

    private void DetectHoldingBreathInput()
    {
        if(enemyAnim.GetBool("isSneaking"))
        {
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if(hit.collider.tag.Equals("Player"))
                    {

                    }
                }
            }
        }
    }

    private void HoldBreath()
    {

    }  

    private void DetectEnemy()
    {
        //Get Closest Enemy
        Transform closestEnemy = FindClosestEnemy();

        //Check how close enemy is
        float dist = Vector3.Distance(closestEnemy.position, this.transform.position);

        //Enable Stealth Mode
        if (dist < 10.0)
        {
            enemyAnim.SetBool("isSneaking", true);

            if (!hasReAdjustedSpeed)
            {
                breathingRingHandler.FadeInRing();

                pcMovementHandler.MoveToPosition();
                hasReAdjustedSpeed = true;
            }
        }
        else //Disable Stealth Mode
        {
            enemyAnim.SetBool("isSneaking", false);

            if (hasReAdjustedSpeed)
            {
                breathingRingHandler.FadeOutRing();

                pcMovementHandler.MoveToPosition();
                hasReAdjustedSpeed = false;
            }
        }
    }

    private Transform FindClosestEnemy()
    {
        Transform closestEnemy = enemies[0];
        float dist = Vector3.Distance(closestEnemy.position, this.transform.position);

        for(int i = 0; i < enemies.Count; i++)
        {
            float newDist = Vector3.Distance(enemies[i].position, this.transform.position);
            if(newDist < dist)
            {
                closestEnemy = enemies[i];
                dist = newDist;
            }
        }

        return closestEnemy;

    }

}
