using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Splines;

[Serializable]
public class SplineControlable : PlayerControlable
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _movementAcceleration = 2f;
    [Space]
    [SerializeField] private Transform _meshTransform;
    private float _currentSplineTime = 0.5f;
    private float _dynamiqueSpeedX;
    private float _dynamiqueSpeedY;
    private float _currentAltitude;
    private SplineContainer _splineContainer;

    public override void EnterControler()
    {
        GoOnNearSplinePoint(transform, _splineContainer);

        SetToSplinePosition(transform, _currentSplineTime, _currentAltitude);
        SetToSplineRotation(transform, _currentSplineTime);
    }

    public void SetSplineContainer(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer;
    }

    public override void UpdateControler(Vector2 inputDirection, Vector2 lookDelta)
    {
        Debug.Log($"SplineControlable UpdateControler | dir : {inputDirection} | look : {lookDelta}");

        _dynamiqueSpeedX = GetDynamicSpeed(_dynamiqueSpeedX, inputDirection.x, _movementAcceleration);
        _dynamiqueSpeedY = GetDynamicSpeed(_dynamiqueSpeedY, inputDirection.y, _movementAcceleration);

        _currentAltitude += Time.deltaTime * _dynamiqueSpeedY;
        _currentAltitude = Mathf.Clamp(_currentAltitude, 1.5f, 15);

        if (_dynamiqueSpeedX != 0)
            _currentSplineTime += _dynamiqueSpeedX * Time.deltaTime / _splineContainer[0].GetLength();

        if (_currentSplineTime > 1) _currentSplineTime = 0;
        if (_currentSplineTime < 0) _currentSplineTime = 1;

        SetToSplinePosition(transform, _currentSplineTime, _currentAltitude);
        SetToSplineRotation(transform, _currentSplineTime);
        AnimateMesh(_dynamiqueSpeedX, _movementSpeed);
    }

    public override void ExitControler()
    {
        _dynamiqueSpeedX = 0;
        _dynamiqueSpeedY = 0;
    }

    public override void AbilitySecond()
    {
        throw new System.NotImplementedException();
    }

    public override void AbilityFisrt()
    {
        throw new System.NotImplementedException();
    }

    private void SetToSplinePosition(Transform transformToPlace, float time, float altitude = 0)
    {
        float3 splinePos;
        float3 splineUp;
        float3 splineTangent;
        _splineContainer[0].Evaluate(time, out splinePos, out splineTangent, out splineUp);

        Vector3 splineWorldPos = _splineContainer.transform.TransformPoint(splinePos);
        transformToPlace.position = splineWorldPos + (Vector3.up * altitude);
    }

    private void SetToSplineRotation(Transform transformToRotate, float time)
    {
        float3 splinePos;
        float3 splineUp;
        float3 splineTangent;
        _splineContainer[0].Evaluate(time, out splinePos, out splineTangent, out splineUp);

        Quaternion lookDirection = Quaternion.LookRotation(Vector3.Cross(splineTangent, splineUp), splineUp);
        transformToRotate.rotation = lookDirection;
    }

    private float GetDynamicSpeed(float current, float target, float acceleration)
    {
        return Mathf.Lerp(current, target * _movementSpeed, Time.deltaTime * acceleration);
    }

    private void GoOnNearSplinePoint(Transform transform, SplineContainer splineContainer)
    {
        float3 nearPoint = 0;
        Vector3 playerSplineLocalPos = splineContainer.transform.InverseTransformPoint(transform.position);
        // print(playerSplineLocalPos);
        SplineUtility.GetNearestPoint(splineContainer[0], playerSplineLocalPos, out nearPoint, out _currentSplineTime);
        nearPoint = splineContainer.transform.TransformPoint(nearPoint);
        transform.position = new Vector3(nearPoint.x, transform.position.y, nearPoint.z);
    }

    public void AnimateMesh(float speedX, float maxSpeed)
    {
        float time = Mathf.InverseLerp(-maxSpeed, maxSpeed, speedX);
        float angle = Mathf.Lerp(50, -50, time);
        _meshTransform.eulerAngles = new Vector3(_meshTransform.eulerAngles.x, _meshTransform.eulerAngles.y, angle);
    }
}