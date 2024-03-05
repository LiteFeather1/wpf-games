using System.Windows.Media.Imaging;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public static class Images
{
    private static readonly Dictionary<Player, BitmapImage> r_playerCompleteMessages = new()
    {
        { Player.X, new(new("Pack://application:,,,/Assets/Xs/X_15.png")) },
        { Player.O, new(new("Pack://application:,,,/Assets/Os/O_15.png")) },
    };

    public static Dictionary<Player, BitmapImage> ImageSources = r_playerCompleteMessages;
}
