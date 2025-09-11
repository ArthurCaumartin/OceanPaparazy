using UnityEngine;

public class CameraStateMachine : StateMachine
{
    [SerializeField] private Camera controledCamera;
    [SerializeField] private CameraStateSpline cameraStateSpline = new();
    [SerializeField] private CameraStateFirstPerson cameraStateFirstPerson = new();
    private Vector2 _movementInput;
    private Vector2 _lookInput;

    public CameraStateSpline CameraStateSpline => cameraStateSpline;
    public CameraStateFirstPerson CameraStateFirstPerson => cameraStateFirstPerson;


    private void Awake()
    {
        controledCamera = Camera.main;

        cameraStateSpline.Init(this, controledCamera, null);
        cameraStateFirstPerson.Init(this, controledCamera, null);
    }

    protected override void Update()
    {
        if (currentState == null) return;
        (currentState as CameraState).UpdateStateInput(_movementInput, _lookInput);
    }

    public void SetCameraState(CameraState newState, Transform redefineTarget = null)
    {
        if (newState == null) return;

        currentState?.ExitState();
        currentState = newState;
        debugStateName = currentState.ToString();
        currentState?.EnterState();
        (currentState as CameraState).SetTarget(redefineTarget);
    }

    private void SetMovementInput(Vector2 input) => _movementInput = input;
    private void SetLookInput(Vector2 input) => _lookInput = input;

    private void OnEnable()
    {
        InputEventManager.OnMoveEvent += SetMovementInput;
        InputEventManager.OnLookEvent += SetLookInput;
    }

    private void OnDisable()
    {
        InputEventManager.OnMoveEvent -= SetMovementInput;
        InputEventManager.OnLookEvent -= SetLookInput;
    }

    public void OutCameraPositionAndRotation(out Vector3 position, out Quaternion rotation)
    {
        position = controledCamera.transform.position;
        rotation = controledCamera.transform.rotation;
    }
}



