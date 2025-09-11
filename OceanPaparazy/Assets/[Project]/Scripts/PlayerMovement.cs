using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string _debug_state_name;

    [Header("Controlers : ")]
    [SerializeField] private SplineControlable _splineControler = new SplineControlable();
    [SerializeField] private PhotoControlable _photoControler = new PhotoControlable();

    public SplineControlable SplineControler { get => _splineControler; }
    public PhotoControlable PhotoControler { get => _photoControler; }
    private PlayerControlable _currentControler;

    private Vector2 _inputDirection;
    private Vector2 _lookDelta;

    private void Start()
    {
        _splineControler.Initialize(transform);
        _photoControler.Initialize(transform);

        SetControler(SplineControler);
    }

    private void Update()
    {
        _currentControler?.UpdateControler(_inputDirection, _lookDelta);
    }

    private void FixedUpdate()
    {
        _currentControler?.FixedUpdateControler(_inputDirection, _lookDelta);
    }

    public void SetControler(PlayerControlable toSet)
    {
        if (toSet == _currentControler) return;

        _currentControler?.ExitControler();
        _currentControler = toSet;
        _debug_state_name = _currentControler.ToString();
        _currentControler.EnterControler();

    }

    private void OnSwapControler(bool isPressed)
    {
        if (!isPressed) return;
        SetControler(_currentControler is SplineControlable ? PhotoControler : SplineControler);
    }

    private void OnMove(Vector2 value)
    {
        _inputDirection = value;
    }

    private void OnLook(Vector2 value)
    {
        _lookDelta = value;
    }

    private void OnFirstAbility(bool isPressed)
    {
        _currentControler?.FirstAbility(isPressed);
    }

    private void OnSecondAbility(bool isPressed)
    {
        _currentControler?.SecondAbility(isPressed);
    }

    private void OnEnable()
    {
        InputEventManager.OnSwapControlerEvent += OnSwapControler;
        InputEventManager.OnMoveEvent += OnMove;
        InputEventManager.OnLookEvent += OnLook;
        InputEventManager.OnAbilityFirstEvent += OnFirstAbility;
        InputEventManager.OnAbilitySecondEvent += OnSecondAbility;
    }

    private void OnDisable()
    {
        InputEventManager.OnSwapControlerEvent -= OnSwapControler;
        InputEventManager.OnMoveEvent -= OnMove;
        InputEventManager.OnLookEvent -= OnLook;
        InputEventManager.OnAbilityFirstEvent -= OnFirstAbility;
        InputEventManager.OnAbilitySecondEvent -= OnSecondAbility;
    }
}
