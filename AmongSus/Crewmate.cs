using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AmongSus
{
    class Crewmate
    {
        private const string amongusImagePath = @"./Resources/AmongSus/AmongSus.png";
        private const string amongusBackgroundImagePath = @"./Resources/AmongSus/AmongSusBackground.png";

        private readonly Border parentPart;
        private readonly Image crewmate;
        private readonly SolidColorBrush[] crewmateColors = { Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Blue };
        private readonly Canvas mainCanvas;

        private double moveVectorX;
        private double moveVectorY;

        private double positionX;
        private double positionY;

        private double rotation;


        public Crewmate(Canvas mainCanvas)
        {
            this.mainCanvas = mainCanvas;
            Random random = new Random();
            parentPart = new Border();
            parentPart.Background = crewmateColors[random.Next(crewmateColors.Length)];
            parentPart.OpacityMask = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(amongusBackgroundImagePath))));
            parentPart.Width = 5;
            parentPart.Height = 5;

            crewmate = new Image();
            crewmate.Source = new BitmapImage(new Uri(Path.GetFullPath(amongusImagePath)));

            parentPart.Child = crewmate;
            mainCanvas.Children.Add(parentPart);

            

            moveVectorX = 0;
            while (moveVectorX == 0)
                moveVectorX = random.Next(2, 10) * random.Next(-1, 2);

            moveVectorY = 0;
            while (moveVectorY == 0)
                moveVectorY = random.Next(2, 10) * random.Next(-1, 2);

            rotation = 0;
            while (rotation == 0)
                rotation = random.Next(-10, 11);

            positionX = 400;
            positionY = 400;

            parentPart.Visibility = System.Windows.Visibility.Visible;
            crewmate.Visibility = System.Windows.Visibility.Visible;
        }

        public void Update()
        {
            parentPart.Width += 1;
            parentPart.Height += 1;
            positionX += moveVectorX;
            positionY += moveVectorY;

            double left = parentPart.Width / 2 + positionX;
            double top = parentPart.Height / 2 + positionY;

            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle += rotation;
            rotateTransform.CenterX = parentPart.Width / 2;
            rotateTransform.CenterY = parentPart.Height / 2;
            if (rotation > 0)
                rotation += 1;
            else
                rotation -= 1;
            //rotation %= 360;
            parentPart.RenderTransform = rotateTransform;

            Canvas.SetLeft(parentPart, left);
            Canvas.SetTop(parentPart, top);

            if (positionX + parentPart.Width < 0
                || positionX - parentPart.Width > 800
                || positionY + parentPart.Height < 0
                || positionY - parentPart.Height > 800) // Out of bounds
            {
                Delete();
            }
        }

        private void Delete()
        {
            crewmate.Source = null;
            crewmate.Visibility = System.Windows.Visibility.Collapsed;
            parentPart.Background = null;
            parentPart.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
