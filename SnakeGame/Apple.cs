using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SnakeGame
{
    internal class Apple
    {
        private Image image;
        public KeyValuePair<int, int> positionOnGrid;

        public Apple(int x, int y, Grid mainGrid) : this(new KeyValuePair<int, int>(x, y), mainGrid) { }
        public Apple(KeyValuePair<int, int> gridPosition, Grid mainGrid)
        {
            positionOnGrid = gridPosition;
            image = new Image();
            image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Apple.png")));
            image.Visibility = System.Windows.Visibility.Visible;
            mainGrid.Children.Add(image);
            image.SetValue(Grid.ColumnProperty, gridPosition.Key);
            image.SetValue(Grid.RowProperty, gridPosition.Value);
        }

        public void SnakeCollided()
        {
            image.Source = null;
            image = null;
        }
    }
}
