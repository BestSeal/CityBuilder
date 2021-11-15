using UnityEngine;

namespace Sources.Views
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private int rowCount = 40;
        [SerializeField] private int columnCount = 40;
        [SerializeField] private Vector2 cellSize = Vector2.one;

        public (int rowCount, int columnCount) GetRowsAndColumnsCount => (rowCount, columnCount);
        public Vector2 GetCellSize => cellSize;
        
        private bool _initialized;
        private Grid.Grid _grid;
        private Vector3 _gridPosition => _grid.GridPosition;
        
        private void Awake()
        {
            _grid = new Grid.Grid().Init(transform.position, cellSize, rowCount, columnCount);
        }

        public bool IsCellsRegionEmpty(Vector3 bottomLeftPoint, int rowCount, int columnCount)
        =>_grid.TryBlockCells(bottomLeftPoint, rowCount, columnCount, templateBuilding: true);
        
        public bool TryPlaceBuilding(Vector3 bottomLeftPoint, int rowCount, int columnCount)
            =>_grid.TryBlockCells(bottomLeftPoint, rowCount, columnCount);

        public Vector3 GetClosestCellPosition(Vector3 currentPointerPosition)
        {
            var closestPointOnSurface = _grid.GetClosestPointOnGrid(currentPointerPosition);

            return new Vector3(closestPointOnSurface.x, transform.position.y, closestPointOnSurface.y);
        }
        
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    var newCellPosition = new Vector3(position.x + cellSize.x * (i + 1) - cellSize.x/2, position.y, position.z + cellSize.y * (j + 1) - cellSize.y/2);
                    Gizmos.DrawCube(newCellPosition, new Vector3(cellSize.x, 0.1f, cellSize.y));
                }
            }
        }
    }
}