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

namespace Tic_Tac_Toe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush sr_xColour;
        private readonly SolidColorBrush sr_oColour;
        private readonly Color sr_xDropShadowColour;
        private readonly Color sr_oDropShadowColour;

        public MainWindow()
        {
            InitializeComponent();

            sr_oColour = Resources["OColour"] as SolidColorBrush;
            sr_xColour = Resources["XColour"] as SolidColorBrush;
            sr_oDropShadowColour = (Color)Resources["ODropShadowColour"];
            sr_oDropShadowColour = (Color)Resources["XDropShadowColour"];
        }

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}