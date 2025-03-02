using System;
using UnityEngine;

[Serializable]
public class CameraStateFisrtPersonne : ICameraState
{
    [SerializeField] private float _speed = 5;
    private Vector3 _lookRotation;
    private CameraControler _controler;


    public void Initialise(CameraControler controler)
    {
        _controler = controler;
    }

    public void UpdateState()
    {
        _controler.transform.position = 
        Vector3.Lerp(_controler.transform.position, _controler.Target.position, Time.deltaTime * 10);
        LookAround();
    }

    private void LookAround()
    {
        Vector3 _inputVector = _controler.InputVector;

        _lookRotation.x += -_inputVector.y * _speed;
        _lookRotation.x = Mathf.Clamp(_lookRotation.x, -90, 90);
        _lookRotation.y += _inputVector.x * _speed;

        _controler.transform.rotation = 
        Quaternion.Slerp(_controler.transform.rotation, Quaternion.Euler(_lookRotation), Time.deltaTime * _speed * 10);
    }
}
