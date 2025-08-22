using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;

[HideScriptField, ExecuteInEditMode]
public class LevelGrid : MonoBehaviour
{
    [SerializeField, Range(0, .5f)] private float _gizmosAlpha = 0.2f;
    [SerializeField] private bool _DebugShowGrid = true;
    [SerializeField] private bool _DebugBuildGridIfMove = false;
    [SerializeField] private bool _DebugBuildGridOnUpdate = false;
    [Space]
    [SerializeField] private LayerMask _cellLayerMask;
    [SerializeField] private int _gridSizeX = 10;
    [SerializeField] private int _gridSizeY = 10;
    [SerializeField] private int _gridSizeZ = 10;
    [SerializeField] private float _cellSize = 1f;
    private GridCell[,,] _gridCell = new GridCell[0, 0, 0];
    private GridCell[,,] _gridCellOccupied = new GridCell[0, 0, 0];
    private List<GridCell> _gridCellAvaiable = new List<GridCell>();
    private Vector3 _lastPosition;

    public float CellSize { get => _cellSize; }

    private void Awake()
    {
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }

    private void Update()
    {
        if (_DebugBuildGridOnUpdate)
        {
            Initialize();
            return;
        }

        if (_DebugBuildGridIfMove && transform.position != _lastPosition)
        {
            Initialize();
        }
        _lastPosition = transform.position;
    }

    [ContextMenu("Initialize")]
    public void Initialize()
    {
        _gridCell = new GridCell[_gridSizeX, _gridSizeY, _gridSizeZ];
        _gridCellOccupied = new GridCell[_gridSizeX, _gridSizeY, _gridSizeZ];
        _gridCellAvaiable = new List<GridCell>();

        //? first pass to create the grid cells
        _gridCell.LoopIn((x, y, z) =>
        {
            Vector3 position = (new Vector3(x, y, z) * _cellSize);
            position = transform.TransformPoint(position);

            Collider[] cols = Physics.OverlapBox(position, Vector3.one * .5f * _cellSize, Quaternion.identity, _cellLayerMask);
            bool isOccupied = cols.Length > 0;

            _gridCell[x, y, z] = new GridCell(position, isOccupied);
            if (isOccupied)
                _gridCellOccupied[x, y, z] = _gridCell[x, y, z];
            else
                _gridCellAvaiable.Add(_gridCell[x, y, z]);
        });

        //? second pass to link the neighbors
        _gridCell.LoopIn((x, y, z) =>
        {
            GridCell cell = _gridCell[x, y, z];

            if (x > 0) cell.leftNeighbor = _gridCell[x - 1, y, z];
            if (x < _gridSizeX - 1) cell.rightNeighbor = _gridCell[x + 1, y, z];

            if (y > 0) cell.botomNeighbor = _gridCell[x, y - 1, z];
            if (y < _gridSizeY - 1) cell.upwardNeighbor = _gridCell[x, y + 1, z];

            if (z > 0) cell.backwardCell = _gridCell[x, y, z - 1];
            if (z < _gridSizeZ - 1) cell.forwardCell = _gridCell[x, y, z + 1];
        });
    }

    public GridCell GetRandomAvaiableCell()
    {
        return _gridCellAvaiable[Random.Range(0, _gridCellAvaiable.Count)];
    }

    public GridCell GetCellAtPosition(Vector3 position)
    {
        Vector3 localPosition = transform.InverseTransformPoint(position);
        localPosition += Vector3.one * .5f * _cellSize;
        int x = Mathf.FloorToInt(localPosition.x / _cellSize);
        int y = Mathf.FloorToInt(localPosition.y / _cellSize);
        int z = Mathf.FloorToInt(localPosition.z / _cellSize);

        if (x < 0 || x >= _gridSizeX || y < 0 || y >= _gridSizeY || z < 0 || z >= _gridSizeZ)
            return null;

        return _gridCell[x, y, z];
    }

    private void OnDrawGizmos()
    {
        if (_gridCell == null || _gridCell.Length == 0) return;
        if (_DebugShowGrid)
        {
            _gridCell.LoopIn((x, y, z) =>
            {
                GridCell cell = _gridCell[x, y, z];
                if (cell != null)
                {
                    Gizmos.color = cell.isOccupied ? Color.red : Color.green;
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, _gizmosAlpha);
                    Gizmos.DrawWireCube(cell.worldPosition, Vector3.one * _cellSize * 0.95f);
                }
            });
        }
    }
}
