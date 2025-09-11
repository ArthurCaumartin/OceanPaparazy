using UnityEngine;

//TODO out les trucs de zoom dans CameraFovSetter

public class DroneBehavior : MonoBehaviour
{
    [SerializeField] private Camera _photoCamera;
    [SerializeField] private DroneCameraUI _droneCameraUI;
    [SerializeField] private PhotoTargetDetector _photoTargetDetector;
    [SerializeField] private CameraPhotoTaker _cameraPhotoTaker;

    [Header("PhotoTaker Settings : ")]
    [SerializeField] private float _maxZoomFOV = 60f;
    [SerializeField] private float _minZoomFOV = 10f;
    [SerializeField] private float _zoomSpeed = 1f;
    private float _currentZoomLevel;

    private void Start()
    {
        _currentZoomLevel = _photoCamera.fieldOfView;
    }

    private void Update()
    {
        UpdateZoom();
        PhotoTarget target = _photoTargetDetector.MostCenterTarget;
        _droneCameraUI.SetSelectorPosition(target ? target.transform.position : null);
    }

    public void TakePhoto()
    {
        _cameraPhotoTaker.SavePNG(_photoTargetDetector.MostCenterTarget);
    }

    private void UpdateZoom()
    {
        _photoCamera.fieldOfView = Mathf.Lerp(_photoCamera.fieldOfView
                                        , _currentZoomLevel
                                        , _zoomSpeed * Time.deltaTime);
    }

    public void Zoom(float delta)
    {
        _currentZoomLevel += delta;
        _currentZoomLevel = Mathf.Clamp(_currentZoomLevel, _minZoomFOV, _maxZoomFOV);
    }
}