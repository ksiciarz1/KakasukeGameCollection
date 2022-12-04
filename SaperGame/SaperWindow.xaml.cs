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

namespace SaperGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SaperWindow : Window
    {
        private Saper saper;
        Window? parent;
        public SaperWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            GameOverGrid.Visibility = Visibility.Hidden;
            Random rand = new Random();
            int columnSize = rand.Next(8, 20);
            int rowSize = rand.Next(8, 20);
            SetWindowSize(columnSize, rowSize);
            // TODO: clear all images from previous saper
            saper = new Saper(columnSize, rowSize, this);
            saper.onGameOver += ShowGameOverGrid;
        }

        public SaperWindow(Window parent) : this()
        {
            this.parent = parent;
        }

        private void SetWindowSize(int x, int y)
        {
            myWindow.Width = x * 35;
            myWindow.Height = y * 35 + 25;
        }
        private void ShowGameOverGrid() => GameOverGrid.Visibility = Visibility.Visible;
        private void TryAgainButtonClick(object sender, RoutedEventArgs e)
        {
            saper.Delete();
            StartGame();
        }
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (parent == null)
            {
                Close();
                return;
            }
            parent.Visibility = Visibility.Visible;
            parent.Focus();
            Close();
        }

        internal void SetMinesLeft(int minesLeft) => MinesLeftTextBlock.Text = "Mines left: " + minesLeft;
    }
}
