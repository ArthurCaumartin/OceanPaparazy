using System;
using UnityEngine;

[Serializable]
public class PhotoControlable : PlayerControlable
{
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    [SerializeField] private float _droneDistance = 2;
    [SerializeField] private float _droneFollowSpeed = 2;
    [SerializeField] private Transform _droneTransorm;
    private PhotoCameraDetector _photoCameraDetector;
    private float _currentAngleX;

    public override void EnterControler()
    {
        cameraControler.SetControler(transform, CameraSettings.PhotoDefault);
        _droneTransorm.parent = null;
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

        Vector3 dronePosTarget = cameraControler.transform.position + (cameraControler.transform.forward * _droneDistance);
        _droneTransorm.position = Vector3.Lerp(_droneTransorm.position, dronePosTarget, Time.deltaTime * _droneFollowSpeed);
        _droneTransorm.rotation = Quaternion.Slerp(_droneTransorm.rotation, cameraControler.transform.rotation, Time.deltaTime * _droneFollowSpeed);
    }

    public override void ExitControler()
    {
        _droneTransorm.parent = transform;
    }

    public override void FirstAbility()
    {
        _photoCameraDetector.TakePhoto();
    }

    public override void SecondAbility()
    {
        _photoCameraDetector.PrintPhotos();
    }

}