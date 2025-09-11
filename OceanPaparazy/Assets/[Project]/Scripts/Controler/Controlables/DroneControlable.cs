using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[Serializable]
public class DroneControlable : PlayerControlable
{
    [SerializeField] private Rigidbody _droneRigidbody;
    [SerializeField] private Camera _photoCamera;

    [Header("Movement and Look Settings : ")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _movementAcceleration = 2f;
    [SerializeField] private float _lookSensivity = 2f;
    [SerializeField] private float _maxLookAngleX = 80f;
    private float _currentAngleX;

    public override void Initialize(Transform transform)
    {
        base.Initialize(transform);
    }

    public override void EnterControler()
    {
        Debug.Log("DroneControlable: EnterControler | Set camera target to : " + _droneRigidbody.transform.name);
        cameraStateMachine.SetCameraState(cameraStateMachine.CameraStateFirstPerson, _droneRigidbody.transform);

        Vector3 newForward = _droneRigidbody.transform.forward;
        newForward.y = 0;
        _droneRigidbody.rotation = Quaternion.LookRotation(newForward, Vector3.up);

        _droneRigidbody.angularVelocity = Vector3.zero;
        _droneRigidbody.linearVelocity = Vector3.zero;
    }

    public override void FixedUpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        MoveAndRotate(inputDirection, lookDelta);
    }
 

    private void MoveAndRotate(Vector2 inputDirection, Vector2 lookDelta)
    {
        // move with velocity
        Vector3 targetVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * _movementSpeed * Time.deltaTime;
        targetVelocity = _droneRigidbody.rotation * targetVelocity;
        _droneRigidbody.linearVelocity = Vector3.Lerp(_droneRigidbody.linearVelocity
                                                    , targetVelocity
                                                    , _movementAcceleration * Time.deltaTime);

        // rotate
        _currentAngleX += lookDelta.y * _lookSensivity * Time.deltaTime;
        _currentAngleX = Mathf.Clamp(_currentAngleX, -_maxLookAngleX, _maxLookAngleX);
        _droneRigidbody.rotation = Quaternion.Euler(-_currentAngleX
                                                    , _droneRigidbody.rotation.eulerAngles.y + lookDelta.x * _lookSensivity * Time.deltaTime
                                                    , 0);
        _droneRigidbody.angularVelocity = Vector3.zero;
    }
}


