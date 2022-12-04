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
    internal class SaperPart
    {
        private readonly Image tileImage;
        private readonly Image mineImage;
        private readonly Image flagImage;
        private readonly Label label;
        private readonly Grid mainGameGrid;
        private readonly Saper manager;

        private readonly BitmapImage tile = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Tile.png")));
        private readonly BitmapImage tileDiscovered = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/TileDiscovered.png")));
        private readonly BitmapImage mine = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Mine.png")));
        private readonly BitmapImage mineExploaded = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/MineExploaded.png")));
        private readonly BitmapImage flag = new BitmapImage(new Uri(Path.GetFullPath(@"./Resources/Flag.png")));

        private bool discovered = false;
        private KeyValuePair<int, int> gridPosition;

        internal bool flagged = false;
        internal int surroundingMines;
        internal SaperTileType saperTile = SaperTileType.None;

        internal SaperPart(int x, int y, Grid mainGameGrid, Saper manager)
        {
            this.mainGameGrid = mainGameGrid;
            this.manager = manager;
            gridPosition = new KeyValuePair<int, int>(x, y);

            tileImage = new Image();
            tileImage.Source = tile;
            mainGameGrid.Children.Add(tileImage);
            tileImage.SetValue(Grid.ColumnProperty, x);
            tileImage.SetValue(Grid.RowProperty, y);

            mineImage = new Image();
            mineImage.Source = mine;
            mainGameGrid.Children.Add(mineImage);
            mineImage.SetValue(Grid.ColumnProperty, x);
            mineImage.SetValue(Grid.RowProperty, y);

            flagImage = new Image();
            flagImage.Source = flag;
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
        internal void Expload()
        {
            mineImage.Source = mineExploaded;
            mineImage.Visibility = System.Windows.Visibility.Visible;
            discovered = true;
        }
        internal void Discover()
        {
            if (discovered)
                return;

            discovered = true;
            tileImage.Source = tileDiscovered;
            label.Content = surroundingMines;

            if (surroundingMines != 0)
                label.Visibility = System.Windows.Visibility.Visible;

            else
                CheckSurroundForMines();

        }
        internal void Delete()
        {
            tileImage.Source = null;
            mineImage.Source = null;
            flagImage.Source = null;
            label.Content = null;
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
            manager.ChangeFlagNumber(flagged);
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
    internal enum SaperTileType
    {
        None,
        Empty,
        Mine
    }
}
