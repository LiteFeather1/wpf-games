using System.Windows.Media.Imaging;

namespace Snake.Source
{
    public static class Images
    {
        public static readonly BitmapImage Empty = LoadImage("Tile.png");

        public static readonly BitmapImage Body = LoadImage("Body.png");
        public static readonly BitmapImage BodyDead = LoadImage("Body_Dead.png");
        public static readonly BitmapImage Head = LoadImage("Head.png");
        public static readonly BitmapImage HeadDead = LoadImage("Head_Dead.png");

        public static readonly BitmapImage Apple = LoadImage("Foods/Apple.png");
        public static readonly BitmapImage Cherry = LoadImage("Foods/Cherry.png");
        public static readonly BitmapImage Banana = LoadImage("Foods/Banana.png");

        private static BitmapImage LoadImage(string fileName)
            => new(new($"Assets/Sprites/{fileName}", UriKind.Relative));
    }
}
