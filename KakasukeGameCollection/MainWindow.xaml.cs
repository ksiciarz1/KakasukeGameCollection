using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KakasukeGameCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window? gameWindow;
        // private SaperGame.MainWindow? SapperGameWindow;
        // private ChessGame.MainWindow? ChessGameWindow;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void SnakeButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameWindow == null)
            {
                gameWindow = new SnakeGame.SnakeWindow(this);
                gameWindow.Closed += GameWindowClosed;
                Visibility = Visibility.Hidden;
            }
        }
        private void SaperButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameWindow == null)
            {
                gameWindow = new SaperGame.SaperWindow(this);
                gameWindow.Closed += GameWindowClosed;
                Visibility = Visibility.Hidden;
            }
        }
        private void GameWindowClosed(object? sender, EventArgs e)
        {
            gameWindow = null;
        }
    }
}
