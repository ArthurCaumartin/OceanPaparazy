using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private CameraSettings _settings;

    /// fps (remote control)
    /// tps (local control with remote effect)

    private void Awake()
    {
        _settings = CameraSettings.Default;
    }

    public void SetControler(Transform target, CameraSettings settings)
    {
        _target = target;
        _settings = settings;
    }

    public void SendInput(Vector2 movementDirection, Vector2 lookDelta)
    {

    }

    public void UpdateCamera()
    {
        if (_target == null) return;

        Vector3 targetPosition = _target.position + _settings.targetPositionOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _settings.speed * Time.deltaTime);

    }
}

[Serializable]
public struct CameraSettings
{
    public float speed;
    public float distance;
    public Vector3 targetPositionOffset;
    public float rotationSpeed;
    public bool listenInput;

    public CameraSettings(float speed, float distance, Vector3 targetPositionOffset, float rotationSpeed, bool listenInput)
    {
        this.speed = speed;
        this.distance = distance;
        this.targetPositionOffset = targetPositionOffset;
        this.rotationSpeed = rotationSpeed;
        this.listenInput = listenInput;
    }

    public static CameraSettings Default => new CameraSettings(5f, 5f, Vector3.zero, 5f, true);
}
