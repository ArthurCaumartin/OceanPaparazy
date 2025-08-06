using System;
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

    public override void EnterControler()
    {
        cameraControler.SetControler(_droneTransform, CameraSettings.PhotoDefault);
    }

    public override void UpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y) * _movementSpeed * Time.deltaTime;
        _droneTransform.Translate(moveDirection, Space.Self);

        _currentAngleX += lookDelta.y * _lookSensivity * Time.deltaTime;
        _currentAngleX = Mathf.Clamp(_currentAngleX, -_maxLookAngleX, _maxLookAngleX);
        cameraControler.SetXAngleOffset(_currentAngleX);
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


