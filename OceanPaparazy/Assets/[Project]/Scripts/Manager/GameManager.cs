using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CameraStateMachine _cameraStateMachine;
    [Space]
    [SerializeField] private LevelManager _levelManager;
    private GameObject _playerInstance;
    private CameraStateMachine _cameraStateMachineInstance;


    private void Awake()
    {
        _levelManager.LoadLevel(_levelManager.SceneNameHub, () =>
        {
            _playerInstance = Instantiate(_playerPrefab);
            _cameraStateMachineInstance = Instantiate(_cameraStateMachine);
            _cameraStateMachine.SetCameraState(_cameraStateMachine.CameraStateSpline, _playerInstance.transform);
        });
    }


}

