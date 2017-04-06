using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHandler : MonoBehaviour
{
    [SerializeField]
    private MovingPlatformHandler[] movingPlatforms;

    private void ActivatePlatformBehaviour()
    {
        //Check that Platforms aren't already moving
        for(int i = 0; i < movingPlatforms.Length; i++)
        {
            if (movingPlatforms[i].IsMoving)
                return;
        }

        for (int i = 0; i < movingPlatforms.Length; i++)
        {
            movingPlatforms[i].StartMoving();
        }

    }
	
}
