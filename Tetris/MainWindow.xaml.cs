using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Tetris.Source;
using Block = Tetris.Source.Block;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int ROWS = 22, COLS = 10;

        private readonly Image[,] r_imageControls;

        private GameState _gameState;

        public MainWindow()
        {
            InitializeComponent();

            _gameState = new(ROWS, COLS);

            // Setup Game Canvas
            r_imageControls = new Image[ROWS, COLS];
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
                    r_imageControls[r, c] = imageControl;
                }
        }

        private void DrawGrid()
        {
            for (var r = 0; r < ROWS; r++)
                for(var c = 0;c < COLS; c++)
                    r_imageControls[r, c].Source = Images.TileImages[_gameState.GameGrid[r, c]];
        }

        private void DrawBlock()
        {
            foreach (var p in _gameState.CurrentBlock.TilePositions())
                r_imageControls[p.Row, p.Col].Source = Images.TileImages[_gameState.CurrentBlock.ID];
        }


        private void Draw()
        {
            DrawGrid();
            DrawBlock();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}