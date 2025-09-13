using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineTime : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] float _speed = 15;
    [Space]
    [SerializeField] string _duration;
    [SerializeField] private float _distance;

    void Update()
    {
        if (!_splineContainer)
        {
            _splineContainer = GetComponent<SplineContainer>();   
            return; 
        }
        _distance = _splineContainer.Spline.GetLength();
        _duration = (_distance / _speed).ToString("F2") + "s";
    }
}