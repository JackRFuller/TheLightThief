using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDeathHandler : BaseMonoBehaviour
{
    private PCMovementController pcMovementHandler;
    private PCStealthHandler pcStealthHandler;

    //Components
    [SerializeField]
    private Animator pcAnim;

    private void Start()
    {
        pcMovementHandler = this.GetComponent<PCMovementController>();
        pcStealthHandler = this.GetComponent<PCStealthHandler>();
    }

    private void Death()
    {
        EventManager.TriggerEvent(Events.DisablePlayerMovement);

        if(pcAnim)
            pcAnim.SetBool("isDead",true);

        pcMovementHandler.KillPlayerMovement();
        pcStealthHandler.PlayerHasDied();

    }
	
}
