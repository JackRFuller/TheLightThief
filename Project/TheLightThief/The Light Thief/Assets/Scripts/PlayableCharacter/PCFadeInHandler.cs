using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFadeInHandler : BaseMonoBehaviour
{
    private Animation fadeInAnim;

    public void FadeIn()
    {
        if(fadeInAnim == null)
            fadeInAnim = this.GetComponent<Animation>();

        fadeInAnim.Play();
    }

}
