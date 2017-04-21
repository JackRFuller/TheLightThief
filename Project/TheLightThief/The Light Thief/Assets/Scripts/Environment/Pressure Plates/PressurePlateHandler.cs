using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    [Header("Switches")]
    [SerializeField]
    private Switch[] switches;

    //Components
    private Animation plateAnim;
    private Collider plateColl;
    private AudioSource platformAudio;
    private bool hasBeenActivated = false;


    private void Start()
    {
        //Get Components
        plateAnim = this.GetComponent<Animation>();
        plateColl = this.GetComponent<Collider>();
        platformAudio = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !hasBeenActivated)
        {
            ActivatePressurePlate();
        }
    }

    private void ActivatePressurePlate()
    {
        //Turn Off Plate
        hasBeenActivated = true;
        plateColl.enabled = false;

        platformAudio.PlayOneShot(platformAudio.clip);

        if(!plateAnim.isPlaying)
            plateAnim.Play();

        for(int i = 0; i < switches.Length; i++)
        {
            switches[i].StartActivatingSwitch();
        }
    }

}
