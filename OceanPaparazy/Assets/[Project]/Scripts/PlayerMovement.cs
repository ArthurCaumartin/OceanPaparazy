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

    public SplineControlable SplineControler { get => _splineControler; }
    public PhotoControlable PhotoControler { get => _photoControler; }
    private PlayerControlable _currentControler;

    private Vector2 _inputDirection;
    private Vector2 _lookDelta;

    private void Start()
    {
        _splineControler.Initialize(transform);
        _splineControler.SetSplineContainer(_splineContainer);

        _photoControler.Initialize(transform);

        SetControler(SplineControler);
    }

    private void Update()
    {
        _currentControler.UpdateControler(_inputDirection, _lookDelta);
    }

    public void SetControler(PlayerControlable toSet)
    {
        print($"toSet : {toSet}");
        print($"current : {_currentControler}");
        if (toSet == _currentControler) return;
        _currentControler?.ExitControler();
        _currentControler = toSet;
        _debug_state_name = _currentControler.ToString();
        _currentControler.EnterControler();
    }

    private void OnSwapControler()
    {
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

    private void OnEnable()
    {
        InputEventManager.OnSwapControlerEvent += OnSwapControler;
        InputEventManager.OnMoveEvent += OnMove;
        InputEventManager.OnLookEvent += OnLook;
    }

    private void OnDisable()
    {
        InputEventManager.OnSwapControlerEvent -= OnSwapControler;
        InputEventManager.OnMoveEvent -= OnMove;
        InputEventManager.OnLookEvent -= OnLook;
    }
}
