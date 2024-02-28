namespace Tetris.Source
{
    public class GameState
    {
        private readonly int[,] r_gameGrid;

        private readonly Block[] r_blocks =
        [
            Block.IBlock,
            Block.JBlock,
            Block.LBlock,
            Block.OBlock,
            Block.SBlock,
            Block.TBlock,
            Block.ZBlock
        ];

        private readonly Random r_random = new();

        private readonly int r_rows;
        private readonly int r_cols;

        private Block _currentBlock;
        // TODO maybe preview next 3 block instead of just 1
        private Block _nextBlock;

        public Block CurrentBlock
        {
            get => _currentBlock;
            private set
            {
                _currentBlock = value;
                _currentBlock.Reset();
            }
        }

        public bool GameOver { get; private set; }
        //22 10
        public GameState(int rows, int cols) 
        {
            r_gameGrid = new int[rows, cols];
            r_rows = rows;
            r_cols = cols;

            _nextBlock = GetRandomBlock();

            CurrentBlock = GetAndUpdateNextBlock();
        }

        public void RotateBlockClockWise()
        {
            CurrentBlock.RotateClockWise();

            if (!BlockFits())
                CurrentBlock.RotateCounterClockWise();
        }

        public void RotateBlockCounterClockWise()
        {
            CurrentBlock.RotateCounterClockWise();

            if (!BlockFits())
                CurrentBlock.RotateClockWise();
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
                CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
                CurrentBlock.Move(0, -1);
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);   

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);

                // Place blocks
                foreach (var p in CurrentBlock.TilePosition())
                    r_gameGrid[p.Row, p.Col] = CurrentBlock.ID;

                // Clear Full Rows
                var cleared = 0;
                for (var r = r_rows -1; r > 0; r--)
                {
                    var isRowFull = true;
                    for (var c = 0; c < r_cols; c++)
                        if (r_gameGrid[r, c] == 0)
                        {
                            isRowFull = false;
                            break;
                        }

                    if (isRowFull)
                    {
                        // Clear Row
                        for (var c = 0; c < r_cols; c++)
                            r_gameGrid[r, c] = 0;

                        cleared++;
                    }
                    else if (cleared > 0)
                        // Move Row Down
                        for (var c = 0; c < r_cols; c++)
                        {
                            r_gameGrid[r + cleared, c] = r_gameGrid[r, c];
                            r_gameGrid[r, c] = 0;
                        }
                }

                // Is Game Over
                if (!(IsRowEmpty(0) && IsRowEmpty(1)))
                    GameOver = true;
                else
                    CurrentBlock = GetAndUpdateNextBlock();
            }

            bool IsRowEmpty(int row)
            {
                for (var c = 0; c < r_cols; c++)
                    if (r_gameGrid[row, c] != 0)
                        return false;

                return true;
            }
        }

        private Block GetRandomBlock() => r_blocks[r_random.Next(r_blocks.Length)];

        private Block GetAndUpdateNextBlock()
        {
            var block = _nextBlock;
            do
                _nextBlock = GetRandomBlock();
            while (block.ID == _nextBlock.ID);

            return block;
        }

        private bool BlockFits()
        {
            foreach (var p in CurrentBlock.TilePosition())
            {
                // Check is grid cord is empty
                if ((p.Row >= 0 && p.Row < r_rows && p.Col >= 0 && p.Col < r_cols) // Is Inside
                    && r_gameGrid[p.Row, p.Col] == 0) // Is Empty
                    return false;
            }

            return true;
        }
    }
}
