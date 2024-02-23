using System.Windows.Media.Imaging;

namespace Snake.Source
{
    public class Food
    {
        public int Score { get; }

        public BitmapImage Image { get; }

        public GridCoordinate Position { get; }

        public Food(Random r, GridCoordinate position)
        {
            Position = position;

            (Score, Image) = r.Next(3) switch
            {
                0 => (5, Images.Apple),
                1 => (10, Images.Cherry),
                _ => (15, Images.Banana),
            };
        }
    }
}
