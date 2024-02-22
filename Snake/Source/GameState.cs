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

        private void AddHead(Position pos)
        {
            r_snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            var tail = TailPosition();
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            r_snakePositions.RemoveLast();
        }

        public void ChangeDirection(Direction dir) => SnakeDirection = dir;

        private bool OutsideGrid(Position pos) 
            => pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
                return GridValue.Outside;

            // Especial case if the next tile the snake will hit is the tail
            if (newHeadPos == TailPosition())
                return GridValue.Empty;

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void Move()
        {
            var newHeadPos = HeadPosition().Translate(SnakeDirection);
            var hit = WillHit(newHeadPos);

            switch (hit)
            {
                case GridValue.Empty:
                    RemoveTail();
                    AddHead(newHeadPos);
                    break;
                case GridValue.Food:
                    AddHead(newHeadPos);
                    Score++;
                    AddFood();
                    break;
                case GridValue.Outside or GridValue.Snake:
                    GameOver = true;
                    break;
            }
        }

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
