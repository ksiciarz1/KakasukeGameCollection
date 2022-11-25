using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SnakeGame
{
    internal class SnakePart
    {
        public KeyValuePair<int, int> positionOnGrid;
        private Snake manager;
        private Image image;

        public SnakeImage SnakeImage = SnakeImage.Head;
        public SnakeDirection SnakeDirection = SnakeDirection.Right;

        public SnakePart(KeyValuePair<int, int> positionOnGrid, Snake manager)
        {
            this.positionOnGrid = positionOnGrid;
            this.manager = manager;
            image = new Image();
            image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/SnakeHead.png")));
            manager.GameGrid.Children.Add(image);
        }
        public SnakePart(KeyValuePair<int, int> positionOnGrid, Snake manager, SnakeImage snakeImage) : this(positionOnGrid, manager)
        {
            SnakeImage = snakeImage;
        }

        public void SetDirection(SnakeDirection direction) => SnakeDirection = direction;
        public void SetImage(SnakeImage snakeImage) => SnakeImage = snakeImage;

        public KeyValuePair<int, int> ChangeGridPosition(int x, int y) => ChangeGridPosition(new KeyValuePair<int, int>(x, y));
        public KeyValuePair<int, int> ChangeGridPosition(KeyValuePair<int, int> positionOnGrid)
        {
            this.positionOnGrid = positionOnGrid;
            return ChangeGridPosition();
        }
        public KeyValuePair<int, int> ChangeGridPosition()
        {
            manager.setGridPosition(image, positionOnGrid);
            return positionOnGrid;
        }


        /// <summary>
        /// Move this part based on it's direction
        /// </summary>
        /// <returns>new position on grid</returns>
        public KeyValuePair<int, int> MoveOnTick()
        {
            switch (SnakeDirection)
            {
                case SnakeDirection.Left:
                    if (positionOnGrid.Key != 0)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key - 1, positionOnGrid.Value);
                        break;
                    }
                    // Error out of map
                    break;
                case SnakeDirection.Up:
                    if (positionOnGrid.Value != 0)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key, positionOnGrid.Value - 1);
                        break;
                    }
                    // Error out of map
                    break;
                case SnakeDirection.Right:
                    if (positionOnGrid.Key != 15)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key + 1, positionOnGrid.Value);
                        break;
                    }
                    // Error out of map
                    break;
                case SnakeDirection.Down:
                    if (positionOnGrid.Value != 15)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key, positionOnGrid.Value + 1);
                        break;
                    }
                    // Error out of map
                    break;
            }
            ChangeGridPosition();
            return positionOnGrid;
        }
    }

    public enum SnakeDirection
    {
        Left,
        Up,
        Right,
        Down,
        None
    }
    public enum SnakeImage
    {
        Head,
        Body,
        Tail
    }
}
