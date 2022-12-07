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

        private readonly double moveVectorX;
        private readonly double moveVectorY;

        private double positionX;
        private double positionY;


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


            moveVectorX = (2.1 + (0.1 * random.Next(-10, 11))) * random.Next(-1, 2);
            moveVectorY = (2.1 + (0.1 * random.Next(-10, 11))) * random.Next(-1, 2);

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
