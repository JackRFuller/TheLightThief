using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFootstepHandler : MonoBehaviour
{
    private AudioSource footStepAudio;
    [SerializeField]
    private AudioClip[] footsteps;
    private int lastFootstepIndex;

    private void Start()
    {
        footStepAudio = this.GetComponent<AudioSource>();
    }

    private void PlayFootstep()
    {
        int footstepIndex = 0;

        do
        {
            footstepIndex = Random.Range(0, footsteps.Length);
        }
        while (footstepIndex == lastFootstepIndex);

        footStepAudio.PlayOneShot(footsteps[lastFootstepIndex]);

        lastFootstepIndex = footstepIndex;
    }
}
	
