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
using System.Windows.Shapes;

namespace CheckersGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CheckerWindow : Window
    {
        private Window parent;
        private CheckerGameManager gameManager;

        public CheckerWindow()
        {
            InitializeComponent();
            MinHeight = 400;
            MinWidth = 400;

            // setting up the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    if ((i + j) % 2 == 0)
                        rectangle.Fill = Brushes.Red;
                    else
                        rectangle.Fill = Brushes.Black;

                    GameGrid.Children.Add(rectangle);
                    rectangle.SetValue(Grid.ColumnProperty, i);
                    rectangle.SetValue(Grid.RowProperty, j);
                    rectangle.Visibility = Visibility.Visible;
                    rectangle.MouseLeftButtonDown += (s, e) =>
                    {
                        //gameManager.TileSelected(new GridPosition(i, j));
                    };
                }
            }
            gameManager = new CheckerGameManager(this);

        }
        public CheckerWindow(Window parent) : this()
        {
            this.parent = parent;
        }



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (parent != null)
                parent.Focus();
        }
    }
}
