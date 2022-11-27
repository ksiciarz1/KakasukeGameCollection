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
        private SnakeGame.SnakeWindow? SnakeGameWindow;
        // private SaperGame.MainWindow? SapperGameWindow;
        // private ChessGame.MainWindow? ChessGameWindow;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void SnakeButtonClick(object sender, RoutedEventArgs e)
        {
            if (SnakeGameWindow == null)
            {
                SnakeGameWindow = new SnakeGame.SnakeWindow(this);
                SnakeGameWindow.Closed += SnakeGameWindow_Closed;
                Visibility = Visibility.Hidden;
            }
        }
        private void SnakeGameWindow_Closed(object? sender, EventArgs e)
        {
            SnakeGameWindow = null;
        }
    }
}
