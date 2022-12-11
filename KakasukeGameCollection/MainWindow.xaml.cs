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

            ImageBrush SnakeImage = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/SnakePreview.png"))));
            ImageBrush SaperImage = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/SaperPreview.png"))));
            ImageBrush CheckerImage = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/CheckersPreview.png"))));
            ImageBrush AmongSuSImage = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/AmongSusPreview.png"))));

            SnakeImage.Stretch = Stretch.UniformToFill;
            SaperImage.Stretch = Stretch.UniformToFill;
            CheckerImage.Stretch = Stretch.UniformToFill;
            AmongSuSImage.Stretch = Stretch.UniformToFill;

            SnakeButton.Background = SnakeImage;
            SaperButton.Background = SaperImage;
            CheckerButton.Background = CheckerImage;
            AmongSusButton.Background = AmongSuSImage;
        }

        private void GameWindowClosed(object? sender, EventArgs e)
        {
            gameWindow = null;
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
        private void CheckerButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameWindow == null)
            {
                gameWindow = new CheckersGame.CheckerWindow(this);
                gameWindow.Closed += GameWindowClosed;
                gameWindow.ShowDialog();
            }
        }
        private void AmongSusButtonClick(object sender, RoutedEventArgs e)
        {
            if (gameWindow == null)
            {
                gameWindow = new AmongSus.AmongSusWindow(this);
                gameWindow.Closed += GameWindowClosed;
                gameWindow.ShowDialog();
            }
        }
    }
}
