using System;
using UnityEngine;

[Serializable]
public class CameraStateThirdPersonne : ICameraState
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _distance = 5;

    private CameraControler _controler;

    public void Initialise(CameraControler controler)
    {
        _controler = controler;
    }

    public void UpdateState()
    {
        FollowTarget();
        Vector3 dirTarget = (_controler.Target.position - _controler.transform.position).normalized;
        _controler.transform.forward =
        Vector3.Lerp(_controler.transform.forward, dirTarget, Time.deltaTime * 5);
        // _controler.transform.LookAt(_controler.Target);
    }

    private void FollowTarget()
    {
        Vector3 posTarget = _controler.Target.transform.position;
        posTarget.z -= _distance;
        _controler.transform.position = Vector3.Lerp(_controler.transform.position, posTarget, Time.deltaTime * _speed);
    }
}
