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

            AddFood();
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

        public Position HeadPosition() =>  r_snakePositions.First.Value;

        public Position TailPosition() =>  r_snakePositions.Last.Value;

        public IEnumerable<Position> SnakePositions() => r_snakePositions;

        private IEnumerable<Position> EmptyPositions()
        {
            for (var r = 0; r < Rows; r++)
                for (var c = 0; c < Cols; c++)
                    if (Grid[r, c] == GridValue.Empty)
                        yield return new(r, c);
        }

        private void AddFood()
        {
            var emptyPositions = new List<Position>(EmptyPositions());
            if (emptyPositions.Count == 0)
                return;

            var pos = emptyPositions[r_random.Next(emptyPositions.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
        }
    }
}
