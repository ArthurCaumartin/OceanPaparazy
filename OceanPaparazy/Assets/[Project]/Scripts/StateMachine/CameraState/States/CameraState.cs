using System;
using UnityEngine;

[Serializable]
public abstract class CameraState : State
{
    protected Transform target;
    protected Camera camera;

    public void Init(StateMachine stateMachine, Camera camera, Transform target)
    {
        base.Init(stateMachine);
        this.camera = camera;
        this.target = target;
    }

    public abstract void UpdateStateInput(Vector2 movementInput, Vector2 lookInput);

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public CameraState() : base() { }
    public CameraState(StateMachine stateMachine, Camera camera, Transform target) : base(stateMachine)
    {
        this.camera = camera;
        this.target = target;
    }
}


