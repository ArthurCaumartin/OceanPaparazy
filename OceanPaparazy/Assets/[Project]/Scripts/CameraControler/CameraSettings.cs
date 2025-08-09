using System;
using UnityEngine;

[Serializable]
public struct CameraSettings
{
    public float distance;
    public float inputSensitivity;
    public float followSpeed;
    public Vector3 targetPositionOffset;
    public Vector3 cameraPositionOffset;
    public bool isFirstPerson;
    public bool followTargetForwardVertical;

    public CameraSettings(float distance
                        , float inputSensitivity
                        , float followSpeed
                        , Vector3 targetPositionOffset
                        , Vector3 cameraPositionOffset
                        , bool isFirstPerson
                        , bool followTargetForwardVertical)
    {
        this.distance = distance;
        this.inputSensitivity = inputSensitivity;
        this.followSpeed = followSpeed;

        this.targetPositionOffset = targetPositionOffset;
        this.cameraPositionOffset = cameraPositionOffset;

        this.isFirstPerson = isFirstPerson;
        this.followTargetForwardVertical = followTargetForwardVertical;
    }

    public static CameraSettings SplineDefault => new CameraSettings(
        20f,
        1f,
        10f,
        new Vector3(0, 1f, 0),
        new Vector3(0, 5f, 0),
        false,
        false);
    public static CameraSettings PhotoDefault => new CameraSettings(
        0f,
        1f,
        100f,
        new Vector3(0, 0f, 0),
        new Vector3(0, 1.5f, 0),
        true,
        false);
    public static CameraSettings DroneDefault => new CameraSettings(
        0f,
        1f,
        100f,
        new Vector3(0, 0f, 0),
        new Vector3(0, 0f, 0),
        true,
        true);
}
