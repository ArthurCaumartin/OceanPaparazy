using System;
using UnityEngine;

[Serializable]
public class CameraStateFirstPerson : CameraState
{

    public override void UpdateStateInput(Vector2 movementInput, Vector2 lookInput)
    {
        camera.transform.position = target.position;
        camera.transform.rotation = target.rotation;
    }

    public CameraStateFirstPerson() : base() { }
    public CameraStateFirstPerson(StateMachine stateMachine, Camera camera, Transform target) : base(stateMachine, camera, target) { }
}


