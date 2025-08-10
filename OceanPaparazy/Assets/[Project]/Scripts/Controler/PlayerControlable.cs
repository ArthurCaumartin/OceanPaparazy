using System;
using Alchemy.Inspector;
using UnityEngine;

/// <summary>
/// Base class for player controlable behaviors.
/// This class should be instantiateable and used in a class that will manage swap between controler like PlayerMovement.

[Serializable]
[BoxGroup]
public abstract class PlayerControlable
{
    protected Transform transform;
    protected CameraStateMachine cameraStateMachine;

    public virtual void Initialize(Transform transform)
    {
        this.transform = transform;
        if (Camera.main != null) this.cameraStateMachine = Camera.main.GetComponent<CameraStateMachine>();
    }

    public virtual void EnterControler() { }
    public virtual void UpdateControler(Vector2 inputDirection, Vector2 lookDelta) { }
    public virtual void FixedUpdateControler(Vector2 inputDirection, Vector2 lookDelta) { }
    public virtual void ExitControler() { }

    public virtual void SecondAbility() { }
    public virtual void FirstAbility() { }

}
