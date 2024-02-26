using Tetris.Source.Blocks;

namespace Tetris.Source
{
    public class GameState
    {
        private Block _currentBlock;

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
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }

        public GameState() 
        {
            GameGrid = new(22, 10);
            BlockQueue = new();  
            CurrentBlock = BlockQueue.GetAndUpdate();
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
                    CurrentBlock = BlockQueue.GetAndUpdate();
            }
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
