using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIHandler : BaseMonoBehaviour
{
    private Animation uiAnim;
    [Header("Animation Clips")]
    [SerializeField]
    private AnimationClip whiteFlash;

    private void OnEnable()
    {
        EventManager.StartListening(Events.WhiteFlash, WhiteFlash);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.WhiteFlash, WhiteFlash);
    }

    private void Start()
    {
        //Get Components
        uiAnim = this.GetComponent<Animation>();
    }

    private void WhiteFlash()
    {
        uiAnim.clip = whiteFlash;
        uiAnim.Play();
    }


}
