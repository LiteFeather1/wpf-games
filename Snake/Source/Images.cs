using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake.Source
{
    public static class Images
    {
        public static readonly BitmapImage Body = LoadImage("all_sprites-Body.png");
        public static readonly BitmapImage BodyDead = LoadImage("all_sprites-Body_Dead.png");
        public static readonly BitmapImage Empty = LoadImage("all_sprites-Empty.png");
        public static readonly BitmapImage Food = LoadImage("all_sprites-Food.png");
        public static readonly BitmapImage Head = LoadImage("all_sprites-Head.png");
        public static readonly BitmapImage HeadDead = LoadImage("all_sprites-Head_Dead.png");

        private static BitmapImage LoadImage(string fileName)
            => new(new($"Assets/Sprites/{fileName}", UriKind.Relative));
    }
}
