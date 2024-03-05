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
using Tic_Tac_Toe.Source;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe;

public partial class MainWindow : Window
{
    private readonly SolidColorBrush sr_xColour;
    private readonly SolidColorBrush sr_oColour;
    private readonly Color sr_xDropShadowColour;
    private readonly Color sr_oDropShadowColour;

    private readonly Image[,] r_imageControls = new Image[3, 3];

    private readonly GameState r_gameState = new();

    public MainWindow()
    {
        InitializeComponent();

        var app = Application.Current as App;
        app.InitializeComponent();

        sr_xColour = app.Resources["XColour"] as SolidColorBrush;
        sr_oColour = app.Resources["OColour"] as SolidColorBrush;
        sr_xDropShadowColour = (Color)app.Resources["XDropShadowColour"];
        sr_oDropShadowColour = (Color)app.Resources["ODropShadowColour"];  

        r_gameState.MoveMade += OnMoveMade;
        r_gameState.GameEnded += OnGameEnded;
        r_gameState.GameRestarted += OnGameRestarted;

        // Set up GameGrid
        for (var r = 0; r < 3; r++)
            for (var c = 0; c < 3; c++)
            {
                var image = new Image();
                r_imageControls[r, c] = image;
                GameGrid.Children.Add(image);
            }
    }

    #region GameState Events
    private void OnMoveMade(SquareCoordinate square)
    {
        r_imageControls[square.Row, square.Col].Source = 
            Images.PlayerCompleteImages[r_gameState.GameGrid[square.Row, square.Col]];

        PlayerImage.Source = Images.PlayerCompleteImages[r_gameState.CurrentPlayer];
    }
    
    private void  OnGameEnded(GameResult gameResult)
    {

    }

    private void OnGameRestarted()
    {

    }
    #endregion

    #region WindowEvents
    private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var squareSize = GameGrid.Width / 3.0;
        var clickPosition = e.GetPosition(GameGrid);
        r_gameState.MakeMove(new(row: (int)(clickPosition.Y / squareSize),
            col: (int)(clickPosition.X / squareSize)));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion
}