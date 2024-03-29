﻿namespace Tetris.Source
{
    public class GameState
    {
        private const int LINES_CLEARED_PER_LEVEL = 10;

        #region Scoring
        private const int POINT_SOFT_DROP = 1;
        private const int POINTS_HARD_DROP = 2;
        private const int POINTS_COMBO = 50;
        private static readonly int[] sr_linesClearedToPoints =
        [
            100,
            300,
            500,
            800
        ];
        #endregion

        //
        /// <summary>
        /// Speed curve in G
        /// In Which 1G is one block per frame down
        /// </summary>
        // Read More here: https://tetris.wiki/Marathon
        private static readonly float[] sr_levelToSpeedCurve =
        [
            0.01667f,
            0.021017f,
            0.026977f,
            0.035256f,
            0.04693f,
            0.06361f,
            0.0879f,
            0.1236f,
            0.1775f,
            0.2598f,
            0.388f,
            0.59f,
            0.92f,
            1.46f,
            2.36f,
            //3.91f,
            //6.61f,
            //11.43f,
            //20f
        ];

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

        private int _linesCleared;

        public Block CurrentBlock { get; private set; }
        // TODO maybe preview next 3 block instead of just 1
        public Block NextBlock { get; private set; }
        public Block HeldBlock { get; private set; }

        public bool CanHoldBlock { get; private set; } = true;

        public int Score { get; private set; }
        public int ComboChainCount { get; private set; }
        public bool GameOver { get; private set; }

        public static float[] SpeedCurve => sr_levelToSpeedCurve;

        public int[,] GameGrid => r_gameGrid;

        public int Level => _linesCleared / LINES_CLEARED_PER_LEVEL;

        public GameState(int rows, int cols) 
        {
            r_gameGrid = new int[rows, cols];

            NextBlock = r_blocks[r_random.Next(r_blocks.Length)];

            UpdateNextAndSetCurrentBlock();
        }

        public void RotateBlockClockWiseInput()
        {
            CurrentBlock.RotateClockWise();

            if (!BlockFits())
                CurrentBlock.RotateCounterClockWise();
        }

        public void RotateBlockCounterClockWiseInput()
        {
            CurrentBlock.RotateCounterClockWise();

            if (!BlockFits())
                CurrentBlock.RotateClockWise();
        }

        public void MoveBlockLeftInput()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
                CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRightInput()
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

                PlaceBlock();  
            }
        }

        public void MoveBlockDownInput()
        {
            MoveBlockDown();
            Score += POINT_SOFT_DROP;
        }

        public int HardDropDistance()
        {
            var hardDropDistance = r_gameGrid.GetLength(0);
            foreach (var p in CurrentBlock.TilePositions())
            {
                // Tile drop distance
                var drop = 0;
                while (IsEmptyPosition(p.Row + drop + 1, p.Col))
                    drop++;

                if (drop < hardDropDistance)
                    hardDropDistance = drop;
            }

            return hardDropDistance;
        }

        public void HardDropInput()
        {
            var hardDropDistance = HardDropDistance();

            // Hard drop block
            CurrentBlock.Move(hardDropDistance, 0);
            PlaceBlock();

            Score += hardDropDistance * POINTS_HARD_DROP;
        }

        public void HoldBlockInput()
        {
            if (!CanHoldBlock)
                return;

            CanHoldBlock = false;
            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                UpdateNextAndSetCurrentBlock();
            }
            else
            {
                (CurrentBlock, HeldBlock) = (HeldBlock, CurrentBlock);
                FixCurrentBlockSpawnPosition();
            }
        }

        private bool IsEmptyPosition(int row, int col)
            => row >= 0 && row < r_gameGrid.GetLength(0) && col >= 0 && col < r_gameGrid.GetLength(1) // Is inside
                && r_gameGrid[row, col] == 0; // Is empty tiles

        private bool BlockFits()
        {
            foreach (var p in CurrentBlock.TilePositions())
                if (!IsEmptyPosition(p.Row, p.Col))
                    return false;

            return true;
        }

        private void PlaceBlock()
        {
            // Place blocks
            foreach (var p in CurrentBlock.TilePositions())
                r_gameGrid[p.Row, p.Col] = CurrentBlock.ID;

            // Clear Full Rows
            var cols = r_gameGrid.GetLength(1);
            var cleared = 0;
            for (var r = r_gameGrid.GetLength(0) - 1; r > 0; r--)
            {
                var isRowFull = true;
                for (var c = 0; c < cols; c++)
                    if (r_gameGrid[r, c] == 0)
                    {
                        isRowFull = false;
                        break;
                    }

                if (isRowFull)
                {
                    // Clear Row
                    for (var c = 0; c < cols; c++)
                        r_gameGrid[r, c] = 0;

                    cleared++;
                }
                else if (cleared > 0)
                    // Move Row Down
                    for (var c = 0; c < cols; c++)
                    {
                        r_gameGrid[r + cleared, c] = r_gameGrid[r, c];
                        r_gameGrid[r, c] = 0;
                    }
            }

            if (_linesCleared < (sr_levelToSpeedCurve.Length - 1) * LINES_CLEARED_PER_LEVEL)
                _linesCleared += cleared;

            if (cleared > 0)
            {
                var level = Level + 1;
                Score += ComboChainCount++ * POINTS_COMBO * level;
                Score += sr_linesClearedToPoints[cleared - 1] * level;
            }
            else
                ComboChainCount = 0;

            // Is Game Over
            if (!(IsRowEmpty(0) && IsRowEmpty(1)))
                GameOver = true;
            else
            {
                UpdateNextAndSetCurrentBlock();
                CanHoldBlock = true;
            }

            bool IsRowEmpty(int row)
            {
                for (var c = 0; c < cols; c++)
                    if (r_gameGrid[row, c] != 0)
                        return false;

                return true;
            }
        }

        private void FixCurrentBlockSpawnPosition()
        {
            CurrentBlock.Reset();

            for (var i = 0; i < 2; i++)
            {
                CurrentBlock.Move(1, 0);

                if (!BlockFits())
                    CurrentBlock.Move(-1, 0);
            }
        }

        private void UpdateNextAndSetCurrentBlock()
        {
            CurrentBlock = NextBlock;
            do
                NextBlock = r_blocks[r_random.Next(r_blocks.Length)];
            while (CurrentBlock.ID == NextBlock.ID);

            FixCurrentBlockSpawnPosition();
        }
    }
}
