using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public class GameState
{
    public Player[,] GameGrid { get; private set; }

    public Player CurrentPlayer { get; private set; }

    public int TurnsPassed { get; private set; }

    public bool GameOver { get; private set; }

    public delegate void MoveMade(int row, int collum);
    public event MoveMade OnMoveMade;

    public event Action<GameResult> GameEnded;

    public event Action GameRestarted;

    public GameState()
    {
        GameGrid = new Player[3, 3];
        CurrentPlayer = Player.X;
    }
}
