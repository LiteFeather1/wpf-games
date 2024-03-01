namespace Snake.Source
{
    public class GameState
    {
        private const int MAX_DIRECTION_CHANGE_BUFFER = 2;

        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }

        public GridCoordinate SnakeDirection { get; private set; }

        public Food Food { get; private set; }

        public int Score { get; private set; }  
        public bool GameOver { get; private set; }

        private readonly List<GridCoordinate> r_directionChangesBuffer = [];    

        private readonly LinkedList<GridCoordinate> r_snakePositions = new();

        private readonly Random r_random = new();

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];

            SnakeDirection = GridCoordinate.Right;

            // Add Snake
            var r = Rows / 2;
            for (var c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                r_snakePositions.AddFirst(new GridCoordinate(c, r));
            }

            AddFood();
        }

        public GridCoordinate HeadPosition() =>  r_snakePositions.First.Value;

        public IEnumerable<GridCoordinate> SnakePositions() => r_snakePositions;

        public void ChangeDirection(GridCoordinate dir)
        {
            bool canChangeDirection;
            if (r_directionChangesBuffer.Count == MAX_DIRECTION_CHANGE_BUFFER)
                canChangeDirection = false;
            else
            {
                GridCoordinate lastDir = r_directionChangesBuffer.Count == 0
                    ? SnakeDirection : r_directionChangesBuffer[^1];

                canChangeDirection =  dir != lastDir && dir != lastDir.Opposite();
            }

            if (canChangeDirection)
                r_directionChangesBuffer.Add(dir);

        }

        public void Move()
        {
            if (r_directionChangesBuffer.Count > 0)
            {
                SnakeDirection = r_directionChangesBuffer[0];
                r_directionChangesBuffer.RemoveAt(0);
            }

            var newHeadPos = r_snakePositions.First.Value.Translate(SnakeDirection);

            GridValue willHit;
            // Outside grid
            if (newHeadPos.Y < 0 || newHeadPos.Y >= Rows || newHeadPos.X < 0 || newHeadPos.X >= Cols)
                willHit = GridValue.Outside;
            // Especial case if the next tile the snake will hit is the tail
            else if (newHeadPos == r_snakePositions.Last.Value)
                willHit = GridValue.Empty;
            else 
                willHit = Grid[newHeadPos.Y, newHeadPos.X];

            switch (willHit)
            {
                case GridValue.Empty:
                    // Remove tail
                    var tail = r_snakePositions.Last.Value;
                    Grid[tail.Y, tail.X] = GridValue.Empty;
                    r_snakePositions.RemoveLast();

                    AddHead(newHeadPos);
                    break;
                case GridValue.Food:
                    AddHead(newHeadPos);
                    Score += Food.Score;
                    AddFood();
                    break;
                case GridValue.Outside or GridValue.Snake:
                    GameOver = true;
                    break;
            }
        }

        private void AddHead(GridCoordinate pos)
        {
            r_snakePositions.AddFirst(pos);
            Grid[pos.Y, pos.X] = GridValue.Snake;
        }

        private void AddFood()
        {
            var emptyPositions = EmptyPositions().ToArray();
            if (emptyPositions.Length == 0)
                return;

            var pos = emptyPositions[r_random.Next(emptyPositions.Length)];
            Grid[pos.Y, pos.X] = GridValue.Food;
            Food = new(r_random, pos);

            IEnumerable<GridCoordinate> EmptyPositions()
            {
                for (var r = 0; r < Rows; r++)
                    for (var c = 0; c < Cols; c++)
                        if (Grid[r, c] == GridValue.Empty)
                            yield return new(c, r);
            }
        }
    }
}
