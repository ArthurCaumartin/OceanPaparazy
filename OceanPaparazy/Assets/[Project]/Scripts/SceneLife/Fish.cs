using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private bool _debugPath = false;
    [SerializeField] private bool _debugSmoothPath = false;
    [Space]
    [SerializeField] private LevelGrid _levelGrid;
    [SerializeField] private float _speed = 1f;
    [SerializeField, Range(0f, 1f)] private float _smoothPathSubdivision = 0.5f;
    private Vector3[] _pathArray = new Vector3[0];
    private Vector3[] _smoothPathArray = new Vector3[0];
    private int _currentPathIndex = 0;
    private float _pathTime;
    private float _pathDistance;
    private Vector3 _startPos;
    private Vector3 _endPos;

    private GridCell _startCell;
    private GridCell _endCell;

    void Start()
    {
        SetNewPathArray();
    }

    void Update()
    {
        if (_pathArray.Length == 0) return;

        _pathTime += (Time.deltaTime * _speed) / _pathDistance;
        if (_pathTime >= 1)
        {
            SetNextPath();
            return;
        }

        transform.position = Vector3.Lerp(_startPos, _endPos, _pathTime);
        transform.forward = Vector3.Slerp((_endPos - _startPos).normalized, transform.forward, Time.deltaTime * 5f);
    }

    private void SetNextPath()
    {
        if (_currentPathIndex >= _pathArray.Length - 1)
        {
            SetNewPathArray();
            return;
        }

        _pathTime = 0;
        _currentPathIndex++;
        _pathDistance = Vector3.Distance(_smoothPathArray[_currentPathIndex - 1], _smoothPathArray[_currentPathIndex]);
        _startPos = _smoothPathArray[_currentPathIndex - 1];
        _endPos = _smoothPathArray[_currentPathIndex];
    }

    private void SetNewPathArray()
    {
        GridCell startCell = _levelGrid.GetCellAtPosition(transform.position);
        GridCell endCell = _levelGrid.GetRandomAvaiableCell();

        _startCell = startCell;
        _endCell = endCell;

        _pathArray = AStarPathfinding.GetPath(startCell, endCell);
        _smoothPathArray = SmoothPath(_pathArray);

        print($"New Path | Start Cell : {(startCell != null ? startCell.worldPosition : "null")}, End Cell : {(endCell != null ? endCell.worldPosition : "null")}");
        print("New Path Array: " + _pathArray.Length + " points");

        if (_pathArray.Length <= 1)
        {
            return;
        }

        _currentPathIndex = 1;
        _pathTime = 0;
        _pathDistance = Vector3.Distance(_smoothPathArray[0], _smoothPathArray[1]);
        _startPos = _smoothPathArray[0];
        _endPos = _smoothPathArray[1];
    }

    private Vector3[] SmoothPath(Vector3[] path)
    {

        //TODO : detecter les virages et les smooth
        return path;
    }

    void OnDrawGizmos()
    {
        if (_levelGrid)
        {
            GridCell currentCell = _levelGrid.GetCellAtPosition(transform.position);
            if (currentCell != null)
            {
                Gizmos.color = new Color(0, 0, 1, 0.2f);
                Gizmos.DrawCube(currentCell.worldPosition, Vector3.one * _levelGrid.CellSize * 0.8f);
            }
        }

        if (_startCell != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_startCell.worldPosition, _levelGrid.CellSize * 0.2f);
        }

        if (_endCell != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_endCell.worldPosition, _levelGrid.CellSize * 0.2f);
        }

        if (_pathArray == null) return;
        if (_pathArray.Length == 0) return;

        if (_debugPath)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < _pathArray.Length - 1; i++)
            {
                Gizmos.DrawLine(_pathArray[i], _pathArray[i + 1]);
            }
        }

        if (_debugSmoothPath)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < _smoothPathArray.Length - 1; i++)
            {
                Gizmos.DrawLine(_smoothPathArray[i], _smoothPathArray[i + 1]);
            }
        }
    }
}