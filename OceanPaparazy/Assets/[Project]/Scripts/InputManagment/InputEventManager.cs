using UnityEngine;
using UnityEngine.InputSystem;

public class InputEventManager : MonoBehaviour
{
    public delegate void VectorEvent(Vector2 inputDirection);
    public static event VectorEvent OnMoveEvent;
    public static event VectorEvent OnLookEvent;

    public delegate void ButtonEvent(bool isPressed);
    public static event ButtonEvent OnSwapControlerEvent;
    public static event ButtonEvent OnAbilityFirstEvent;
    public static event ButtonEvent OnAbilitySecondEvent;


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
        bool isPressed = value.Get<float>() > 0.5f;
        OnAbilityFirstEvent?.Invoke(isPressed);
    }

    private void OnSecondAbility(InputValue value)
    {
        bool isPressed = value.Get<float>() > 0.5f;
        OnAbilitySecondEvent?.Invoke(isPressed);
    }

    private void OnSwapControler(InputValue value)
    {
        bool isPressed = value.Get<float>() > 0.5f;
        OnSwapControlerEvent.Invoke(isPressed);
    }
}