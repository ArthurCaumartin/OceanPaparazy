using UnityEngine;

public class StateMachine_Drone : StateMachine
{
    [SerializeField] private State_DroneIdle DroneStateIdle;
    [SerializeField] private State_DroneFollow DroneStateFollow;

    public StateMachine_Drone()
    {
        DroneStateIdle = new State_DroneIdle(this);
        DroneStateFollow = new State_DroneFollow(this);

        SetState(DroneStateIdle);
    }
}
