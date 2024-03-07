using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public class WinInfo(WinType winType, int number = 0)
{
    public WinType WinType { get; } = winType;

    public int Number { get; } = number;
}
