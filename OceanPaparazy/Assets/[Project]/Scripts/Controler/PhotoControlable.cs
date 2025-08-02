
using System;
using UnityEngine;

[Serializable]
public class PhotoControlable : PlayerControlable
{
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    private float _currentAngleX;

    public override void EnterControler()
    {
        cameraControler.SetControler(transform, CameraSettings.PhotoDefault);
    }

    public override void UpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        _currentAngleX += lookDelta.y * _lookSensivity * Time.deltaTime;
        _currentAngleX = Mathf.Clamp(_currentAngleX, -_maxLookAngleX, _maxLookAngleX);
        cameraControler.SetXAngleOffset(_currentAngleX);
        transform.Rotate(new Vector3(0, lookDelta.x * _lookSensivity * Time.deltaTime));
    }

    public override void ExitControler()
    {

    }

    public override void AbilityFisrt()
    {

    }

    public override void AbilitySecond()
    {

    }

}