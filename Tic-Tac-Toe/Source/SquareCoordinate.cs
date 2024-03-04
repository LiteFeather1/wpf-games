namespace Tic_Tac_Toe.Source;

public readonly struct SquareCoordinate(int row, int col)
{
    public readonly int Row { get; } = row;
    public readonly int Col { get; } = col;
}
