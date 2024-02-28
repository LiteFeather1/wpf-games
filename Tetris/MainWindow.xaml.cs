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
using Tetris.Source;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] _imageControls;

        private GameState _gameState;

        public MainWindow()
        {
            InitializeComponent();
            const int ROWS = 22, COLS = 10;

            _gameState = new(ROWS, COLS);

            // Setup Game Canvas
            _imageControls = new Image[ROWS, COLS];
            int cellSize = (int)(GameCanvas.Width / COLS); 
            for (var r = 0; r < ROWS; r++)
                for (var c = 0; c < COLS; c++)
                {
                    var imageControl = new Image()
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    // The top 2 rows are invisible/ not visible
                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    _imageControls[r, c] = imageControl;
                }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}