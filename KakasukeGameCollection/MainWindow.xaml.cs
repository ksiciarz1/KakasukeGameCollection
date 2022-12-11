using System;
using System.Collections.Generic;
using System.IO;
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

namespace KakasukeGameCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window? gameWindow;

        public MainWindow()
        {
            InitializeComponent();
            SnakeButton.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/SnakePreview.png"))));
            SaperButton.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/SaperPreview.png"))));
            CheckerButton.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/CheckersPreview.png"))));
        }

        private void SnakeButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameWindow == null)
            {
                gameWindow = new SnakeGame.SnakeWindow(this);
                gameWindow.Closed += GameWindowClosed;
                gameWindow.ShowDialog();
            }
        }
        private void SaperButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameWindow == null)
            {
                gameWindow = new SaperGame.SaperWindow(this);
                gameWindow.Closed += GameWindowClosed;
                gameWindow.ShowDialog();
            }
        }
        private void GameWindowClosed(object? sender, EventArgs e)
        {
            gameWindow = null;
        }

        private void CheckerButtonClick(object sender, RoutedEventArgs e)
        {
            if(gameWindow == null)
            {
                gameWindow = new CheckersGame.CheckerWindow(this);
                gameWindow.Closed += GameWindowClosed;
                gameWindow.ShowDialog();
            }
        }
    }
}
