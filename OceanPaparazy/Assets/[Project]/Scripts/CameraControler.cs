using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [Header("Third Persone Parameter : ")]
    [SerializeField] private float _followSpeed = 5;
    [SerializeField] private float _followDistance = 5;

    [Header("First Persone Parameter : ")]
    [SerializeField] private float _acceleration = 5;
    [SerializeField] private float _speed = 5;
    private Vector2 _inputVector;
    private Vector3 _lookRotation;
    private bool _isFirstPersone = false;


    private void Update()
    {
        if (_isFirstPersone)
            LookAround();
        else
            FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 posTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z - _followDistance);
        transform.position = Vector3.Lerp(transform.position, posTarget, Time.deltaTime * _followSpeed);
        transform.LookAt(_target);
    }

    private void LookAround()
    {
        _lookRotation.x += -_inputVector.y * _speed;
        _lookRotation.x = Mathf.Clamp(_lookRotation.x, -90, 90);
        _lookRotation.y += _inputVector.x * _speed;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_lookRotation), Time.deltaTime * _acceleration);
    }

    private void OnLook(InputValue value)
    {
        _inputVector = value.Get<Vector2>();
    }

    public void EnableFirstPersone(bool value)
    {
        _isFirstPersone = value;
    }
}
