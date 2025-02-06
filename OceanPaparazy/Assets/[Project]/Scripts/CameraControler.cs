using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private float _acceleration = 5;
    [SerializeField] private float _speed = 5;
    private Vector2 _inputVector;
    private Vector3 _lookRotation;

    private void Update()
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
}
