using UnityEngine;

public class CameraStateMachine : MonoBehaviour
{
    [SerializeField] private string _debugStateName;
    [SerializeField] private Camera controledCamera;
    [SerializeField] private CameraStateSpline cameraStateSpline = new CameraStateSpline();
    [SerializeField] private CameraStateFirstPerson cameraStateFirstPerson = new CameraStateFirstPerson();
    private CameraState _currentState;
    private Vector2 _movementInput;
    private Vector2 _lookInput;

    public CameraStateSpline CameraStateSpline => cameraStateSpline;
    public CameraStateFirstPerson CameraStateFirstPerson => cameraStateFirstPerson;


    private void Awake()
    {
        controledCamera = Camera.main;

        cameraStateSpline.Initialize(controledCamera);
        cameraStateFirstPerson.Initialize(controledCamera);
    }

    private void Update()
    {
        if (_currentState == null) return;

        //TODO add transition logic

        _currentState.UpdateState(_movementInput, _lookInput);
    }

    public void SetState(CameraState newState, Transform redefineTarget = null)
    {
        if (newState == null) return;
        // if (_currentState != null && _currentState.GetType() == newState.GetType())
        //     return;

        _currentState?.Exit();
        _currentState = newState;
        _debugStateName = _currentState.ToString();
        _currentState?.Enter(redefineTarget);
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



