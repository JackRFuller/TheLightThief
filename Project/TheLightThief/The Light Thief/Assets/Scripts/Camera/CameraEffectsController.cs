using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffectsController : BaseMonoBehaviour
{
    private PostProcessingProfile postProcessingProfile;

    [Header("Effects - Holding Breath")]
    [SerializeField]
    private MathfLerpingAttributes holdingBreathVignetteEffect;
    [SerializeField]
    private MathfLerpingAttributes holdingBreathCameraZoomEffect;
    private LerpingVariables breathVignetteEffect;
    private LerpingVariables breathCameraEffect;


    [Header("Effects - Releasing Breath")]
    [SerializeField]
    private MathfLerpingAttributes releasingBreathVignetteEffect;
    [SerializeField]
    private MathfLerpingAttributes releasingBreathCameraZoomEffect;

    //General Lerping Variables
    private class LerpingVariables
    {
        public float startPoint;
        public float targetPoint;
        public float lerpSpeed;
        public AnimationCurve lerpCurve;
        public float timeStarted;
    }
    private float startPoint;
    private float endPoint;
    private float timeStartedBreathEffect;
    private bool isLerpingBreathEffect;

    private void OnEnable()
    {
        EventManager.StartListening(Events.HoldingBreath, InitHoldingBreathEffects);
        EventManager.StartListening(Events.StopHoldingBreath, InitReleasingBreathEffects);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.HoldingBreath, InitHoldingBreathEffects);
        EventManager.StopListening(Events.StopHoldingBreath, InitReleasingBreathEffects);
    }

    private void Start()
    {
        postProcessingProfile = this.GetComponent<PostProcessingBehaviour>().profile;
    }

    public override void UpdateNormal()
    {
        if (isLerpingBreathEffect)
        {
            LerpBreathVignetteEffects();

            LerpBreathCameraZoomEffects();
        }
    }

    private void InitHoldingBreathEffects()
    {
        breathVignetteEffect = new LerpingVariables();
        breathCameraEffect = new LerpingVariables();

        //Init Vignette Effect
        breathVignetteEffect.startPoint = postProcessingProfile.vignette.settings.intensity;
        breathVignetteEffect.targetPoint = holdingBreathVignetteEffect.pointTwo;
        breathVignetteEffect.lerpSpeed = holdingBreathVignetteEffect.lerpSpeed;
        breathVignetteEffect.lerpCurve = holdingBreathVignetteEffect.lerpCurve;

        //Init Camera Effect
        breathCameraEffect.startPoint = Camera.main.orthographicSize;
        breathCameraEffect.targetPoint = holdingBreathCameraZoomEffect.pointTwo;
        breathCameraEffect.lerpSpeed = holdingBreathCameraZoomEffect.lerpSpeed;
        breathCameraEffect.lerpCurve = holdingBreathCameraZoomEffect.lerpCurve;

        timeStartedBreathEffect = Time.time;    
        isLerpingBreathEffect = true;
    }

    private void InitReleasingBreathEffects()
    {
        //Init Vignette Effect
        breathVignetteEffect.startPoint = postProcessingProfile.vignette.settings.intensity;
        breathVignetteEffect.targetPoint = releasingBreathVignetteEffect.pointOne;
        breathVignetteEffect.lerpSpeed = releasingBreathVignetteEffect.lerpSpeed;
        breathVignetteEffect.lerpCurve = releasingBreathVignetteEffect.lerpCurve;

        //Init Camera Effect
        breathCameraEffect.startPoint = Camera.main.orthographicSize;
        breathCameraEffect.targetPoint = releasingBreathCameraZoomEffect.pointOne;
        breathCameraEffect.lerpSpeed = releasingBreathCameraZoomEffect.lerpSpeed;
        breathCameraEffect.lerpCurve = releasingBreathCameraZoomEffect.lerpCurve;

        timeStartedBreathEffect = Time.time;
    }   

    private void LerpBreathVignetteEffects()
    {
        float timeSinceStarted = Time.time - timeStartedBreathEffect;
        float percentageComplete = timeSinceStarted / breathVignetteEffect.lerpSpeed;

        float vignetteIntensity = Mathf.Lerp(breathVignetteEffect.startPoint, breathVignetteEffect.targetPoint, breathVignetteEffect.lerpCurve.Evaluate(percentageComplete));

        var vignetteSettings = postProcessingProfile.vignette.settings;
        vignetteSettings.intensity = vignetteIntensity;       

        postProcessingProfile.vignette.settings = vignetteSettings;
    }

    private void LerpBreathCameraZoomEffects()
    {
        float timeSinceStarted = Time.time - timeStartedBreathEffect;
        float percentageComplete = timeSinceStarted / breathCameraEffect.lerpSpeed;

        float cameraZoom = Mathf.Lerp(breathCameraEffect.startPoint, breathCameraEffect.targetPoint, breathCameraEffect.lerpCurve.Evaluate(percentageComplete));

        Camera.main.orthographicSize = cameraZoom;
    }

}
