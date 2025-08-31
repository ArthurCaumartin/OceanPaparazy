using UnityEngine;

public class DroneControler : MonoBehaviour
{
    [SerializeField] private Camera _photoCamera;

    [Header("PhotoTaker Settings : ")]
    [SerializeField] private float _maxFOV = 60f;
    [SerializeField] private float _minFOV = 10f;
    [SerializeField] private float _zoomSpeed = 2f;

    private float _currentZoomLevel;
    private bool _isZoomingPressed;
    private bool _hasZoomed;

    private void Start()
    {
        _currentZoomLevel = _photoCamera.fieldOfView;
    }

    private void Update()
    {
        UpdateZoom();
    }

    private void UpdateZoom()
    {
        if (_isZoomingPressed)
        {
            _currentZoomLevel += (_hasZoomed ? -1 : 1) * _zoomSpeed * Time.deltaTime;
            _currentZoomLevel = Mathf.Clamp(_currentZoomLevel, _minFOV, _maxFOV);
        }

        _photoCamera.fieldOfView = Mathf.Lerp(_photoCamera.fieldOfView
                                        , _currentZoomLevel
                                        , _zoomSpeed * Time.deltaTime);
    }

    private void OnZoomInput(bool isPressed)
    {
        Debug.Log("DroneControlable: OnZoomInput | Zoom : " + isPressed);
        _isZoomingPressed = isPressed;
        if (isPressed)
            _hasZoomed = !_hasZoomed;
    }

    private void OnEnable()
    {
        InputEventManager.OnAbilityFirstEvent += OnZoomInput;
    }

    private void OnDisable()
    {
        InputEventManager.OnAbilityFirstEvent -= OnZoomInput;
    }
}