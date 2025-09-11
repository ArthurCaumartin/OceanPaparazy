using System.Collections.Generic;
using UnityEngine;

public class PhotoTargetDetector : MonoBehaviour
{
    [SerializeField] private bool _debugGizmo;
    [SerializeField] private Camera _photoCamera;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField, Range(1, 20)] private int _detectionResolution = 5;
    [SerializeField, Range(0.1f, 1f)] private float _detectionSize = 0.95f;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private LayerMask _terrainLayerMask;

    private List<PhotoTarget> _photoTargetInRangeList = new List<PhotoTarget>();

    public PhotoTarget MostCenterTarget
    {
        get
        {
            if (_photoTargetInRangeList.Count == 0) return null;
            float minDot = -1f;
            PhotoTarget toReturn = null;
            foreach (var item in _photoTargetInRangeList)
            {
                Vector3 dirToTarget = (item.transform.position - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, dirToTarget);
                // print($"Target: {item.TargetName}, Dot: {dot}");
                if (dot > minDot)
                {
                    minDot = dot;
                    toReturn = item;
                }
            }
            return toReturn;
        }
    }

    private void Update()
    {
        DetectionTargetOverLap();
    }

    private void DetectionTargetOverLap()
    {
        Vector3[,] cornersArray = GetFrustrumCornersForSteps(_detectionResolution);
        cornersArray.LoopIn((i, j) => cornersArray[i, j] = transform.TransformPoint(cornersArray[i, j]));

        _photoTargetInRangeList.Clear();
        for (int i = 1; i < _detectionResolution; i++)
        {
            Vector3 position = Vector3.Lerp(transform.position, transform.position + transform.forward * _detectionRange
                                            , Mathf.InverseLerp(0, _detectionResolution, i));

            float sizeX = (cornersArray[i, 1] - cornersArray[i, 0]).magnitude;
            float sizeY = (cornersArray[i, 3] - cornersArray[i, 0]).magnitude;
            float sizeZ = _detectionRange / _detectionResolution;

            Collider[] hitColliders =
            Physics.OverlapBox(position, new Vector3(sizeX, sizeY, sizeZ) * 0.5f * _detectionSize
                             , transform.rotation, _targetLayerMask);

            if (hitColliders.Length == 0) continue;

            for (int j = 0; j < hitColliders.Length; j++)
            {
                PhotoTarget target = hitColliders[j].GetComponent<PhotoTarget>();
                if (target != null && !_photoTargetInRangeList.Contains(target))
                {
                    if(Physics.Linecast(transform.position, target.transform.position, _terrainLayerMask))
                        continue;
                    _photoTargetInRangeList.Add(target);
                }
            }
        }
    }

    public Vector3[,] GetFrustrumCornersForSteps(int resolution)
    {
        Vector3[,] cornresArray = new Vector3[resolution, 4];
        for (int i = 0; i < resolution; i++)
        {
            float time = Mathf.Lerp(0, 1, Mathf.InverseLerp(0, _detectionResolution, i));

            Vector3[] currentCorners = new Vector3[4];
            _photoCamera.CalculateFrustumCorners(_photoCamera.rect
                                                , Mathf.Lerp(0, _detectionRange, time)
                                                , Camera.MonoOrStereoscopicEye.Mono
                                                , currentCorners);
            for (int j = 0; j < currentCorners.Length; j++)
                cornresArray[i, j] = currentCorners[j];
        }

        return cornresArray;
    }

    private void OnDrawGizmos()
    {
        if (!_debugGizmo) return;
        Vector3[,] cornersArray = GetFrustrumCornersForSteps(_detectionResolution);
        // cornersArray.LoopIn((i, j) => cornersArray[i, j] = transform.TransformPoint(cornersArray[i, j]));
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        //? Draw each detection box
        for (int i = 1; i < _detectionResolution; i++)
        {
            Color color = i % 2 == 0 ? Color.yellow : Color.cyan;
            color.a = 0.5f;
            Gizmos.color = color;

            Vector3 position = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 1) * _detectionRange
                                            , Mathf.InverseLerp(0, _detectionResolution, i));
            float sizeX = (cornersArray[i, 1] - cornersArray[i, 0]).magnitude;
            float sizeY = (cornersArray[i, 3] - cornersArray[i, 0]).magnitude;
            float sizeZ = _detectionRange / _detectionResolution;

            Gizmos.DrawWireCube(position, new Vector3(sizeX, sizeY, sizeZ) * _detectionSize);

            for (int j = 0; j < 4; j++)
            {
                Gizmos.DrawLine(Vector3.zero, cornersArray[i, j]);
                Gizmos.DrawSphere(cornersArray[i, j], 0.5f);
            }

            //? draw end cross
            if (i == _detectionResolution - 1)
            {
                Gizmos.DrawLine(cornersArray[i, 0], cornersArray[i, 2]);
                Gizmos.DrawLine(cornersArray[i, 1], cornersArray[i, 3]);
            }
        }
    }

}
