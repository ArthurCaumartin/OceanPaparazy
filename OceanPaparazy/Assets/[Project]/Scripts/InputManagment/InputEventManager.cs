using UnityEngine;
using UnityEngine.InputSystem;

public class InputEventManager : MonoBehaviour
{
    public delegate void SwapControlerHandler();
    public static event SwapControlerHandler OnSwapControlerEvent;

    public delegate void MoveHandler(Vector2 inputDirection);
    public static event MoveHandler OnMoveEvent;

    public delegate void LookHandler(Vector2 lookDelta);
    public static event LookHandler OnLookEvent;

    public delegate void AbilityFirstHandler();
    public static event AbilityFirstHandler OnAbilityFirstEvent;

    public delegate void AbilitySecondHandler();
    public static event AbilitySecondHandler OnAbilitySecondEvent;

    private void OnSwapControler(InputValue value)
    {
        if (OnSwapControlerEvent != null && value.Get<float>() > 0.5f)
            OnSwapControlerEvent.Invoke();
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDirection = value.Get<Vector2>();
        OnMoveEvent?.Invoke(inputDirection);
    }

    private void OnLook(InputValue value)
    {
        Vector2 lookDelta = value.Get<Vector2>();
        OnLookEvent?.Invoke(lookDelta);
    }

    private void OnFirstAbility(InputValue value)
    {
        OnAbilityFirstEvent?.Invoke();
    }

    private void OnSecondAbility(InputValue value)
    {
        OnAbilitySecondEvent?.Invoke();
    }
}