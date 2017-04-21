using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateController : BaseMonoBehaviour
{
    //States
    public CameraFollowState followState;
    public ICameraState trackState;
    public ICameraState CurrentState;
    private ICameraState lastState;


    [Header("Camera Follow Attributes")]
    [SerializeField]
    private Vector2 focusAreaSize;
    public Vector2 FocusAreaSize { get { return focusAreaSize; } }
    [SerializeField]
    private float verticalOffset;
    public float VerticalOffset { get { return verticalOffset; } }
    [SerializeField]
    private float lookAheadDistX;
    public float LookAheadDistX { get { return lookAheadDistX; } }
    [SerializeField]
    private float lookSmoothTimeX;
    public float LookSmoothTimeX { get { return lookSmoothTimeX; } }
    [SerializeField]
    private float verticalSmoothTime;
    public float VerticalSmoothTime { get { return verticalSmoothTime; } }

    private Collider target;
    public Collider Target { get { return target; } }

    private void OnEnable()
    {
        EventManager.StartListening(Events.StartedMoving, StartTrackingPlayer);
        EventManager.StartListening(Events.EndedMoving, StartFollowingPlayer);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.StartedMoving, StartTrackingPlayer);
        EventManager.StopListening(Events.EndedMoving, StartFollowingPlayer);
    }  

    private void Start()
    {
        //Get Target
        target = PCPathFindingHandler.Instance.GetComponent<Collider>();

        //Create States
        trackState = new CameraTrackState(this);
        followState = new CameraFollowState(this);

        //Set Initial State
        CurrentState = followState;
    }    

    public override void UpdateNormal()
    {
        if(lastState != CurrentState)
        {
            CurrentState.OnEnterState();
            lastState = CurrentState;
        }
        else
        {
            CurrentState.OnUpdateState();
        }
    }

    public override void UpdateLate()
    {
        if(CurrentState == followState)
        {
            followState.OnLateUpdateState();
        }
    }

    private void StartTrackingPlayer()
    {
        CurrentState = trackState;
    }

    private void StartFollowingPlayer()
    {
        CurrentState = followState;
    }
}
