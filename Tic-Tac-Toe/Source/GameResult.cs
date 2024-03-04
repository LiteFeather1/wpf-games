﻿using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public class GameResult(Player player, WinInfo winInfo = null)
{
    public Player Player { get; } = player;

    public WinInfo WinInfo { get; } = winInfo;
}
