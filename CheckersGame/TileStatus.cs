using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CheckersGame
{
    internal class TileStatus
    {
        internal bool selected = false;
        internal GridPosition gridPosition;
        private Image selectionImage = new Image();
        private Grid mainGrid;
        private CheckerGameManager manager;
        public CheckerPiece? checkerPiece = null;

        public event TileClicked? onTileClicked;
        public delegate void TileClicked(TileStatus thisTile);

        public TileStatus(GridPosition gridPosition, Grid mainGrid, CheckerGameManager manager)
        {
            this.gridPosition = gridPosition;
            this.mainGrid = mainGrid;
            this.manager = manager;

            selectionImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/TileSelectionRing.png")));
            selectionImage.Visibility = System.Windows.Visibility.Hidden;
            mainGrid.Children.Add(selectionImage);
            selectionImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            selectionImage.SetValue(Grid.RowProperty, gridPosition.y);
            selectionImage.MouseLeftButtonDown += (s, e) =>
            {
                onTileClicked?.Invoke(this);
            };
        }

        public void AddPiece(CheckerColor color)
        {
            checkerPiece = new CheckerPiece(gridPosition, color, this, manager);
        }
        public void AddPiece(CheckerPiece piece)
        {
            piece.MoveImage(gridPosition);
            checkerPiece = piece;
        }
        public void RemovePiece()
        {
            checkerPiece?.Delete();
            checkerPiece = null;
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
