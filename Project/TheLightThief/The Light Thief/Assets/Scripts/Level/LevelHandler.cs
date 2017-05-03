using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoSingleton<LevelHandler>
{
    private PCStartPoint playerStartPoint;

    [SerializeField]
    private Parallaxing parallaxManager;

    [SerializeField]
    private Transform nextLevelTarget;
    public  Transform NextLevelTarget { get { return nextLevelTarget; } }

    [SerializeField]
    private Vector3 nextLevelSpawnSize;
    public Vector3 NextLevelSpawnSize { get { return nextLevelSpawnSize; } }

    [SerializeField]
    private Vector3 cameraRotationTarget;
    public Vector3 CameraRotationTarget { get { return cameraRotationTarget; } }

    [Header("Level Growth Attributes")]
    [SerializeField]
    private float growthSpeed;
    [SerializeField]
    private AnimationCurve growthCurve;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float timeStartedGrowing;
    private bool isGrowing;

    private void OnEnable()
    {
        EventManager.StartListening(Events.TransitionLevels, TurnOffParallaxing);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.TransitionLevels, TurnOffParallaxing);
    }

    public void InitGrowLevel(Vector3 startingSize)
    {
        //Find Player Start Point
        playerStartPoint = GetComponentInChildren<PCStartPoint>();

        this.transform.localScale = startingSize;
        startPoint = this.transform.localScale;
        Debug.Log(startPoint + gameObject.name);
        targetPoint = Vector3.one;

        timeStartedGrowing = Time.time;
        isGrowing = true;
        Debug.Log("STarted Growing");
        
    }

    private void TurnOffParallaxing()
    {
        parallaxManager.enabled = false;
    }

    private void Update()
    {
        if (isGrowing)
        {
            GrowLevel();
        }
    }

    private void GrowLevel()
    {
        float timeSinceStarted = Time.time - timeStartedGrowing;
        float percentageComplete = timeSinceStarted / growthSpeed;

        Vector3 newSize = Vector3.Lerp(startPoint, targetPoint, growthCurve.Evaluate(percentageComplete));
        this.transform.localScale = newSize;
        
        if(percentageComplete >= 1.0f)
        {
            isGrowing = false;
            playerStartPoint.enabled = true;
        }
    }
}
