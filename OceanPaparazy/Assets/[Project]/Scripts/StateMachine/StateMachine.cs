using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State _currentState;

    public void SetState(State toSet)
    {
        if (toSet == _currentState) return;
        _currentState?.ExitState();
        _currentState = toSet;
        _currentState.EnterState();
    }

    public virtual void Update()
    {
        _currentState?.UpdateState();
    }

    public virtual void FixedUpdate()
    {
        _currentState?.FixedUpdateState();
    }
}
