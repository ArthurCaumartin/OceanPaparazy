using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementAcceleration;

    [Header("Visual")]
    [SerializeField] private Transform _meshTransform;

    private Vector3 _inputDirection;
    private float _currentSplineTime = 0.5f;
    private float _dynamiqueSpeedX;
    private float _dynamiqueSpeedY;
    private float _currentAltitude;

    private void Start()
    {
        GoOnNearSplinePoint();
    }

    private void Update()
    {
        _dynamiqueSpeedX = GetDynamicSpeed(_dynamiqueSpeedX, _inputDirection.x, _movementAcceleration);
        _dynamiqueSpeedY = GetDynamicSpeed(_dynamiqueSpeedY, _inputDirection.y, _movementAcceleration);

        _currentAltitude += Time.deltaTime * _dynamiqueSpeedY;
        _currentAltitude = Mathf.Clamp(_currentAltitude, 1.5f, 15);

        if (_dynamiqueSpeedX != 0)
            _currentSplineTime += _dynamiqueSpeedX * Time.deltaTime / _splineContainer[0].GetLength();

        if (_currentSplineTime > 1) _currentSplineTime = 0;
        if (_currentSplineTime < 0) _currentSplineTime = 1;

        SetToSplinePosition(transform, _currentSplineTime, _currentAltitude);
        AnimateMesh(_dynamiqueSpeedX, _movementSpeed);
    }


    private void OnMove(InputValue value)
    {
        _inputDirection = value.Get<Vector2>();
    }

    public void SetToSplinePosition(Transform transformToPlace, float time, float altitude = 0)
    {
        float3 splinePos;
        float3 splineUp;
        float3 splineTangent;
        _splineContainer[0].Evaluate(time, out splinePos, out splineTangent, out splineUp);

        Vector3 splineWorldPos = _splineContainer.transform.TransformPoint(splinePos);
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.Cross(splineTangent, splineUp), splineUp);

        print($"Spline Pos To Set at {time} time : {splineWorldPos}");
        transformToPlace.position = splineWorldPos + (Vector3.up * altitude);
        transformToPlace.rotation = lookDirection;

    }

    public float GetDynamicSpeed(float current, float target, float acceleration)
    {
        return Mathf.Lerp(current, target * _movementSpeed, Time.deltaTime * acceleration);
    }

    private void GoOnNearSplinePoint()
    {
        float3 nearPoint = 0;
        Vector3 playerSplineLocalPos = _splineContainer.transform.InverseTransformPoint(transform.position);
        print(playerSplineLocalPos);
        SplineUtility.GetNearestPoint(_splineContainer[0], playerSplineLocalPos, out nearPoint, out _currentSplineTime);
        transform.position = new Vector3(nearPoint.x, transform.position.y, nearPoint.z);
    }

    public void AnimateMesh(float speedX, float maxSpeed)
    {
        float time = Mathf.InverseLerp(-maxSpeed, maxSpeed, speedX);
        float angle = Mathf.Lerp(50, -50, time);
        _meshTransform.eulerAngles = new Vector3(_meshTransform.eulerAngles.x, _meshTransform.eulerAngles.y, angle);
    }
}
