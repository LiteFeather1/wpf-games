using System.Windows.Media.Imaging;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public class Images
{
    public readonly Dictionary<Player, BitmapImage> ImageSources = new()
    {
        { Player.X, new(new("Pack://application:,,,/Assets/Xs/X_15.png")) },
        { Player.O, new(new("Pack://application:,,,/Assets/Os/O_15.png")) },
    };
}
