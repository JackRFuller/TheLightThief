using System.Collections;
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
    private Animation breathingRingAnim;
    [SerializeField]
    private AnimationClip[] breathingRingAnimClips;

    [Header("Breathing Ring Movement")]
    [SerializeField]
    private Transform breathingRing;
    [SerializeField]
    private Vector3 smallestSize;
    [SerializeField]
    private Vector3 largestSize;
    [SerializeField]
    private float breathingSpeed;
    [SerializeField]
    private AnimationCurve breathingCurve;
    private Vector3 startSize;
    private Vector3 targetSize;
    private float timeStartedBreathing;
    private float breathingIndex;
    

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

        InitBreathing();
    }

    public override void UpdateNormal()
    {
        if(enemies.Count > 0)
        {
            DetectEnemy();

            Breath();
        }
        
    }

    private void InitBreathing()
    {
        startSize = breathingRing.localScale;
        if(breathingIndex == 0)
        {
            targetSize = largestSize;
        }
        else
        {
            targetSize = smallestSize;
        }

        timeStartedBreathing = Time.time;
    }

    private void Breath()
    {
        float timeSinceStarted = Time.time - timeStartedBreathing;
        float percentageComplete = timeSinceStarted / breathingSpeed;

        Vector3 newSize = Vector3.Lerp(startSize, targetSize, breathingCurve.Evaluate(percentageComplete));

        breathingRing.localScale = newSize;

        if(percentageComplete >= 1.0f)
        {
            breathingIndex++;
            if (breathingIndex > 1)
                breathingIndex = 0;

            InitBreathing();
        }
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
                breathingRingAnim.clip = breathingRingAnimClips[0];
                breathingRingAnim.Play();

                pcMovementHandler.MoveToPosition();
                hasReAdjustedSpeed = true;
            }
        }
        else //Disable Stealth Mode
        {
            enemyAnim.SetBool("isSneaking", false);

            if (hasReAdjustedSpeed)
            {
                breathingRingAnim.clip = breathingRingAnimClips[1];
                breathingRingAnim.Play();

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
