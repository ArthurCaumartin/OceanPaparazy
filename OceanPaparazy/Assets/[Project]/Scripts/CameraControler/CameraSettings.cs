using System;
using UnityEngine;

[Serializable]
public struct CameraSettings
{
    public float distance;
    public float inputSensitivity;
    public Vector3 targetPositionOffset;
    public Vector3 cameraPositionOffset;
    public bool isFirstPerson;

    public CameraSettings(float distance
                        , float inputSensitivity
                        , Vector3 targetPositionOffset
                        , Vector3 cameraPositionOffset
                        , bool isFirstPerson)
    {
        this.distance = distance;
        this.inputSensitivity = inputSensitivity;

        this.targetPositionOffset = targetPositionOffset;
        this.cameraPositionOffset = cameraPositionOffset;

        this.isFirstPerson = isFirstPerson;
    }

    public static CameraSettings SplineDefault => new CameraSettings(
        20f,
        1f,
        new Vector3(0, 1f, 0),
        new Vector3(0, 5f, 0),
        false);
    public static CameraSettings PhotoDefault => new CameraSettings(
        0f,
        1f,
        new Vector3(0, 0f, 0),
        new Vector3(0, 1.5f, 0),
        true);
    public static CameraSettings DroneDefault => new CameraSettings(
        0f,
        1f,
        new Vector3(0, 0f, 0),
        new Vector3(0, 0f, 0),
        true);
}
