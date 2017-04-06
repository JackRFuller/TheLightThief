using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //Componets
    private PCMovementController pcMovementController;

    private Camera mainCamera;

    private bool allowPlayerMovement = true;

    private void OnEnable()
    {
        EventManager.StartListening(Events.EnablePlayerMovement, EnableInput);
        EventManager.StartListening(Events.DisablePlayerMovement, DisableInput);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.EnablePlayerMovement, EnableInput);
        EventManager.StopListening(Events.DisablePlayerMovement, DisableInput);
    }

    private void Start()
    {
        pcMovementController = PCPathFindingHandler.Instance.GetComponent<PCMovementController>();
        mainCamera = this.GetComponent<Camera>();
    }

    private void Update()
    {
        GetMouseInput();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Node")
                {
                    if(allowPlayerMovement)
                    {
                        PCPathFindingHandler.Instance.CacheClickPointAndTargetPosition(hit.point, hit.transform);

                        if (PCPathFindingHandler.Instance.CheckIfPathIsValid())
                        {
                            //Let PC Move
                            pcMovementController.MoveToPosition(hit.point);
                            Debug.Log("Valid Path");
                        }
                        else
                        {
                            Debug.Log("No Valid Path");
                        }
                    }
                }

                if(hit.collider.tag == "Switch")
                {
                    hit.transform.SendMessage("ActivatePlatformBehaviour", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    private void EnableInput()
    {
        allowPlayerMovement = true;
    }

    private void DisableInput()
    {
        allowPlayerMovement = false;
    }
}
