using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CheckersGame
{
    internal class TileStatus
    {
        internal bool selected = false;
        internal GridPosition gridPosition;
        private Image selectionImage = new Image();
        private Grid mainGrid;
        internal bool occupied = false;

        public TileStatus(GridPosition gridPosition, Grid mainGrid)
        {
            this.gridPosition = gridPosition;
            this.mainGrid = mainGrid;

            selectionImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/TileSelectionRing.png")));
            selectionImage.Visibility = System.Windows.Visibility.Hidden;
            mainGrid.Children.Add(selectionImage);
            selectionImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            selectionImage.SetValue(Grid.RowProperty, gridPosition.y);
        }

        internal void Select()
        {
            selectionImage.Visibility = System.Windows.Visibility.Visible;
        }
        internal void Unselect()
        {
            selectionImage.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
