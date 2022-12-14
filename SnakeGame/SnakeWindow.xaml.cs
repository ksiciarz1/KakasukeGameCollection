using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly Window parent; // Main window with game select
        private Snake? snake;
        bool isFullscreen;
        private int score = 0;

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
        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (snake != null)
                snake.KeyDownEvent(sender, e);
        }
    }
}
