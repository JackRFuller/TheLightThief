using System.Collections;
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
    private Collider switchCol;
    private AudioSource switchAudio;

    [Header("Audio")]
    [SerializeField]
    private AudioClip startUpSFX;
    [SerializeField]
    private AudioClip activatedSFX;

    [Header("Associated Paths")]
    [SerializeField]
    private MovingPlatformPath[] movingPlatforms;
    [SerializeField]
    private RotatingPlatformHandler[] rotatingPlatforms;

    [Header("Associated Wires")]
    [SerializeField]
    private WireHandler[] wires;

    [Header("Screenshake")]
    [SerializeField]
    private CameraScreenShake.Properties screenShakeProperties;

    //Colour Lerping
    private float startingAlphaValue;
    private float timeStarted;
    private bool isActivating;
    private Material newMat;
    private Color newColor;

    private int triggers = 0; //When Triggers == 2 then switch is active

    private void Start()
    {
        switchAnim = this.GetComponent<Animation>();
        switchCol = this.GetComponent<Collider>();
        switchCol.enabled = false;
        switchAudio = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Triggered by associated pressure plate
    /// </summary>
    public void StartActivatingSwitch()
    {
        switchCol.enabled = false;
        newMat = wheelMesh.GetChild(0).GetComponent<MeshRenderer>().materials[0];
        newColor = newMat.color;

        timeStarted = Time.time;
        isActivating = true;

        for(int i = 0; i < wires.Length; i++)
        {
            wires[i].TriggerWires(this);
        }

        switchAudio.PlayOneShot(startUpSFX);        
    }

    private void Update()
    {
        if (isActivating)
            ActivateSwitch();

        if (triggers == 2 && !switchCol.enabled)
            TurnOnSwitchCollider();

    }

    private void ActivateSwitch()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / 1;

        newColor.a = Mathf.Lerp(newColor.a, 1, percentageComplete);
        newMat.color = newColor;

        newMat.color = newColor;
        foreach (Transform part in wheelMesh)
        {
            part.GetComponent<MeshRenderer>().material = newMat;
        }

        if (percentageComplete >= 1.0f)
        {
            isActivating = false;
            IncremementTrigger();
        }
    }

    private void TurnOnSwitchCollider()
    {
        switchCol.enabled = true;
    }

    public void IncremementTrigger()
    {
        triggers++;
    }
    
    private void ActivatePlatformBehaviour()
    {
        //Check that Platforms aren't already moving
        for(int i = 0; i < rotatingPlatforms.Length; i++)
        {
            if (rotatingPlatforms[i].IsMoving)
                return;
        }

        for(int i = 0; i < movingPlatforms.Length; i++)
        {
            if (movingPlatforms[i].IsMoving)
                return;
        }

        Debug.Log("Can Activate Platforms");

        //Activate Platforms
        for (int i = 0; i < movingPlatforms.Length; i++)
        {
            movingPlatforms[i].ActivateMovingPlatform();
        }

        for (int i = 0; i < rotatingPlatforms.Length; i++)
        {
            rotatingPlatforms[i].StartPlatformRotation();
        }

        

        if (!switchAnim.isPlaying)
            switchAnim.Play();

        switchAudio.PlayOneShot(activatedSFX);

        StartCoroutine(WaitToReCalibrateNodePositions());
        CameraScreenShake.Instance.StartShake(screenShakeProperties);

    }

    IEnumerator WaitToReCalibrateNodePositions()
    {
        yield return new WaitForSeconds(0.2f);
        EventManager.TriggerEvent(Events.RecalibrateNodes);
        PCPathFindingHandler.Instance.CheckIfPathIsValid();
    }
	
}
