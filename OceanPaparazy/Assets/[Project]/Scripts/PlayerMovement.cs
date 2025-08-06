using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string _debug_state_name;
    [SerializeField] private Camera _camera;
    [SerializeField] private SplineContainer _splineContainer;

    [Header("Controlers : ")]
    [SerializeField] private SplineControlable _splineControler = new SplineControlable();
    [SerializeField] private PhotoControlable _photoControler = new PhotoControlable();
    [SerializeField] private DroneControlable _droneControler = new DroneControlable();

    public SplineControlable SplineControler { get => _splineControler; }
    public PhotoControlable PhotoControler { get => _photoControler; }
    public DroneControlable DroneControler { get => _droneControler; }
    private PlayerControlable _currentControler;

    private Vector2 _inputDirection;
    private Vector2 _lookDelta;

    private void Start()
    {
        _splineControler.Initialize(transform);
        _splineControler.SetSplineContainer(_splineContainer);

        _photoControler.Initialize(transform);
        _droneControler.Initialize(transform);

        SetControler(SplineControler);
    }

    private void Update()
    {
        _currentControler.UpdateControler(_inputDirection, _lookDelta);
    }

    public void SetControler(PlayerControlable toSet)
    {
        if (toSet == _currentControler) return;

        _currentControler?.ExitControler();
        _currentControler = toSet;
        _debug_state_name = _currentControler.ToString();
        _currentControler.EnterControler();

    }

    private void OnSwapControler()
    {
        if (_currentControler is SplineControlable)
        {
            SetControler(PhotoControler);
            return;
        }

        if (_currentControler is DroneControlable)
        {
            SetControler(SplineControler);
            return;
        }

        if (_currentControler is PhotoControlable)
        {
            SetControler(DroneControler);
            return;
        }
    }

    private void OnMove(Vector2 value)
    {
        _inputDirection = value;
    }

    private void OnLook(Vector2 value)
    {
        _lookDelta = value;
    }

    private void OnFirstAbility()
    {
        _currentControler?.FirstAbility();
    }

    private void OnSecondAbility()
    {
        _currentControler?.SecondAbility();
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
