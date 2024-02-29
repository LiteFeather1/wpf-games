using System.Windows.Media.Imaging;

namespace Tetris.Source
{
    public static class Images
    {
        private static readonly BitmapImage[] sr_tileImages =
        [
            new(new("Assets/Tiles/Empty_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/I_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/J_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/L_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/O_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/S_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/T_Tile.png", UriKind.Relative)),
            new(new("Assets/Tiles/Z_Tile.png", UriKind.Relative))
        ];

        private static readonly BitmapImage[] sr_BlockPreviewImages =
        [
            new(new("Assets/Preview_Icons/Empty-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/I-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/J-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/L-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/O-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/S-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/T-Block.png", UriKind.Relative)),
            new(new("Assets/Preview_Icons/Z-Block.png", UriKind.Relative))
        ];

        public static BitmapImage[] TileImages => sr_tileImages;

        public static BitmapImage[] BlockPreviewImages => sr_BlockPreviewImages;
    }
}
