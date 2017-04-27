using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingRingHandler : BaseMonoBehaviour
{
    //Components
    private Animation breathingRingAnim;

    [Header("Animations")]
    [SerializeField]
    private AnimationClip fadeIn;
    [SerializeField]
    private AnimationClip fadeOut;

    private Transform playerPoint;
    private LerpingAttributes lerpValues;
    private Vector3 startingPoint;
    private Vector3 targetPoint;
    private float timeStarted;
    private int lerpIndex;

    private BreathingMode breathingMode;
    private enum BreathingMode
    {
        Standard,
        Holding,
    }

    private void Start()
    {
        breathingRingAnim = this.GetComponent<Animation>();
    }

    public void SetupBreathingRing(Transform followPoint, LerpingAttributes lerpValue)
    {
        breathingMode = BreathingMode.Standard;

        playerPoint = followPoint;
        lerpValues = lerpValue;

        if(!breathingRingAnim)
            breathingRingAnim = this.GetComponent<Animation>();

        breathingRingAnim.clip = fadeIn;
        breathingRingAnim.Play();

        //Initiate Lerp
        startingPoint = this.transform.localScale;
        targetPoint = lerpValues.pointTwo;

        timeStarted = Time.time;
    }

    public override void UpdateNormal()
    {
        UpdatePosition();

        if(breathingMode == BreathingMode.Standard)
        {
            Breath();
        }
        else
        {
            HoldBreath();
        }
        
    }

    private void HoldBreath()
    {

    }

    private void Breath()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / lerpValues.lerpSpeed;

        Vector3 newSize = Vector3.Lerp(startingPoint, targetPoint, lerpValues.lerpCurve.Evaluate(percentageComplete));
        this.transform.localScale = newSize;

        if(percentageComplete >= 1.0f)
        {
            lerpIndex++;

            if (lerpIndex > 1)
                lerpIndex = 0;

            startingPoint = this.transform.localScale;

            if (lerpIndex == 0)
                targetPoint = lerpValues.pointTwo;
            else
                targetPoint = lerpValues.pointOne;

            timeStarted = Time.time;
        }
    }

    private void UpdatePosition()
    {
        this.transform.position = playerPoint.position;
    }

    public void FadeInRing()
    {
        breathingRingAnim.clip = fadeIn;
        breathingRingAnim.Play();
    }

    public void FadeOutRing()
    {
        breathingRingAnim.clip = fadeOut;
        breathingRingAnim.Play();
    }


}
