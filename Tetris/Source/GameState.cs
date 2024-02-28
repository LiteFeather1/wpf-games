﻿namespace Tetris.Source
{
    public class GameState
    {
        #region Scoring
        private const int POINT_SOFT_DROP = 1;
        private const int POINTS_COMBO = 50;
        private static readonly Dictionary<int, int> sr_linesClearedToPoints = new()
        {
            {1, 100},
            {2, 300},
            {3, 500},
            {4, 800}
        };

        #endregion

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

        private readonly int[,] r_gameGrid;
        private readonly int r_rows;
        private readonly int r_cols;

        public int[,] GameGrid => r_gameGrid;

        public Block CurrentBlock { get; private set; }
        // TODO maybe preview next 3 block instead of just 1
        public Block NextBlock { get; private set; }

        public int Score { get; private set; }
        public int ComboChainCount { get; private set; }
        public bool GameOver { get; private set; }

        public GameState(int rows, int cols) 
        {
            r_gameGrid = new int[rows, cols];
            r_rows = rows;
            r_cols = cols;

            NextBlock = r_blocks[r_random.Next(r_blocks.Length)];

            UpdateNextAndSetCurrentBlock();
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
                foreach (var p in CurrentBlock.TilePositions())
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

                if (cleared > 0)
                {
                    Score += ComboChainCount++ * POINTS_COMBO;
                    Score += sr_linesClearedToPoints[cleared];
                }
                else
                    ComboChainCount = 0;


                // Is Game Over
                if (!(IsRowEmpty(0) && IsRowEmpty(1)))
                    GameOver = true;
                else
                    UpdateNextAndSetCurrentBlock();
            }

            bool IsRowEmpty(int row)
            {
                for (var c = 0; c < r_cols; c++)
                    if (r_gameGrid[row, c] != 0)
                        return false;

                return true;
            }
        }

        public void MoveBlockDownInput()
        {
            MoveBlockDown();
            Score += POINT_SOFT_DROP;
        }

        private void UpdateNextAndSetCurrentBlock()
        {
            // Get and next update block
            var block = NextBlock;
            do
                NextBlock = r_blocks[r_random.Next(r_blocks.Length)];
            while (block.ID == NextBlock.ID);

            // Set block
            CurrentBlock = block;
            CurrentBlock.Reset();

            for (var i = 0; i < 2; i++)
            {
                CurrentBlock.Move(1, 0);

                if (!BlockFits())
                    CurrentBlock.Move(-1, 0);
            }
        }

        private bool BlockFits()
        {
            foreach (var p in CurrentBlock.TilePositions())
            {
                // Check is grid cord is empty
                if (p.Row >= 0 && p.Row < r_rows && p.Col >= 0 && p.Col < r_cols // Is Inside
                    && r_gameGrid[p.Row, p.Col] == 0) // Is Empty
                    continue;

                return false;
            }

            return true;
        }
    }
}
