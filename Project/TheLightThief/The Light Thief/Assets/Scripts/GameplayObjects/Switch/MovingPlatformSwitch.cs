using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class MovingPlatformSwitch : Switch
{
    [Header("Animation Clips")]
    [SerializeField]
    private AnimationClip[] animClips;
    private Animation switchAnim;
    private int animIndex = 0;

    protected override void Start()
    {
        base.Start();
        switchAnim = this.GetComponent<Animation>();
    }

    protected override void ActivatePlatformBehaviour()
    {
        base.ActivatePlatformBehaviour();

        if (!switchAnim.isPlaying)
        {
            switchAnim.clip = animClips[animIndex];
            switchAnim.Play();

            animIndex++;
            if (animIndex > 1)
                animIndex = 0;
        }

        StartCoroutine(WaitToReCalibrateNodePositions());

    }
}
