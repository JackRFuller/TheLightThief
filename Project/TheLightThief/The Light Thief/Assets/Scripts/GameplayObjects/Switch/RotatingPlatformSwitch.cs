using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformSwitch : Switch
{
    [Header("Animation")]
    [SerializeField]
    private AnimationClip rotateCW;
    [SerializeField]
    private AnimationClip rotateCCW;
    private Animation switchAnim;

    [Header("Input Colliders")]
    [SerializeField]
    private Collider leftCollider;
    [SerializeField]
    private Collider rightCollider;

    protected override void Start()
    {
        switchAnim = this.GetComponent<Animation>();
        switchAudio = this.GetComponent<AudioSource>();
        leftCollider.enabled = false;
        rightCollider.enabled = false;
    }  

    public override void IncrementTriggers()
    {
       
        numOfTriggers++;
        if (numOfTriggers == 2)
        {
            isActivated = true;

            leftCollider.enabled = true;
            rightCollider.enabled = true;

            //switchCollider.enabled = true;

            //Turn Rotating Platforms To White
            for (int i = 0; i < rotatingPlatforms.Length; i++)
            {
                rotatingPlatforms[i].ActivatePlatform();
            }
        }
    }

    private void ActivatePlatformBehaviour(int direction)
    {
        //Check that connected platforms aren't already moving
        //Check that Platforms aren't already moving
        for (int i = 0; i < rotatingPlatforms.Length; i++)
        {
            if (rotatingPlatforms[i].IsMoving)
                return;
        }

        if(direction == 0)
        {
            switchAnim.clip = rotateCW;
        }
        else
        {
            switchAnim.clip = rotateCCW;
        }

        switchAnim.Play();

        for(int i = 0; i < rotatingPlatforms.Length; i++)
        {
            rotatingPlatforms[i].StartPlatformRotation(direction);
        }

        StartCoroutine(WaitToReCalibrateNodePositions());
    }

    
}
