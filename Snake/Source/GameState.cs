namespace Snake.Source
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }

        public GridCoordinate SnakeDirection { get; private set; }

        public int Score { get; private set; }  
        public bool GameOver { get; private set; }

        private readonly LinkedList<GridCoordinate> r_directionChangesBuffer = new();

        private readonly LinkedList<GridCoordinate> r_snakePositions = new();

        private readonly Random r_random = new();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[Rows, Cols];

            SnakeDirection = GridCoordinate.Right;

            AddSnake();

            AddFood();
        }

        public void AddSnake()
        {
            var r = Rows / 2;
            for (var c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                r_snakePositions.AddFirst(new GridCoordinate(c, r));   
            }
        }

        public GridCoordinate HeadPosition() =>  r_snakePositions.First.Value;

        public GridCoordinate TailPosition() =>  r_snakePositions.Last.Value;

        public IEnumerable<GridCoordinate> SnakePositions() => r_snakePositions;

        private void AddHead(GridCoordinate pos)
        {
            r_snakePositions.AddFirst(pos);
            Grid[pos.Y, pos.X] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            var tail = TailPosition();
            Grid[tail.Y, tail.X] = GridValue.Empty;
            r_snakePositions.RemoveLast();
        }

        private GridCoordinate GetLastDirection()
        {
            if (r_directionChangesBuffer.Count == 0)
                return SnakeDirection;

            return r_directionChangesBuffer.Last.Value;
        }

        private bool CanChangeDirection(GridCoordinate newDirection)
        {
            if (r_directionChangesBuffer.Count == 2)
                return false;

            var lastDir = GetLastDirection();
            return newDirection != lastDir && newDirection != lastDir.Opposite();
        }

        public void ChangeDirection(GridCoordinate dir)
        {
            if (CanChangeDirection(dir))
                r_directionChangesBuffer.AddLast(dir);

        }

        private bool OutsideGrid(GridCoordinate pos) 
            => pos.Y < 0 || pos.Y >= Rows || pos.X < 0 || pos.X >= Cols;

        private GridValue WillHit(GridCoordinate newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
                return GridValue.Outside;

            // Especial case if the next tile the snake will hit is the tail
            if (newHeadPos == TailPosition())
                return GridValue.Empty;

            return Grid[newHeadPos.Y, newHeadPos.X];
        }

        public void Move()
        {
            if (r_directionChangesBuffer.Count > 0)
            {
                SnakeDirection = r_directionChangesBuffer.First.Value;
                r_directionChangesBuffer.RemoveFirst();
            }

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

        private IEnumerable<GridCoordinate> EmptyPositions()
        {
            for (var r = 0; r < Rows; r++)
                for (var c = 0; c < Cols; c++)
                    if (Grid[r, c] == GridValue.Empty)
                        yield return new(r, c);
        }

        private void AddFood()
        {
            var emptyPositions = EmptyPositions().ToArray();
            if (emptyPositions.Length == 0)
                return;

            var pos = emptyPositions[r_random.Next(emptyPositions.Length)];
            Grid[pos.Y, pos.X] = GridValue.Food;
        }
    }
}
