using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SaperGame
{
    class SaperPart
    {
        private readonly Image tileImage;
        private readonly Image mineImage;
        private readonly Image flagImage;
        private readonly Label label;

        private bool flagged = false;
        private bool discovered = false;
        private KeyValuePair<int, int> gridPosition;
        private readonly Grid mainGameGrid;
        private readonly Saper manager;

        public int surroundingMines;
        public SaperTileType saperTile = SaperTileType.None;

        public SaperPart(int x, int y, Grid mainGameGrid, Saper manager)
        {
            this.mainGameGrid = mainGameGrid;
            this.manager = manager;
            gridPosition = new KeyValuePair<int, int>(x, y);

            tileImage = new Image();
            tileImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Tile.png")));
            mainGameGrid.Children.Add(tileImage);
            tileImage.SetValue(Grid.ColumnProperty, x);
            tileImage.SetValue(Grid.RowProperty, y);

            mineImage = new Image();
            mineImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Mine.png")));
            mainGameGrid.Children.Add(mineImage);
            mineImage.SetValue(Grid.ColumnProperty, x);
            mineImage.SetValue(Grid.RowProperty, y);

            flagImage = new Image();
            flagImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Flag.png")));
            mainGameGrid.Children.Add(flagImage);
            flagImage.SetValue(Grid.ColumnProperty, x);
            flagImage.SetValue(Grid.RowProperty, y);

            label = new Label();
            mainGameGrid.Children.Add(label);
            label.SetValue(Grid.ColumnProperty, x);
            label.SetValue(Grid.RowProperty, y);

            tileImage.Visibility = System.Windows.Visibility.Visible;
            mineImage.Visibility = System.Windows.Visibility.Hidden;
            flagImage.Visibility = System.Windows.Visibility.Hidden;
            label.Visibility = System.Windows.Visibility.Hidden;

            tileImage.MouseLeftButtonDown += ClickEvent;
            mineImage.MouseLeftButtonDown += ClickEvent;
            flagImage.MouseLeftButtonDown += ClickEvent;
            label.MouseLeftButtonDown += ClickEvent;

            tileImage.MouseRightButtonDown += ToggleFlagEvent;
            mineImage.MouseRightButtonDown += ToggleFlagEvent;
            flagImage.MouseRightButtonDown += ToggleFlagEvent;
            label.MouseRightButtonDown += ToggleFlagEvent;
        }
        public void Expload()
        {
            mineImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/MineExploaded.png")));
            mineImage.Visibility = System.Windows.Visibility.Visible;
            discovered = true;
        }
        public void Discover()
        {
            if (discovered)
                return;
            discovered = true;
            tileImage.Source = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/TileDiscovered.png")));
            label.Content = surroundingMines;
            if (surroundingMines != 0)
            {
                label.Visibility = System.Windows.Visibility.Visible;
            }
            else
                CheckSurroundForMines();

        }

        private void CheckSurroundForMines()
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    KeyValuePair<int, int> lookedPartPosition = new(gridPosition.Key + i, gridPosition.Value + j);
                    if (!manager.CheckIfPartHaveMine(lookedPartPosition))
                        manager.DiscoverPart(lookedPartPosition);

                }
            }
        }

        private void ToggleFlagEvent(Object o, MouseButtonEventArgs e)
        {
            if (discovered)
                return;
            if (!flagged)
            {
                flagged = true;
                flagImage.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                flagged = false;
                flagImage.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void ClickEvent(Object o, MouseButtonEventArgs e)
        {
            if (discovered)
                return;

            if (saperTile == SaperTileType.None)
            {
                saperTile = SaperTileType.Empty;
                manager.FirstClick();
                Discover();
            }
            else if (!flagged)
            {
                if (saperTile == SaperTileType.Mine)
                    manager.GameOver();

                else
                    Discover();
            }
        }


    }
    public enum SaperTileType
    {
        None,
        Empty,
        Mine
    }
}
