namespace Snake.Source
{
    public class GameState
    {
        private const int MAX_DIRECTION_CHANGE_BUFFER = 2;

        public GridValue[,] Grid { get; }

        public GridCoordinate SnakeDirection { get; private set; }

        private readonly LinkedList<GridCoordinate> r_directionChangesBuffer = new();    

        private readonly LinkedList<GridCoordinate> r_snakePositions = new();

        private readonly Random r_random = new();

        private Difficulty _difficulty;

        public Food Food { get; private set; }

        public int Score { get; private set; }  
        public bool GameOver { get; private set; }

        public GameState(int rows, int cols)
        {
            Grid = new GridValue[rows, cols];

            SnakeDirection = GridCoordinate.Right;

            // Add Snake
            var r = rows / 2;
            for (var c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                r_snakePositions.AddFirst(new GridCoordinate(r, c));
            }

            AddFood();
        }

        public void SetDifficulty(int difficultyIndex) => _difficulty = difficultyIndex switch
        {
            0 => new(64, 192f, 1536f),
            1 => new(32, 128f, 1024f),
            _ => new(16, 64f, 512f)
        };

        public GridCoordinate HeadPosition() =>  r_snakePositions.First.Value;

        public IEnumerable<GridCoordinate> SnakePositions() => r_snakePositions;

        public void ChangeDirection(GridCoordinate dir)
        {
            if (r_directionChangesBuffer.Count == MAX_DIRECTION_CHANGE_BUFFER)
                return;
            else
            {
                var lastDir = r_directionChangesBuffer.Count == 0
                    ? SnakeDirection : r_directionChangesBuffer.Last.Value;
                if (dir == lastDir || dir == lastDir.Opposite())
                    return;
            }

            r_directionChangesBuffer.AddLast(dir);
        }

        public async Task Move()
        {
            await Task.Delay(_difficulty.Delay(Score));

            if (r_directionChangesBuffer.Count > 0)
            {
                SnakeDirection = r_directionChangesBuffer.First.Value;
                r_directionChangesBuffer.RemoveFirst();
            }

            var newHeadPos = r_snakePositions.First.Value.Translate(SnakeDirection);

            GridValue willHit;
            // Outside grid
            if (newHeadPos.Row < 0 || newHeadPos.Row >= Grid.GetLength(0) 
                || newHeadPos.Col < 0 || newHeadPos.Col >= Grid.GetLength(1))
                willHit = GridValue.Outside;
            // Especial case if the next tile the snake will hit is the tail
            else if (newHeadPos == r_snakePositions.Last.Value)
                willHit = GridValue.Empty;
            else 
                willHit = Grid[newHeadPos.Row, newHeadPos.Col];

            switch (willHit)
            {
                case GridValue.Empty:
                    // Remove tail
                    var tail = r_snakePositions.Last.Value;
                    Grid[tail.Row, tail.Col] = GridValue.Empty;
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
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void AddFood()
        {
            var emptyPositions = EmptyPositions().ToArray();
            if (emptyPositions.Length == 0)
                return;

            var pos = emptyPositions[r_random.Next(emptyPositions.Length)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
            Food = new(r_random, pos);

            IEnumerable<GridCoordinate> EmptyPositions()
            {
                for (var r = 0; r < Grid.GetLength(0); r++)
                    for (var c = 0; c < Grid.GetLength(1); c++)
                        if (Grid[r, c] == GridValue.Empty)
                            yield return new(r, c);
            }
        }

        private readonly struct Difficulty(int minDelay, float maxDelay, float scoreToMin)
        {
            public readonly int Delay(int score)
            {
                var delay = (int)(maxDelay + (minDelay - maxDelay) * (score / scoreToMin));
                if (delay < minDelay)
                    return minDelay;

                return delay;
            }
        }
    }
}
