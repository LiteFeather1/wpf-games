using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Tic_Tac_Toe.Source;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe;

public partial class MainWindow : Window
{
    private static readonly Dictionary<Player, SolidColorBrush> sr_playerToColour = new(2);

    private readonly Image[,] r_imageControls = new Image[3, 3];

    private readonly GameState r_gameState = new();

    public MainWindow()
    {
        InitializeComponent();

        var app = Application.Current as App;
        app.InitializeComponent();

        sr_playerToColour.Add(Player.X, app.Resources["XColour"] as SolidColorBrush);
        sr_playerToColour.Add(Player.O, app.Resources["OColour"] as SolidColorBrush);

        r_gameState.MoveMade += OnMoveMade;
        r_gameState.GameEnded += OnGameEnded;
        r_gameState.GameRestarted += OnGameRestarted;

        // Set up GameGrid
        var thickness = new Thickness(18.0);
        for (var r = 0; r < 3; r++)
            for (var c = 0; c < 3; c++)
            {
                var playerImage = new Image()
                { 
                    Margin = thickness,
                };

                r_imageControls[r, c] = playerImage;
                GameGrid.Children.Add(playerImage);

                GameGridSquare.Children.Add(new Image()
                {
                    Margin = thickness,
                    Source = Images.Square,
                    IsHitTestVisible = false,
                });
            }
    }

    #region GameState Events
    private void OnMoveMade(SquareCoordinate square)
    {
        var prevPlayer = r_gameState.GameGrid[square.Row, square.Col];
        r_imageControls[square.Row, square.Col].Source = Images.PlayerCompleteImages[prevPlayer];

        SetPlayerPanel(PlayerText, 
            PlayerImage,
            Images.PlayerCompleteImages[r_gameState.CurrentPlayer],
            r_gameState.CurrentPlayer, 
            prevPlayer);
    }
    
    private async void OnGameEnded(GameResult gameResult)
    {
        await Task.Delay(1000);

        // Trasition to end screen
        if (gameResult.Winner != Player.None)
        {
            ResultText.Text = "Winner: ";
            SetPlayerPanel(ResultText, 
                WinnerImage, 
                Images.PlayerCompleteImages[gameResult.Winner], 
                gameResult.Winner, 
                r_gameState.CurrentPlayer);
        }
        else
        {
            ResultText.Text = "It\'s a tie";
            SetPlayerPanel(ResultText,
                WinnerImage,
                null,
                r_gameState.OppositePlayer,
                r_gameState.CurrentPlayer);
            PlayAgainButton.Foreground = sr_playerToColour[r_gameState.CurrentPlayer];
            PlayAgainButton.BorderBrush = sr_playerToColour[r_gameState.OppositePlayer];
            WinnerImage.Source = null;
        }
        EndScreen.Visibility = Visibility.Visible;
    }

    private void OnGameRestarted()
    {

    }
    #endregion

    #region WindowEvents
    private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // FIXME Game is breaking when placing on the first spot
        var squareSize = GameGrid.Width / 3.0;
        var clickPosition = e.GetPosition(GameGrid);
        r_gameState.MakeMove(new(row: (int)(clickPosition.Y / squareSize),
            col: (int)(clickPosition.X / squareSize)));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
    }
    #endregion

    private static void SetPlayerPanel(TextBlock text,
        Image image,
        BitmapImage source, 
        Player currentPlayer,
        Player oppositePlayer)
    {
        text.Foreground = sr_playerToColour[currentPlayer];
        (text.Effect as DropShadowEffect).Color = sr_playerToColour[oppositePlayer].Color;
        image.Source = source;
    }
}