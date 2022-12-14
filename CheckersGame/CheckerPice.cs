using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CheckersGame
{
    internal class CheckerPiece
    {
        internal GridPosition gridPosition;
        internal CheckerColor Color { get => color; }
        public bool promoted = false;

        private readonly CheckerColor color;

        internal event PieceSelected? onPieceSelected;
        internal delegate void PieceSelected(CheckerPiece thisPiece);

        internal TileStatus parentTile;
        private Image selectionRing;
        private Image pieceImage;
        private Image promotionImage;
        internal bool selected = false;

        public CheckerPiece(GridPosition gridPosition, CheckerColor checkerColor, TileStatus tile, CheckerGameManager manager)
        {
            this.gridPosition = gridPosition;
            color = checkerColor;
            parentTile = tile;

            // Piece image
            pieceImage = new Image();
            BitmapImage bitmapImage;

            if (color == CheckerColor.White)
                bitmapImage = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/CheckerPieceWhite.png")));

            else
                bitmapImage = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/CheckerPieceRed.png")));

            manager.GameGrid.Children.Add(pieceImage);
            pieceImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            pieceImage.SetValue(Grid.RowProperty, gridPosition.y);
            pieceImage.Source = bitmapImage;
            pieceImage.Visibility = System.Windows.Visibility.Visible;
            pieceImage.MouseLeftButtonDown += (s, e) => onPieceSelected?.Invoke(this);

            // Selection ring image
            selectionRing = new Image();
            selectionRing.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/SelectionRing.png")));
            manager.GameGrid.Children.Add(selectionRing);
            selectionRing.SetValue(Grid.ColumnProperty, gridPosition.x);
            selectionRing.SetValue(Grid.RowProperty, gridPosition.y);
            selectionRing.Visibility = System.Windows.Visibility.Hidden;

            // Promotion image
            promotionImage = new Image();
            promotionImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/CheckerPromotion.png")));
            manager.GameGrid.Children.Add(promotionImage);
            promotionImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            promotionImage.SetValue(Grid.RowProperty, gridPosition.y);
            promotionImage.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void MoveImage(GridPosition gridPosition)
        {
            this.gridPosition = gridPosition;
            parentTile.checkerPiece = null;

            Unselect();
            selectionRing.SetValue(Grid.ColumnProperty, gridPosition.x);
            selectionRing.SetValue(Grid.RowProperty, gridPosition.y);

            pieceImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            pieceImage.SetValue(Grid.RowProperty, gridPosition.y);

            promotionImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            promotionImage.SetValue(Grid.RowProperty, gridPosition.y);

            if (Color == CheckerColor.White && gridPosition.y == 7
                || Color == CheckerColor.Red && gridPosition.y == 0)
                Promote();
        }

        private void Promote()
        {
            promoted = true;
            promotionImage.Visibility = System.Windows.Visibility.Visible;
        }

        internal void Select()
        {
            selectionRing.Visibility = System.Windows.Visibility.Visible;
            selected = true;
        }
        internal void Unselect()
        {
            selectionRing.Visibility = System.Windows.Visibility.Collapsed;
            selected = false;
        }

        internal void Delete()
        {
            selectionRing.Visibility = System.Windows.Visibility.Collapsed;
            selectionRing.Source = null;
            pieceImage.Visibility = System.Windows.Visibility.Collapsed;
            pieceImage.Source = null;

            parentTile.checkerPiece = null;
        }
    }

    internal enum CheckerColor
    {
        White,
        Red
    }
}
