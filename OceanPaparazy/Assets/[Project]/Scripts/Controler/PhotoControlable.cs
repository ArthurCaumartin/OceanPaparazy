using System;
using UnityEngine;

[Serializable]
public class PhotoControlable : PlayerControlable
{
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    [SerializeField] private PhotoCameraDetector _photoCameraDetector;
    private float _currentAngleX;

    public override void EnterControler()
    {
        cameraControler.SetControler(transform, CameraSettings.PhotoDefault);
    }

    public override void Initialize(Transform transform)
    {
        base.Initialize(transform);
        
        _photoCameraDetector = cameraControler.GetComponent<PhotoCameraDetector>();
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

    public override void FirstAbility()
    {
        _photoCameraDetector.TakePhoto();
    }

    public override void SecondAbility()
    {

    }

}