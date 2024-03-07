using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe.Source;

public static class Images
{
    private static readonly Dictionary<Player, BitmapImage> r_playerCompleteImages = new(2)
    {
        { Player.X, LoadImageInAssets("Xs/X_15") },
        { Player.O, LoadImageInAssets("Os/O_15") }
    };

    private static readonly Dictionary<Player, ObjectAnimationUsingKeyFrames> sr_playerToAnimation = new(2)
    {
        { Player.X, new() },
        { Player.O, new() },
    };

    public static Dictionary<Player, BitmapImage> PlayerCompleteImages 
        => r_playerCompleteImages;

    public static Dictionary<Player, ObjectAnimationUsingKeyFrames> PlayerAnimations 
        => sr_playerToAnimation;

    public static BitmapImage Square => LoadImageInAssets("Square");

    static Images()
    {
        // Set up animations
        string[] p = ["X", "O"];
        var pIndex = 0;
        foreach (var objectAnimation in sr_playerToAnimation.Values)
        {
            objectAnimation.Duration = TimeSpan.FromSeconds(.25);
            for (var i = 0; i < 16; i++)
                objectAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(
                    LoadImageInAssets($"{p[pIndex]}s/{p[pIndex]}_{i:00}")));

            pIndex++;
        }
    }

    private static BitmapImage LoadImageInAssets(string path)
        => new(new($"Pack://application:,,,/Assets/{path}.png"));
}
