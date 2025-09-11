using System;

[Serializable]
public abstract class State
{
    protected StateMachine _stateMachine;
    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }

    public State(StateMachine stateMachine = null)
    {
        _stateMachine = stateMachine;
    }
}

