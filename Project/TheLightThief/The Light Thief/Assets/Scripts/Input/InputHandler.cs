using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : BaseMonoBehaviour
{
    //Componets
    private CameraStateController cameraStateController;
    private PCMovementController pcMovementController;
    private AudioSource inputAudio;

    private Camera mainCamera;

    private bool allowPlayerMovement = true;

    [Header("Base Cursors")]
    [SerializeField]
    private Texture2D cursorImageWhite;
    [SerializeField]
    private Texture2D cursorImageBlack;
    private bool isWhite = true;
    private Vector2 hotSpot;

    [Header("Rotation Cursors")]
    [SerializeField]
    private Texture2D rotateCWWhite;
    [SerializeField]
    private Texture2D rotateCWBlack;
    [SerializeField]
    private Texture2D rotateCCWWhite;
    [SerializeField]
    private Texture2D rotateCCWBlack;

    [Header("Indicators")]
    [SerializeField]
    private GameObject playerWpPrefab;
    private GameObject playerWaypoint;

    private ActivePC activePC;
    private enum ActivePC
    {
        PC,
        Devil,
    }

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
        SetCursor();

        activePC = ActivePC.PC;
    }

    private void SetCursor()
    {
        hotSpot = new Vector2(cursorImageWhite.width * 0.5f, cursorImageWhite.height * 0.5f);

        if (isWhite)
        {
            Cursor.SetCursor(cursorImageWhite, hotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorImageBlack, hotSpot, CursorMode.Auto);
        }
    }

    public override void UpdateNormal()
    {
        GetInputTriggers();

        GetMouseInput();

        GetKeyboardInput();
    }

    private void GetKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            EventManager.TriggerEvent(Events.Invert);
            if (isWhite)
                isWhite = false;
            else
                isWhite = true;

            SetCursor();
        }
    }

    private void GetInputTriggers()
    {
        Ray detectionRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(detectionRay, out rayHit, Mathf.Infinity))
        {
            if(rayHit.collider.tag.Equals("RotateCCW"))
            {
                if(isWhite)
                {
                    Cursor.SetCursor(rotateCCWWhite, hotSpot, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(rotateCCWBlack, hotSpot, CursorMode.Auto);
                }
            }
            else if(rayHit.collider.tag.Equals("RotateCW"))
            {
                if (isWhite)
                {
                    Cursor.SetCursor(rotateCWWhite, hotSpot, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(rotateCWBlack, hotSpot, CursorMode.Auto);
                }
            }
            else if(!rayHit.collider.tag.Equals("RotateCW") && rayHit.collider.tag.Equals("RotateCCW"))
            {
                if (isWhite)
                {
                    Cursor.SetCursor(cursorImageWhite, hotSpot, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(cursorImageBlack, hotSpot, CursorMode.Auto);
                }
            }
        }
        else
        {
            if (isWhite)
            {
                Cursor.SetCursor(cursorImageWhite, hotSpot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(cursorImageBlack, hotSpot, CursorMode.Auto);
            }
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
                if(hit.collider.tag == "Player")
                {
                    if(activePC != ActivePC.PC)
                    {
                        EventManager.TriggerEvent(Events.SwitchPlayers);

                        activePC = ActivePC.PC;
                    }
                }

                if(hit.collider.tag == "Devil")
                {
                    if (activePC != ActivePC.Devil)
                    {
                        EventManager.TriggerEvent(Events.SwitchPlayers);
                        activePC = ActivePC.Devil;
                    }
                }

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

                if(hit.collider.tag == "RotateCW")
                {
                    hit.transform.parent.SendMessage("ActivatePlatformBehaviour", 0, SendMessageOptions.DontRequireReceiver);
                }

                if (hit.collider.tag == "RotateCCW")
                {
                    hit.transform.parent.SendMessage("ActivatePlatformBehaviour", 1, SendMessageOptions.DontRequireReceiver);
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
