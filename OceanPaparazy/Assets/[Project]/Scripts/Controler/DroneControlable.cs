using System;
using Alchemy.Inspector;
using UnityEngine;

[Serializable]
public class DroneControlable : PlayerControlable
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _movementAcceleration = 2f;
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    [SerializeField] private Transform _droneTransform;
    private float _currentAngleX;
    private Rigidbody _rigidbody;

    public override void Initialize(Transform transform)
    {
        base.Initialize(transform);
        _rigidbody = _droneTransform.GetComponent<Rigidbody>();
    }

    public override void EnterControler()
    {
        cameraControler.SetControler(_droneTransform, CameraSettings.DroneDefault);

        Vector3 newForward = _droneTransform.forward;
        newForward.y = 0;
        _droneTransform.rotation = Quaternion.LookRotation(newForward, Vector3.up);

        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.linearVelocity = Vector3.zero;
    }

    public override void UpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y) * _movementSpeed * Time.deltaTime;
        _droneTransform.Translate(moveDirection, Space.Self);

        _currentAngleX += lookDelta.y * _lookSensivity * Time.deltaTime;
        _currentAngleX = Mathf.Clamp(_currentAngleX, -_maxLookAngleX, _maxLookAngleX);
        // cameraControler.SetXAngleOffset(_currentAngleX);
        // _droneTransform.eulerAngles = new Vector3(_currentAngleX, _droneTransform.eulerAngles.y + lookDelta.x * _lookSensivity * Time.deltaTime, 0);
        _droneTransform.Rotate(new Vector3(0, lookDelta.x * _lookSensivity * Time.deltaTime));
    }

    public override void ExitControler()
    {

    }

    public override void SecondAbility()
    {

    }

    public override void FirstAbility()
    {

    }
}


