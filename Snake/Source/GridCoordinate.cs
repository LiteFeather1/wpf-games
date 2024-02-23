
namespace Snake.Source
{
    public readonly struct GridCoordinate
    {
        public readonly static GridCoordinate Left = new(-1, 0);
        public readonly static GridCoordinate Right = new(1, 0);
        public readonly static GridCoordinate Up = new(0, -1);
        public readonly static GridCoordinate Down = new(0, 1);

        public readonly int X { get; }
        public readonly int Y { get; }

        public GridCoordinate(int x, int y) => (X, Y) = (x, y);

        public readonly GridCoordinate Translate(GridCoordinate dir) => new(X + dir.X, Y + dir.Y);

        public readonly GridCoordinate Opposite() => new(-X, -Y);

        public override readonly bool Equals(object obj)
            => obj is GridCoordinate direction &&
                   Y == direction.Y &&
                   X == direction.X;

        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(GridCoordinate left, GridCoordinate right) 
            => EqualityComparer<GridCoordinate>.Default.Equals(left, right);

        public static bool operator !=(GridCoordinate left, GridCoordinate right) 
            => !(left == right);
    }
}
