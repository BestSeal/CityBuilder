using UnityEngine;

namespace Sources.Grid
{
    public class GridCell
    {
        private Vector2 _cellSize;

        /// <summary>
        /// Position of the bottom left corner point of the cell
        /// </summary>
        public Vector3 Position { get; }

        public bool IsEmpty { get; set; } = true;

        public Vector3 GetCellCenterPosition 
            => new Vector3(Position.x + _cellSize.x / 2, Position.y, Position.z + _cellSize.y / 2);

        public GridCell(Vector2 cellSize, Vector3 position)
        {
            _cellSize = cellSize;
            Position = position;
        }
    }
}