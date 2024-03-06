using System.Windows.Media.Imaging;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public static class Images
{
    private static readonly Dictionary<Player, BitmapImage> r_playerCompleteImages = new()
    {
        { Player.X, LoadImageInAssets("Xs/X_15") },
        { Player.O, LoadImageInAssets("Os/O_15") }
    };

    public static BitmapImage Square => LoadImageInAssets("Square");

    public static Dictionary<Player, BitmapImage> PlayerCompleteImages => r_playerCompleteImages;

    private static BitmapImage LoadImageInAssets(string path)
        => new(new($"Pack://application:,,,/Assets/{path}.png"));
}
