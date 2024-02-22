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
        private readonly Dictionary<GridValue, BitmapSource> r_gridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food },
        };

        private readonly int r_rows = 16, r_cols = 16;
        private readonly Image[,] r_gridImages;

        private readonly GameState r_gameState;

        public MainWindow()
        {
            InitializeComponent();

            r_gridImages = SetUpGrid();

            r_gameState = new(r_rows, r_cols);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (r_gameState.GameOver)
                return;

            r_gameState.ChangeDirection(e.Key switch
            {
                Key.A or Key.Left => Direction.Left,
                Key.D or Key.Right => Direction.Right,
                Key.W or Key.Up => Direction.Up,
                _ or Key.S or Key.Down => Direction.Down
            });
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

        private void DrawGrid()
        {
            for (var r = 0; r < r_rows; r++)
            {
                for (var c = 0;c < r_cols; c++)
                {
                    var gridValue = r_gameState.Grid[r, c];
                    r_gridImages[r, c].Source = r_gridValToImage[gridValue];
                }
            }
        }

        private void Draw()
        {
            DrawGrid();
        }
    }
}