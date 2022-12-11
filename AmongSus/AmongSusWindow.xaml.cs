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

namespace AmongSus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AmongSusWindow : Window
    {
        private Window parent;
        private List<Crewmate> crewmates = new List<Crewmate>();
        private bool isRunning = true;
        private Color canvasStartingColor;
        private Color canvasEndingColor;
        private int[] colorDirections = { 1, 1, 1, 1, 1, 1 };

        private AmongSusWindow()
        {
            InitializeComponent();
            Area.Background = new RadialGradientBrush(Color.FromRgb(100, 100, 100), Color.FromRgb(200, 200, 200));
            canvasStartingColor = Color.FromRgb(50, 50, 50);
            canvasEndingColor = Color.FromRgb(50, 50, 50);
            Icon = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"./Resources/AmongSus/AmongSus.png")));

            for (int i = 0; i < 25; i++)
            {
                crewmates.Add(new Crewmate(Area));
            }
            Loop();
        }
        public AmongSusWindow(Window parent) : this()
        {
            this.parent = parent;
        }

        private async void Loop()
        {
            while (isRunning)
            {
                foreach (Crewmate crewmate in crewmates)
                {
                    crewmate.Update();
                }
                crewmates.Add(new Crewmate(Area));

                CanvasColorUpdate();

                await Task.Delay(16);
            }
        }

        private void CanvasColorUpdate()
        {
            Random random = new Random();
            const int change = 3;

            canvasStartingColor.R += (byte)(random.Next(change) * colorDirections[0]);
            canvasStartingColor.G += (byte)(random.Next(change) * colorDirections[1]);
            canvasStartingColor.B += (byte)(random.Next(change) * colorDirections[2]);

            if (canvasStartingColor.R > 200 || canvasStartingColor.R < 50) colorDirections[0] *= -1;
            if (canvasStartingColor.G > 200 || canvasStartingColor.G < 50) colorDirections[1] *= -1;
            if (canvasStartingColor.B > 200 || canvasStartingColor.B < 50) colorDirections[2] *= -1;


            canvasEndingColor.R += (byte)(random.Next(change) * colorDirections[3]);
            canvasEndingColor.G += (byte)(random.Next(change) * colorDirections[4]);
            canvasEndingColor.B += (byte)(random.Next(change) * colorDirections[5]);

            if (canvasEndingColor.R > 200 || canvasEndingColor.R < 50) colorDirections[3] *= -1;
            if (canvasEndingColor.G > 200 || canvasEndingColor.G < 50) colorDirections[4] *= -1;
            if (canvasEndingColor.B > 200 || canvasEndingColor.B < 50) colorDirections[5] *= -1;

            Area.Background = new RadialGradientBrush(canvasStartingColor, canvasEndingColor);
        }
    }
}
