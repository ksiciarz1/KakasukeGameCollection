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
    internal class CheckerPice
    {
        internal GridPosition gridPosition;
        internal CheckerColor Color { get => color; }
        private readonly CheckerColor color;

        internal event PiceSelected? onPiceSelected;
        internal delegate void PiceSelected(CheckerPice thisPice);

        private readonly CheckerGameManager Manager;
        private Image selectionRing;


        public CheckerPice(GridPosition gridPosition, CheckerColor checkerColor, CheckerGameManager manager)
        {
            this.gridPosition = gridPosition;
            Manager = manager;
            color = checkerColor;

            // Pice image
            Image piceImage = new Image();
            BitmapImage bitmapImage;

            if (color == CheckerColor.White)
            {
                bitmapImage = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/CheckerPiceWhite.png")));
            }
            else
            {
                bitmapImage = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/CheckerPiceRed.png")));
            }
            manager.GameGrid.Children.Add(piceImage);
            piceImage.SetValue(Grid.ColumnProperty, gridPosition.x);
            piceImage.SetValue(Grid.RowProperty, gridPosition.y);
            piceImage.Source = bitmapImage;
            piceImage.Visibility = System.Windows.Visibility.Visible;
            piceImage.MouseLeftButtonDown += (s, e) =>
            {
                onPiceSelected?.Invoke(this);
                Select();
            };

            // Selection ring image
            selectionRing = new Image();
            selectionRing.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Checkers/SelectionRing.png")));
            manager.GameGrid.Children.Add(selectionRing);
            selectionRing.SetValue(Grid.ColumnProperty, gridPosition.x);
            selectionRing.SetValue(Grid.RowProperty, gridPosition.y);
            selectionRing.Visibility = System.Windows.Visibility.Hidden;
        }

        internal void Select() => selectionRing.Visibility = System.Windows.Visibility.Visible;
        internal void Unselect() => selectionRing.Visibility = System.Windows.Visibility.Hidden;

    }

    internal enum CheckerColor
    {
        White,
        Red
    }
}
