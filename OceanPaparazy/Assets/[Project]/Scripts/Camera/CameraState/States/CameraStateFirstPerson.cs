using System;
using UnityEngine;

[Serializable]
public class CameraStateFirstPerson : CameraState
{
    public override void UpdateState(Vector2 movementInput, Vector2 lookInput)
    {
        camera.transform.position = target.position;
        camera.transform.rotation = target.rotation;
    }
}


