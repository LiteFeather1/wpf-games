using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Snake.Source;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int r_rows = 15, r_cols = 15;
        private readonly Image[,] r_gridImages;

        public MainWindow()
        {
            InitializeComponent();

            r_gridImages = SetUpGrid();
        }

        private Image[,] SetUpGrid()
        {
            var images = new Image[r_rows, r_cols];
            GameGrid.Rows = r_rows;
            GameGrid.Columns = r_cols;
            for (var r = 0; r < r_rows; r++)
            {
                for (var c = 0; c < r_cols; c++)
                {
                    var image = new Image() { Source = Images.Empty };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }
    }
}