
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public abstract class Controler
{
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float movementAcceleration;
    public abstract void EnterControler();
    public abstract void UpdateControler(Vector2 inputDirection);
    public abstract void ExitControler();
}


public class SplineControler : Controler
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementAcceleration;
    private float _currentSplineTime = 0.5f;
    private float _dynamiqueSpeedX;
    private float _dynamiqueSpeedY;
    private float _currentAltitude;
    private SplineContainer _splineContainer;
    private Transform _transform;


    public void Initialize(Transform transform, SplineContainer splineContainer)
    {
        _transform = transform;
        _splineContainer = splineContainer;
    }

    public override void EnterControler()
    {

    }

    public override void UpdateControler(Vector2 inputDirection)
    {
        _dynamiqueSpeedX = GetDynamicSpeed(_dynamiqueSpeedX, inputDirection.x, _movementAcceleration);
        _dynamiqueSpeedY = GetDynamicSpeed(_dynamiqueSpeedY, inputDirection.y, _movementAcceleration);

        _currentAltitude += Time.deltaTime * _dynamiqueSpeedY;
        _currentAltitude = Mathf.Clamp(_currentAltitude, 1.5f, 15);

        if (_dynamiqueSpeedX != 0)
            _currentSplineTime += _dynamiqueSpeedX * Time.deltaTime / _splineContainer[0].GetLength();

        if (_currentSplineTime > 1) _currentSplineTime = 0;
        if (_currentSplineTime < 0) _currentSplineTime = 1;

        SetToSplinePosition(_transform, _currentSplineTime, _currentAltitude);
        // AnimateMesh(_dynamiqueSpeedX, _movementSpeed);
    }

    public override void ExitControler()
    {

    }

    private void SetToSplinePosition(Transform transformToPlace, float time, float altitude = 0)
    {
        float3 splinePos;
        float3 splineUp;
        float3 splineTangent;
        _splineContainer[0].Evaluate(time, out splinePos, out splineTangent, out splineUp);

        Vector3 splineWorldPos = _splineContainer.transform.TransformPoint(splinePos);
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.Cross(splineTangent, splineUp), splineUp);

        // print($"Spline Pos To Set at {time} time : {splineWorldPos}");
        transformToPlace.position = splineWorldPos + (Vector3.up * altitude);
        transformToPlace.rotation = lookDirection;
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
        transform.position = new Vector3(nearPoint.x, transform.position.y, nearPoint.z);
    }
}