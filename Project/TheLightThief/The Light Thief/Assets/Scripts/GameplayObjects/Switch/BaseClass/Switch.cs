using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Switch : BaseMonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    protected MeshRenderer switchMesh;   
    private Collider switchCollider;
    private AudioSource switchAudio;

    [Header("Associated Platforms")]
    [SerializeField]
    protected MovingPlatformPath[] movingPlatforms;
    [SerializeField]
    protected RotatingPlatformHandler[] rotatingPlatforms;

    [Header("Associated Wires")]
    [SerializeField]
    protected WireHandler[] wires;

    [Header("Screenshake")]
    [SerializeField]
    protected CameraScreenShake.Properties screenShakeProperties;

    //Colour Lerp Values
    private float startingAlphaValue;
    private float timeStarted;
    private bool isActivating;
    private Material newMaterial;
    private Color newColor;

    private int numOfTriggers;

    protected virtual void Start()
    {
        switchCollider = this.GetComponent<Collider>();
        switchAudio = this.GetComponent<AudioSource>();

        switchCollider.enabled = false;
    }

    public void StartActivatingSwitch()
    {
        newMaterial = switchMesh.materials[0];
        newColor = newMaterial.color;

        for (int i = 0; i < wires.Length; i++)
        {
            wires[i].TriggerWires(this);
        }        

        timeStarted = Time.time;
        isActivating = true;
    }

    public override void UpdateNormal()
    {
        if (isActivating)
            ActivateSwitch();
    }

    private void ActivateSwitch()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / 1.0f;

        newColor.a = Mathf.Lerp(newColor.a, 1, percentageComplete);
        newMaterial.color = newColor;

        switchMesh.GetComponent<MeshRenderer>().material = newMaterial;

        if(percentageComplete >= 1.0f)
        {
            isActivating = false;
            IncrementTriggers();
        }
    }

    /// <summary>
    /// Used for turning switch collider on
    /// </summary>
    public void IncrementTriggers()
    {
        numOfTriggers++;
        if(numOfTriggers == 2)
        {
            switchCollider.enabled = true;

            //Turn Rotating Platforms To White
            for (int i = 0; i < rotatingPlatforms.Length; i++)
            {
                rotatingPlatforms[i].ActivatePlatform();
            }

            //Turn Moving Platform Paths On
            for (int i = 0; i < movingPlatforms.Length; i++)
            {
                movingPlatforms[i].ActivatePath();
            }
        }
    }

    protected virtual void ActivatePlatformBehaviour()
    {
        //Check that connected platforms aren't already moving
        //Check that Platforms aren't already moving
        for (int i = 0; i < rotatingPlatforms.Length; i++)
        {
            if (rotatingPlatforms[i].IsMoving)
                return;
        }

        for (int i = 0; i < movingPlatforms.Length; i++)
        {
            if (movingPlatforms[i].IsMoving)
                return;
        }

        //Activate Platforms
        for (int i = 0; i < movingPlatforms.Length; i++)
        {
            movingPlatforms[i].ActivateMovingPlatform();
        }

        for (int i = 0; i < rotatingPlatforms.Length; i++)
        {
            rotatingPlatforms[i].StartPlatformRotation();
        }        
        
        CameraScreenShake.Instance.StartShake(screenShakeProperties);

    }

    protected IEnumerator WaitToReCalibrateNodePositions()
    {
        yield return new WaitForSeconds(0.2f);        
        PCPathFindingHandler.Instance.CheckIfPathIsValid();
    }

}
