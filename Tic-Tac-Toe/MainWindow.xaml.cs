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

        sr_oColour = Resources["OColour"] as SolidColorBrush;
        sr_xColour = Resources["XColour"] as SolidColorBrush;
        sr_oDropShadowColour = (Color)Resources["ODropShadowColour"];
        sr_oDropShadowColour = (Color)Resources["XDropShadowColour"];

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

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
    #endregion
}