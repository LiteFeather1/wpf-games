
namespace Snake.Source
{
    public readonly struct GridCoordinate
    {
        public readonly static GridCoordinate Left = new(0, -1);
        public readonly static GridCoordinate Right = new(0, 1);
        public readonly static GridCoordinate Up = new(-1, 0);
        public readonly static GridCoordinate Down = new(1, 0);

        public readonly int Row { get; }
        public readonly int Col { get; }

        public GridCoordinate(int row, int col) => (Row, Col) = (row, col);

        public readonly GridCoordinate Translate(GridCoordinate dir) => new(Row + dir.Row, Col + dir.Col);

        public readonly GridCoordinate Opposite() => new(-Row, -Col);

        public override readonly bool Equals(object obj)
            => obj is GridCoordinate gridCord &&
                   Row == gridCord.Row &&
                   Col == gridCord.Col;

        public override readonly int GetHashCode() => HashCode.Combine(Row, Col);

        public static bool operator ==(GridCoordinate left, GridCoordinate right) 
            => EqualityComparer<GridCoordinate>.Default.Equals(left, right);

        public static bool operator !=(GridCoordinate left, GridCoordinate right) 
            => !(left == right);
    }
}
