﻿namespace Snake.Source
{
    public class GameState
    {
        private const int MAX_DIRECTION_CHANGE_BUFFER = 2;

        public GridValue[,] Grid { get; }

        public GridCoordinate SnakeDirection { get; private set; }

        private readonly LinkedList<GridCoordinate> r_directionChangesBuffer = new();    

        private readonly LinkedList<GridCoordinate> r_snakePositions = new();

        private readonly Random r_random = new();

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
                var lastDir = r_directionChangesBuffer.Count == 0
                    ? SnakeDirection : r_directionChangesBuffer.Last.Value;

                canChangeDirection =  dir != lastDir && dir != lastDir.Opposite();
            }

            if (canChangeDirection)
                r_directionChangesBuffer.AddLast(dir);
        }

        public void Move()
        {
            if (r_directionChangesBuffer.Count > 0)
            {
                SnakeDirection = r_directionChangesBuffer.First.Value;
                r_directionChangesBuffer.RemoveFirst();
            }

            var newHeadPos = r_snakePositions.First.Value.Translate(SnakeDirection);

            GridValue willHit;
            // Outside grid
            if (newHeadPos.Y < 0 || newHeadPos.Y >= Grid.GetLength(0) 
                || newHeadPos.X < 0 || newHeadPos.X >= Grid.GetLength(1))
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
                for (var r = 0; r < Grid.GetLength(0); r++)
                    for (var c = 0; c < Grid.GetLength(1); c++)
                        if (Grid[r, c] == GridValue.Empty)
                            yield return new(c, r);
            }
        }
    }
}
