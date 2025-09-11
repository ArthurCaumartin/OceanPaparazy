using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] protected string debugStateName;
    protected State currentState;

    public void SetState(State toSet)
    {
        if (toSet == currentState) return;
        currentState?.ExitState();
        currentState = toSet;
        debugStateName = currentState.ToString();
        currentState.EnterState();
    }

    protected virtual void Update()
    {
        currentState?.UpdateState();
    }

    protected virtual void FixedUpdate()
    {
        currentState?.FixedUpdateState();
    }
}
