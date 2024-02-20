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

            AddSnake();
        }

        public void AddSnake()
        {
            var r = Rows / 2;

            for (var c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                r_snakePositions.AddFirst(new Position(r, c));   
            }
        }
    }
}
