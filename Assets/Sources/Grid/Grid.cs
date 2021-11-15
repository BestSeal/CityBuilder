using System.Collections.Generic;
using UnityEngine;

namespace Sources.Grid
{
    public class Grid
    {
        public GridCell[,] Cells { get; private set; }
        public Vector3 GridPosition { get; private set; }
        public IReadOnlyCollection<Vector3> CellPositions { get; private set; }

        private int _rowsCount;
        private int _columnsCount;
        private Vector2 _cellSize;

        public Grid Init(Vector3 gridPosition, Vector2 cellSize, int cellRowsCount, int cellColumnsCount)
        {
            _rowsCount = cellRowsCount;
            _columnsCount = cellColumnsCount;
            _cellSize = cellSize;
            var cellPositions = new List<Vector3>(cellColumnsCount * cellRowsCount);
            GridPosition = gridPosition;
            Cells = new GridCell[cellRowsCount, cellColumnsCount];
            for (int i = 0; i < cellRowsCount; i++)
            {
                for (int j = 0; j < cellColumnsCount; j++)
                {
                    var newCellPosition = new Vector3(gridPosition.x + cellSize.x * i, gridPosition.y + GridPosition.y,
                        gridPosition.z + cellSize.y * j);
                    Cells[i, j] = new GridCell(cellSize, newCellPosition);
                    cellPositions.Add(newCellPosition);
                }
            }

            CellPositions = cellPositions;

            return this;
        }
        
        /// <summary>
        /// Try to block cells in the provided region if <see cref="templateBuilding"/> set to false,
        /// otherwise checks if it is possible to block cells in the region
        /// </summary>
        public bool TryBlockCells(Vector3 bottomLeftPoint, int rowCount, int columnCount, bool templateBuilding = false)
        {
            var buildPosition = GetClosestPointOnGrid(bottomLeftPoint);
            
            var maxRowNumber = Mathf.RoundToInt(buildPosition.x) + rowCount;
            var maxColumnNumber = Mathf.RoundToInt(buildPosition.y) + columnCount;
            
            if (maxRowNumber>= _rowsCount ||
                maxColumnNumber>= _columnsCount)
            {
                return false;
            }

            var isRegionEmpty = IsRegionEmpty(Mathf.RoundToInt(buildPosition.x), Mathf.RoundToInt(buildPosition.y),
                maxRowNumber, maxColumnNumber);

            if (templateBuilding)
            {
                return isRegionEmpty;
            }
            
            if (isRegionEmpty)
            {
                for (int i = Mathf.RoundToInt(buildPosition.x); i < maxRowNumber; i++)
                {
                    for (int j = Mathf.RoundToInt(buildPosition.y); j < maxColumnNumber; j++)
                    {
                        Cells[i, j].IsEmpty = false;
                    }
                }

                return true;
            }

            return false;
        }

        public Vector2 GetClosestPointOnGrid(Vector3 currentPoint)
        {
            var clampedXCoordinate = Mathf.RoundToInt(Mathf.Clamp(currentPoint.x, 0, _rowsCount - 1));
            var clampedYCoordinate = Mathf.RoundToInt(Mathf.Clamp(currentPoint.z, 0, _columnsCount - 1));
            var closestCell = Cells[clampedXCoordinate, clampedYCoordinate];

            return new Vector2(closestCell.Position.x, closestCell.Position.z);
        }

        private bool IsRegionEmpty(int minXCoordinate, int minYCoordinate, int maxXCoordinate, int maxYCoordinate)
        {
            var isRegionEmpty = true;
            
            for (int i = minXCoordinate; i < maxXCoordinate; i++)
            {
                for (int j = minYCoordinate; j < maxYCoordinate; j++)
                {
                    if (!Cells[i, j].IsEmpty)
                    {
                        isRegionEmpty = false;
                        break;
                    }
                }
                if (!isRegionEmpty)
                {
                    break;
                }
            }
            
            return isRegionEmpty;
        }
    }
}