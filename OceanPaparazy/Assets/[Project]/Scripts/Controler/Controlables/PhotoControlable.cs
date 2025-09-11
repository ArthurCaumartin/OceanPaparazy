using System;
using UnityEngine;

[Serializable]
public class PhotoControlable : PlayerControlable
{
    //TODO set la position target pour le DroneBehavior, pour ne plus set la position du drone directement
    [SerializeField] private Rigidbody _droneRb;
    [SerializeField] private DroneBehavior _droneBehavior;
    [SerializeField] private Transform _cameraPivot;
    [Space]
    [Header("Movement and Look Settings : ")]
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    [Space]
    [SerializeField] private float _zoomSpeed;
    [Space]
    [SerializeField] private float _droneDistance = 2;
    [SerializeField] private float _droneFollowSpeed = 2;
    private float _currentAngleX;
    private bool _isZooming;
    private bool _hasZoomed;



    public override void EnterControler()
    {
        cameraStateMachine.SetState(cameraStateMachine.CameraStateFirstPerson, _cameraPivot);
        _droneRb.transform.parent = null;
    }

    public override void Initialize(Transform transform)
    {
        base.Initialize(transform);
    }

    public override void FixedUpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        float xAngle = _currentAngleX + lookDelta.y * _lookSensivity * Time.fixedDeltaTime;
        _currentAngleX = Mathf.Clamp(xAngle, -_maxLookAngleX, _maxLookAngleX);

        transform.Rotate(Vector3.up * lookDelta.x * _lookSensivity * Time.fixedDeltaTime);
        _cameraPivot.localRotation = Quaternion.Euler(-_currentAngleX, 0, 0);

        SetDronePosition();
        SetDroneZoom();
    }

    private void SetDroneZoom()
    {
        if (_isZooming)
            _droneBehavior.Zoom(_zoomSpeed * Time.fixedDeltaTime * (_hasZoomed ? -1 : 1));
    }

    private void SetDronePosition()
    {
        Vector3 cameraPosition = Vector3.zero;
        Quaternion cameraRotation = Quaternion.identity;
        cameraStateMachine.OutCameraPositionAndRotation(out cameraPosition, out cameraRotation);

        Vector3 droneTargetPosition = cameraPosition + cameraRotation * Vector3.forward * _droneDistance;
        _droneRb.position = Vector3.Lerp(_droneRb.position, droneTargetPosition, _droneFollowSpeed * Time.fixedDeltaTime);
        _droneRb.rotation = Quaternion.Lerp(_droneRb.rotation, cameraRotation, _droneFollowSpeed * Time.fixedDeltaTime);
    }

    public override void ExitControler()
    {
        _droneRb.transform.parent = transform;
    }

    public override void FirstAbility(bool isPressed)
    {
        base.FirstAbility(isPressed);
        _isZooming = isPressed;
        if (isPressed)
            _hasZoomed = !_hasZoomed;
    }

    public override void SecondAbility(bool isPressed)
    {
        base.SecondAbility(isPressed);
        if (isPressed)
            _droneBehavior.TakePhoto();
    }
}