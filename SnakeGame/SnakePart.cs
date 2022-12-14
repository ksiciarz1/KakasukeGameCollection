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
using System.Xml;

namespace SnakeGame
{
    internal class SnakePart
    {
        // manager of all snake parts
        private readonly Snake manager;
        // Visual aspect of this part also wpf element that has to be moved on grid
        private Image image;

        public KeyValuePair<int, int> positionOnGrid;
        private SnakeImage SnakeImage = SnakeImage.Tail;
        private SnakeDirection snakeDirection = SnakeDirection.Right;
        public SnakeDirection directionToChangeTo = SnakeDirection.Right;

        public SnakePart(KeyValuePair<int, int> positionOnGrid, Snake manager)
        {
            this.positionOnGrid = positionOnGrid;
            this.manager = manager;
            image = new Image();
            switch (SnakeImage)
            {
                case SnakeImage.Head:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeHead.png")));
                    break;
                case SnakeImage.Body:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeBody.png")));
                    break;
                case SnakeImage.Tail:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeTail.png")));
                    break;
            }
            manager.GameGrid.Children.Add(image);
        }
        public SnakePart(KeyValuePair<int, int> positionOnGrid, Snake manager, SnakeImage snakeImage) : this(positionOnGrid, manager)
        {
            SnakeImage = snakeImage;
        }

        public void SetDirection(SnakeDirection direction)
        {
            if (direction == SnakeDirection.Right && snakeDirection == SnakeDirection.Left)
            {
                return;
            }
            if (direction == SnakeDirection.Down && snakeDirection == SnakeDirection.Up)
            {
                return;
            }
            else if (direction + 2 == snakeDirection)
            {
                return;
            }

            directionToChangeTo = direction;
            switch (snakeDirection)
            {
                case SnakeDirection.Left:
                    image.RenderTransform = new RotateTransform(90, 45 / 2, 45 / 2);
                    break;
                case SnakeDirection.Up:
                    image.RenderTransform = new RotateTransform(180, 45 / 2, 45 / 2);
                    break;
                case SnakeDirection.Right:
                    image.RenderTransform = new RotateTransform(-90, 45 / 2, 45 / 2);
                    break;
                case SnakeDirection.Down:
                    image.RenderTransform = new RotateTransform(0, 45 / 2, 45 / 2);
                    break;
            }
        }
        public void SetImage(SnakeImage snakeImage)
        {
            SnakeImage = snakeImage;
            switch (SnakeImage)
            {
                case SnakeImage.Head:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeHead.png")));
                    break;
                case SnakeImage.Body:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeBody.png")));
                    break;
                case SnakeImage.Tail:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeTail.png")));
                    break;
                case SnakeImage.TurnLeft:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeTurnLeft.png")));
                    break;
                case SnakeImage.TurnRight:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeTurnRight.png")));
                    break;
                case SnakeImage.TailTurnLeft:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeTailTurnLeft.png")));
                    break;
                case SnakeImage.TailTurnRight:
                    image.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Snake/SnakeTailTurnRight.png")));
                    break;

            }
        }

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
            snakeDirection = directionToChangeTo;
            switch (snakeDirection)
            {
                case SnakeDirection.Left:
                    if (positionOnGrid.Key != 0)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key - 1, positionOnGrid.Value);
                        image.RenderTransform = new RotateTransform(90, 45 / 2, 45 / 2);
                        break;
                    }
                    else
                        throw new OutOfMapException();
                case SnakeDirection.Up:
                    if (positionOnGrid.Value != 0)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key, positionOnGrid.Value - 1);
                        image.RenderTransform = new RotateTransform(180, 45 / 2, 45 / 2);
                        break;
                    }
                    else
                        throw new OutOfMapException();
                case SnakeDirection.Right:
                    if (positionOnGrid.Key != 15)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key + 1, positionOnGrid.Value);
                        image.RenderTransform = new RotateTransform(-90, 45 / 2, 45 / 2);
                        break;
                    }
                    else
                        throw new OutOfMapException();
                case SnakeDirection.Down:
                    if (positionOnGrid.Value != 15)
                    {
                        positionOnGrid = new KeyValuePair<int, int>(positionOnGrid.Key, positionOnGrid.Value + 1);
                        image.RenderTransform = new RotateTransform(0, 45 / 2, 45 / 2);
                        break;
                    }
                    else
                        throw new OutOfMapException();
            }
            ChangeGridPosition();
            return positionOnGrid;
        }

        public void Delete()
        {
            image.Source = null;
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
        Tail,
        TurnLeft,
        TurnRight,
        TailTurnLeft,
        TailTurnRight
    }

    public class OutOfMapException : Exception
    {
        public OutOfMapException() : base() { }
    }
    public class PartsCollisionException : Exception
    {
        public PartsCollisionException() : base() { }
    }
}
