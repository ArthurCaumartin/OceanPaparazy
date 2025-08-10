using System;
using UnityEngine;

[Serializable]
public abstract class CameraState
{
    protected Transform target;
    protected Camera camera;

    public virtual void Initialize(Camera camera)
    {
        this.camera = camera;
    }

    public virtual void Enter(Transform redefineTarget = null)
    {
        target = redefineTarget;
    }
    public virtual void Exit() { }
    public abstract void UpdateState(Vector2 movementInput, Vector2 lookInput);
}


