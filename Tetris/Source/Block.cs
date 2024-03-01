namespace Tetris.Source
{
    public class Block(int id, Position[][] tiles, Position startOffset)
    {
        #region tetrominoes
        public static readonly Block IBlock = new(
            1,
            [
                [new (1, 0), new(1, 1), new(1, 2), new(1, 3)],
                [new (0, 2), new(1, 2), new(2, 2), new(3, 2)],
                [new (2, 0), new(2, 1), new(2, 2), new(2, 3)],
                [new (0, 1), new(1, 1), new(2, 1), new(3, 1)]
            ],
            new(-1, 3));

        public static readonly Block JBlock = new(
            2,
            [
                [new(0, 0), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(0, 2), new(1, 1), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 2)],
                [new(0, 1), new(1, 1), new(2, 0), new(2, 1)]
            ],
            new(0, 3));

        public static readonly Block LBlock = new(
            3,
            [
                [new(0, 2), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(2, 1), new(2, 2)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 0)],
                [new(0, 0), new(0, 1), new(1, 1), new(2, 1)]
            ],
            new(0, 3));

        public static readonly Block OBlock = new(
            4,
            [[new(0, 0), new(0, 1), new(1, 0), new(1, 1)]],
            new(0, 4));

        public static readonly Block SBlock = new(
            5,
            [
                [new(0, 1), new(0, 2), new(1, 0), new(1, 1)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 2)],
                [new(1, 1), new(1, 2), new(2, 0), new(2, 1)],
                [new(0, 0), new(1, 0), new(1, 1), new(2, 1)]
            ],
            new(0, 3));

        public static readonly Block TBlock = new(
            6,
            [
                [new(0, 1), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 1)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 1)]
            ],
            new(0, 3));

        public static readonly Block ZBlock = new(
            7,
            [
                [new(0, 0), new(0, 1), new(1, 1), new(1, 2)],
                [new(0, 2), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(2, 1), new(2, 2)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 0)]
            ],
            new(0, 3));
        #endregion

        public int ID { get; } = id;

        private readonly Position[][] r_tiles = tiles;
        private readonly Position r_startOffSet = startOffset;
        private readonly Position r_offset = new(startOffset.Row, startOffset.Col);
        private int _rotationState;

        public IEnumerable<Position> TilePositions()
        {
            foreach (var tile in r_tiles[_rotationState])
                yield return new(tile.Row + r_offset.Row, tile.Col + r_offset.Col);
        }

        public void RotateClockWise()
            => _rotationState = (_rotationState + 1) % r_tiles.Length;

        public void RotateCounterClockWise()
            => _rotationState = _rotationState == 0 ? r_tiles.Length - 1 : --_rotationState;

        public void Move(int rows, int cols)
        {
            r_offset.Row += rows;
            r_offset.Col += cols;
        }

        public void Reset()
        {
            _rotationState = 0;
            r_offset.Row = r_startOffSet.Row;
            r_offset.Col = r_startOffSet.Col;
        }
    }
}
