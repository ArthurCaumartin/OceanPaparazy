using System;
using UnityEngine;

[Serializable]
public class CameraStateSpline : CameraState
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Vector3 positionOffSet;
    [SerializeField] private Vector3 lookOffSet;

    public override void UpdateStateInput(Vector2 movementInput, Vector2 lookInput)
    {
        Vector3 pos = target.TransformPoint(positionOffSet);
        camera.transform.position = Vector3.Lerp(camera.transform.position, pos, _movementSpeed * Time.deltaTime);

        Vector3 lookDirection = ((target.position + lookOffSet) - camera.transform.position).normalized;
        camera.transform.forward = Vector3.Lerp(camera.transform.forward, lookDirection, _movementSpeed * Time.deltaTime);
    }

    public CameraStateSpline() : base() { }
    public CameraStateSpline(StateMachine stateMachine, Camera camera, Transform target) : base(stateMachine, camera, target) { }
}



