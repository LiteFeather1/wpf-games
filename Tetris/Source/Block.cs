using System.Collections.Generic;

namespace Tetris.Source
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int ID { get; }

        private readonly Position r_offset;
        private int _rotationState;

        public Block() => r_offset = new(StartOffSet.Row, StartOffSet.Col);

        public IEnumerable<Position> TilePosition()
        {
            foreach (var tile in Tiles[_rotationState])
                yield return new(tile.Row + r_offset.Row, tile.Col + tile.Col);
        }

        public void RotateClockWise()
            => _rotationState = (_rotationState + 1) % Tiles.Length;

        public void RotateCounterClockWise()
            => _rotationState = _rotationState == 0 ? Tiles.Length - 1 : --_rotationState;

        public void Move(int rows, int cols)
        {
            r_offset.Row += rows;
            r_offset.Col += cols;
        }

        public void Reset()
        {
            _rotationState = 0;
            r_offset.Row = StartOffSet.Row;
            r_offset.Col = StartOffSet.Col;
        }
    }
}
