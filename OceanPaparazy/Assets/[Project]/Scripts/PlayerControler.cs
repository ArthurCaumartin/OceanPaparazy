using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    //! movement controler
    [SerializeField] private CameraControler _cameraControler;




    public void OnChangeControlerState(InputValue value)
    {
        _cameraControler
    }
}
