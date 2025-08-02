using System;
using UnityEngine;

/// <summary>
/// Base class for player controlable behaviors.
/// This class should be instantiateable and used in a class that will manage swap between controler like PlayerMovement.

[Serializable]
public abstract class PlayerControlable
{
    protected Transform transform;
    protected CameraControler cameraControler;
    protected CameraControler CameraControler => cameraControler;

    public virtual void Initialize(Transform transform)
    {
        this.transform = transform;
        this.cameraControler = Camera.main.GetComponent<CameraControler>();
    }

    public abstract void EnterControler();
    public abstract void UpdateControler(Vector2 inputDirection, Vector2 lookDelta);
    public abstract void ExitControler();

    public abstract void AbilitySecond();
    public abstract void AbilityFisrt();
}
