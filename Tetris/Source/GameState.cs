namespace Tetris.Source
{
    public class GameState
    {
        private Block _currentBlock;

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

        public GameGrid GameGrid { get; }
        public bool GameOver { get; private set; }

        public GameState() 
        {
            _nextBlock = GetRandomBlock();

            GameGrid = new(22, 10);
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
                    GameGrid[p.Row, p.Col] = CurrentBlock.ID;

                GameGrid.ClearFullRows();

                // Is Game Over
                if (!(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(0)))
                    GameOver = true;
                else
                    CurrentBlock = GetAndUpdateNextBlock();
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
                if (!GameGrid.IsEmpty(p.Row, p.Col))
                    return false;

            return true;
        }
    }
}
