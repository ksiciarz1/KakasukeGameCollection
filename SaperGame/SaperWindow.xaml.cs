using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
        private Saper? saper;
        private AdvancedStart? advancedStart;
        private readonly Window parent;

        public SaperWindow(Window parent)
        {
            this.parent = parent;
            InitializeComponent();
            Visibility = Visibility.Visible;
            Focus();
            StartGame();
        }

        private void StartGame()
        {
            Random rand = new Random();
            int columnSize = rand.Next(8, 20);
            int rowSize = rand.Next(8, 20);
            int minesPercent = rand.Next(5, 30);
            CreateSaperWindow(columnSize, rowSize, minesPercent);
        }
        private void CreateSaperWindow(int columnSize, int rowSize, int minesPercent)
        {
            GameOverGrid.Visibility = Visibility.Hidden;
            SetWindowSize(columnSize, rowSize);
            // TODO: clear all images from previous saper
            saper = new Saper(columnSize, rowSize, minesPercent, this);
            saper.onGameOver += ShowGameOverGrid;
        }
        private void SetWindowSize(int x, int y)
        {
            myWindow.Width = x * 35;
            myWindow.Height = y * 35 + 25;
        }
        private void ShowGameOverGrid() => GameOverGrid.Visibility = Visibility.Visible;
        private void TryAgainButtonClick(object sender, RoutedEventArgs e)
        {
            if (saper != null)
                saper.Delete();
            StartGame();
        }
        private void AdvancedButtonClick(object sender, RoutedEventArgs e)
        {
            if (advancedStart == null)
            {
                advancedStart = new AdvancedStart(this);
                advancedStart.ShowDialog();
            }
        }

        internal void SetMinesLeft(int minesLeft) => MinesLeftTextBlock.Text = "Mines left: " + minesLeft;
        internal void AdvancedSettingsClosed()
        {
            advancedStart = null;
        }
        internal void AdvancedSettingConfirmed(int width, int height, int minesPercent)
        {
            if (saper != null)
                saper.Delete();
            CreateSaperWindow(width, height, minesPercent);
        }
    }
}
