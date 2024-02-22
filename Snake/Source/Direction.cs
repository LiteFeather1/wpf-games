
namespace Snake.Source
{
    public class Direction
    {
        public readonly static Direction Left = new(0, -1);
        public readonly static Direction Right = new(0, 1);
        public readonly static Direction Up = new(-1, 0);
        public readonly static Direction Down = new(1, 0);

        public int RowOffset { get; }
        public int ColOffset { get; }

        private Direction(int row, int col) => (RowOffset, ColOffset) = (row, col);

        public Direction Opposite() => new(-RowOffset, -ColOffset);

        public override bool Equals(object obj)
            => obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;

        public override int GetHashCode() => HashCode.Combine(RowOffset, ColOffset);

        public static bool operator ==(Direction left, Direction right) 
            => EqualityComparer<Direction>.Default.Equals(left, right);

        public static bool operator !=(Direction left, Direction right) 
            => !(left == right);
    }
}
