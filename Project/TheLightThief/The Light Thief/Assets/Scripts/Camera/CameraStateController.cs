using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateController : BaseMonoBehaviour
{
    private static CameraStateController instance;
    public static CameraStateController Instance { get { return instance; } }

    //States
    public CameraFollowState followState;
    public ICameraState trackState;
    public ICameraState shakeState;
    public ICameraState switchState;

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

    private Collider mainPC;
    private Collider devilPC;

    private Collider target;
    public Collider Target { get { return target; } }

    //Camera Shake
    private CameraShakeProperties shakeProperties;
    public CameraShakeProperties ShakeProperties { get { return shakeProperties; } }

    protected override void Awake()
    {
        base.Awake();

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.StartedMoving, StartTrackingPlayer);   
        EventManager.StartListening(Events.SwitchPlayers, SwitchPlayableCharacters);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.StartedMoving, StartTrackingPlayer);
        EventManager.StopListening(Events.SwitchPlayers, SwitchPlayableCharacters);
    }  

    private void Start()
    {
        //Get Target
        mainPC = PCPathFindingHandler.Instance.GetComponent<Collider>();
        devilPC = DevilPathFinding.Instance.GetComponent<Collider>();
        target = mainPC;

        //Create States
        trackState = new CameraTrackState(this);
        followState = new CameraFollowState(this);
        shakeState = new CameraShakeState(this);
        switchState = new CameraSwitchState(this);

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
        if(lastState == followState)
        {
            followState.OnLateUpdateState();
        }
    }

    private void SwitchPlayableCharacters()
    {
        if(target == mainPC)
        {
            target = devilPC;
        }
        else
        {
            target = mainPC;
        }

        CurrentState = switchState;
    }

    public void StartTrackingPlayer()
    {
        CurrentState = trackState;
    }

    public void StartFollowingPlayer()
    {
        if(CurrentState != shakeState)
        {
            CurrentState = followState;
        }
        
    }
}
