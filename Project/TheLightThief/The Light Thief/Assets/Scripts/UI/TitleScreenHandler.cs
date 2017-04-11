using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenHandler : MonoBehaviour
{
    public void NewGame(Button hitButton)
    {
        hitButton.enabled = false;
        CameraMovementHandler.Instance.InitCameraMovement();
    }
	
}
