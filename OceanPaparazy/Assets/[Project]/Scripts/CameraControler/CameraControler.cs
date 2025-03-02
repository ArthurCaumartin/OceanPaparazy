using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    public string DEBUG_CAMERA_NAME;
    [SerializeField] private Transform _target;

    [SerializeField] private CameraStateFisrtPersonne _firstPersonneState = new CameraStateFisrtPersonne();
    [SerializeField] private CameraStateThirdPersonne _thirdPersonneState = new CameraStateThirdPersonne();
    private ICameraState _currentState;

    public Transform Target { get => _target; }

    public CameraStateFisrtPersonne FisrtPersonne { get => _firstPersonneState; }
    public CameraStateThirdPersonne ThirdPersonne { get => _thirdPersonneState; }

    private Vector2 _inputVector;
    public Vector2 InputVector { get => _inputVector; }


    private void Start()
    {
        _firstPersonneState.Initialise(this);
        _thirdPersonneState.Initialise(this);

        SetState(ThirdPersonne);
    }

    private void Update()
    {
        DEBUG_CAMERA_NAME = _currentState.ToString();
        _currentState.UpdateState();
    }

    private void OnLook(InputValue value)
    {
        print("skldhburg");
        _inputVector = value.Get<Vector2>();
    }

    public void SetState(ICameraState toSet)
    {
        if (_currentState == toSet) return;
        _currentState = toSet;
    }
}
