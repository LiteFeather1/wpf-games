using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake.Source
{
    public static class Images
    {
        public static readonly ImageSource Body = LoadImage("all_sprites-Body.png");
        public static readonly ImageSource BodyDead = LoadImage("all_sprites-Body_Dead.png");
        public static readonly ImageSource Empty = LoadImage("all_sprites-Empty.png");
        public static readonly ImageSource Food = LoadImage("all_sprites-Food.png");
        public static readonly ImageSource Head = LoadImage("all_sprites-Head.png");
        public static readonly ImageSource HeadDead = LoadImage("all_sprites-Head_Dead.png");

        private static ImageSource LoadImage(string fileName) 
            => new BitmapImage(new Uri($"Assets/Sprites/{fileName}", UriKind.Relative));
    }
}
