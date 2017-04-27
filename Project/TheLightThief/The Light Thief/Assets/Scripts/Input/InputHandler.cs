using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //Componets
    private CameraStateController cameraStateController;
    private PCMovementController pcMovementController;
    private AudioSource inputAudio;

    private Camera mainCamera;

    private bool allowPlayerMovement = true;

    [Header("Cursor")]
    [SerializeField]
    private Texture2D cursorImage;

    [Header("Indicators")]
    [SerializeField]
    private GameObject playerWpPrefab;
    private GameObject playerWaypoint;

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
        //Get Components
        cameraStateController = this.GetComponent<CameraStateController>();
        pcMovementController = PCPathFindingHandler.Instance.GetComponent<PCMovementController>();
        mainCamera = this.GetComponent<Camera>();
        inputAudio = this.GetComponent<AudioSource>();

        //Set Cursor Sprite
        Vector2 hotSpot = new Vector2(cursorImage.width * 0.5f, cursorImage.height * 0.5f);
        Cursor.SetCursor(cursorImage, hotSpot, CursorMode.Auto);

       
    }

    private void Update()
    {
        GetMouseInput();

        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            EventManager.TriggerEvent(Events.Invert);
        }
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
                            //Spawn In Waypoint
                            if(playerWaypoint == null)
                            {
                                playerWaypoint = Instantiate(playerWpPrefab) as GameObject;
                            }
                            
                            playerWaypoint.transform.position = new Vector3(hit.point.x,
                                                                            hit.point.y,
                                                                            playerWaypoint.transform.position.z);
                            playerWaypoint.GetComponent<Animation>().Play();

                            cameraStateController.StartFollowingPlayer();

                            //Let PC Move
                            pcMovementController.isMoving = true;
                            pcMovementController.targetPoint = hit.point;
                            pcMovementController.MoveToPosition();

                            inputAudio.PlayOneShot(inputAudio.clip);
                            
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
