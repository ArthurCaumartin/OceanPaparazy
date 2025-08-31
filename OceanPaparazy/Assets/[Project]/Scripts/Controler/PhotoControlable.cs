using System;
using UnityEngine;

[Serializable]
public class PhotoControlable : PlayerControlable
{
    [SerializeField] private Rigidbody _droneRb;
    [SerializeField] private Transform _cameraPivot;
    [Space]
    [Header("Movement and Look Settings : ")]
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    [SerializeField] private float _droneDistance = 2;
    [SerializeField] private float _droneFollowSpeed = 2;
    private float _currentAngleX;


    private PhotoCameraDetector _photoCameraDetector;

    public override void EnterControler()
    {
        cameraStateMachine.SetState(cameraStateMachine.CameraStateFirstPerson, _cameraPivot);
        _droneRb.transform.parent = null;
    }

    public override void Initialize(Transform transform)
    {
        base.Initialize(transform);
        _photoCameraDetector = cameraStateMachine.GetComponent<PhotoCameraDetector>();

    }

    public override void FixedUpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        float xAngle = _currentAngleX + lookDelta.y * _lookSensivity * Time.fixedDeltaTime;
        _currentAngleX = Mathf.Clamp(xAngle, -_maxLookAngleX, _maxLookAngleX);

        transform.Rotate(Vector3.up * lookDelta.x * _lookSensivity * Time.fixedDeltaTime);
        _cameraPivot.localRotation = Quaternion.Euler(-_currentAngleX, 0, 0);

        SetDronePosition();
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
        Debug.Log("DroneControlable: FirstAbility | Zoom : " + isPressed);
        base.FirstAbility(isPressed);

    }

    public override void SecondAbility(bool isPressed)
    {
        _photoCameraDetector.TakePhoto();
        _photoCameraDetector.PrintPhotos();
    }
}