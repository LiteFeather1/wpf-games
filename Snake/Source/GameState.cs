namespace Snake.Source
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }

        public Direction SnakeDirection { get; private set; }
        public int Score { get; private set; }  
        public bool GameOver { get; private set; }

        private readonly LinkedList<Position> r_snakePositions = new();

        private readonly Random r_random = new();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[Rows, Cols];

            SnakeDirection = Direction.Right;

        }
    }
}
