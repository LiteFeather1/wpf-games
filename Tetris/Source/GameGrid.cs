
namespace Tetris.Source
{
    public class GameGrid(int rows, int cols)
    {
        private readonly int[,] r_grid = new int[rows, cols];

        public int Rows { get; } = rows;
        public int Cols { get; } = cols;

        public int this[int r, int c] => r_grid[r, c];

        public bool IsInside(int r, int c)
            => r >= 0 && r < Rows && c >= 0 && c < Cols;

        public bool IsEmpty(int r, int c)
            => IsInside(r, c) && r_grid[r, c] == 0;

        public bool IsRowFull(int r)
        {
            for (var c = 0; c < Cols; c++)
                if (r_grid[r, c] == 0)
                    return false;

            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (var c = 0; r < Cols; c++)
                if (r_grid[r, c] != 0)
                    return false;

            return true;
        }
    }
}
