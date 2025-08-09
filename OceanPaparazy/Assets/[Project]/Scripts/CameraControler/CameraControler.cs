using Alchemy.Inspector;
using UnityEngine;


public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private CameraSettings _settings;

    private Vector2 _inputMovementDirection;
    private Vector2 _inputLookDirection;
    private float _currentAngleXOffset;


    private void Awake()
    {
        _settings = CameraSettings.SplineDefault;
    }

    public void SetControler(Transform target, CameraSettings settings)
    {
        _target = target;
        _settings = settings;
    }

    private void Update()
    {
        if (_target == null) return;
        UpdateCamera();
    }


    public void UpdateCamera()
    {
        if (_target == null) return;

        if (_settings.isFirstPerson)
        {
            UpdateFirstPersonCamera();
        }
        else
        {
            UpdateThirdPersonCamera();
        }
    }

    private void UpdateFirstPersonCamera()
    {
        float xAngleTarget = _settings.followTargetForwardVertical ? _target.eulerAngles.x : -_currentAngleXOffset;
        Quaternion targetRotation = Quaternion.Euler(xAngleTarget, _target.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _settings.followSpeed);

        LerpToTargetPosition(_target.position + _settings.cameraPositionOffset);
    }

    private void UpdateThirdPersonCamera()
    {
        Vector3 targetPosition = _target.position + _settings.targetPositionOffset;
        LerpToTargetPosition(_target.position + (_target.forward * -_settings.distance) + _settings.cameraPositionOffset);
        transform.forward = Vector3.Slerp(transform.forward, (targetPosition - transform.position).normalized, Time.deltaTime * _settings.followSpeed);
    }

    private void LerpToTargetPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _settings.followSpeed);
    }

    public void SetMovementInput(Vector2 inputDirection)
    {
        _inputMovementDirection = inputDirection;
    }

    public void SetXAngleOffset(float angle)
    {
        _currentAngleXOffset = angle;
    }

    public void SetLookDirection(Vector2 lookDelta)
    {
        _inputLookDirection = lookDelta;
    }


    private void OnEnable()
    {
        InputEventManager.OnMoveEvent += SetMovementInput;
        InputEventManager.OnLookEvent += SetLookDirection;
    }

    private void OnDisable()
    {
        InputEventManager.OnMoveEvent -= SetMovementInput;
        InputEventManager.OnLookEvent -= SetLookDirection;
    }
}
