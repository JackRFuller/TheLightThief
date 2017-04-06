using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    [Header("Switches")]
    [SerializeField]
    private SwitchHandler[] switches;

    //Components
    private Animation plateAnim;
    private Collider plateColl;


    private void Start()
    {
        //Get Components
        plateAnim = this.GetComponent<Animation>();
        plateColl = this.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            ActivatePressurePlate();
        }
    }

    private void ActivatePressurePlate()
    {
        //Turn Off Plate
        plateColl.enabled = false;

        if(!plateAnim.isPlaying)
            plateAnim.Play();

        for(int i = 0; i < switches.Length; i++)
        {
            switches[i].StartActivatingSwitch();
        }
    }

}
