
namespace Tetris.Source
{
    public class GameGrid(int rows, int cols)
    {
        private readonly int[,] r_grid = new int[rows, cols];

        public int Rows { get; } = rows;
        public int Cols { get; } = cols;

        public int this[int r, int c]
        {
            get => r_grid[r, c];
            set => r_grid[r, c] = value;
        }

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

        public void ClearRow(int r)
        {
            for (var c = 0; c < Cols; c++)
                r_grid[r, c] = 0;
        }

        public void MoveRowDown(int r, int numsOfRows)
        {
            for (var c = 0; c < Cols; c++)
            {
                r_grid[r + numsOfRows, c] = r_grid[r, c];
                r_grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            var cleared = 0;
            for (var r = Rows - 1; r > 0; r--)
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                    MoveRowDown(r, Cols);

            return cleared;
        }
    }
}
