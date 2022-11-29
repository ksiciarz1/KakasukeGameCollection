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

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SnakeWindow : Window
    {
        bool isFullscreen;
        private readonly Window parent; // Main window with game select
        private Snake snake;
        private int score = 0;
        public bool running = true; // running game loop

        public SnakeWindow() : this(new Window()) { } // HACK
        public SnakeWindow(Window parent)
        {
            this.parent = parent;
            InitializeComponent();
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized)
                isFullscreen = true;
            else
                isFullscreen = false;

            Visibility = Visibility.Visible;

            // Green checkerboard for visuals
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Rectangle rec = new Rectangle();
                    if ((i + j) % 2 == 0)
                        rec.Fill = Brushes.Green;
                    else
                        rec.Fill = Brushes.DarkGreen;

                    MainGameGrid.Children.Add(rec);
                    rec.SetValue(Grid.ColumnProperty, i);
                    rec.SetValue(Grid.RowProperty, j);
                    rec.Visibility = Visibility.Visible;
                }
            }
            Focus();
            StartGame();
        }

        private void StartGame()
        {
            GameOverGrid.Visibility = Visibility.Hidden;
            KeyValuePair<int, int> position = new KeyValuePair<int, int>(new Random().Next(2, 13), new Random().Next(2, 13));
            snake = new Snake(position, MainGameGrid);
            snake.OnScoreAdded += Snake_scoreAdded;
            snake.OnGameOver += Snake_gameOver;
            ScoreLabel.Content = "Score: 0";
        }

        private void Snake_gameOver()
        {
            GameOverScore.Content = "Score: " + score;
            GameOverGrid.Visibility = Visibility.Visible;
            score = 0;

        }

        private void Snake_scoreAdded()
        {
            score += 100;
            ScoreLabel.Content = "Score: " + score;
        }
        private void PlayAgainButton_Click(object sender, RoutedEventArgs e) => StartGame();
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (parent != null) // HACK
            {
                parent.Visibility = Visibility.Visible; // Bring up the main window for game select before closing this window
                parent.Focus();
            }
            Close();
        }
        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                isFullscreen = false;
            }
            else
            {
                WindowState = WindowState.Maximized;
                isFullscreen = true;
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                if (isFullscreen)
                    WindowState = WindowState.Maximized;
                else
                    WindowState = WindowState.Normal;
            }
            else
                WindowState = WindowState.Minimized;
        }
        private void OnKeyDownEvent(object sender, KeyEventArgs e) => snake.KeyDownEvent(sender, e);

    }
}
