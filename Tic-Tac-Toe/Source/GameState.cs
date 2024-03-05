using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public class GameState
{
    private static readonly SquareCoordinate[] sr_mainDiag = [new(0, 0), new(1, 1), new(2, 2)];
    private static readonly SquareCoordinate[] sr_antiDiag = [new(0, 2), new(1, 1), new(2, 0)];

    public Player[,] GameGrid { get; private set; }

    public Player CurrentPlayer { get; private set; }

    public int TurnsPassed { get; private set; }

    public bool GameOver { get; private set; }

    public event Action<SquareCoordinate> MoveMade;

    public event Action<GameResult> GameEnded;

    public event Action GameRestarted;

    public GameState()
    {
        GameGrid = new Player[3, 3];
        CurrentPlayer = Player.X;
    }

    public void MakeMove(SquareCoordinate square)
    {
        // Can Make Move
        if (GameOver || GameGrid[square.Row, square.Col] != Player.None)
            return;

        GameGrid[square.Row, square.Col] = CurrentPlayer;
        TurnsPassed++;

        // Check if someone won
        var rows = new SquareCoordinate[3];
        var cols = new SquareCoordinate[3];
        for (var i = 0; i < 3; i++)
        {
            rows[i] = new(square.Row, i);
            cols[i] = new(i, square.Col);
        }

        WinInfo winInfo = true switch
        {
            _ when AreSquaresMarked(rows) => new(WinType.Row, square.Row),
            _ when AreSquaresMarked(cols) => new(WinType.Column, square.Col),
            _ when AreSquaresMarked(sr_mainDiag) => new(WinType.MainDiagonal),
            _ when AreSquaresMarked(sr_antiDiag) => new(WinType.AntiDiagonal),
            // Not a winnier
            _ => null
        };

        // Check the game ended
        GameResult gameResult = true switch
        {
            // Someone won
            _ when winInfo != null => new(CurrentPlayer, winInfo),
            // Is Grid Full
            _ when TurnsPassed == 9 => new(Player.None),
            _ => null
        };

        if (gameResult != null)
        {
            GameOver = true;
            GameEnded?.Invoke(gameResult);
        }
        else
            CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;

        MoveMade?.Invoke(square);

        bool AreSquaresMarked(SquareCoordinate[] s)
        {
            for (var i = 0; i < s.Length; i++)
                if (GameGrid[s[i].Row, s[i].Col] != CurrentPlayer)
                    return false;

            return true;
        }
    }

    public void Reset()
    {
        GameGrid = new Player[3, 3];
        CurrentPlayer = Player.X;
        TurnsPassed = 0;
        GameOver = false;

        GameRestarted?.Invoke();
    }
}
