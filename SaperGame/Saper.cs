using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SaperGame
{
    internal class Saper
    {
        private readonly Grid mainGameGrid;
        private readonly SaperWindow saperWindow;
        private readonly KeyValuePair<int, int> gridDimentions;

        private SaperPart[,] saperParts;
        private int mines = 0;
        private int flags = 0;
        private int minesPercent;
        internal GameOverEvent? onGameOver;

        internal Saper(int x, int y, int minesPercent, SaperWindow myWindow)
        {
            saperWindow = myWindow;
            mainGameGrid = myWindow.MainGameGrid;
            this.minesPercent = minesPercent;
            gridDimentions = new KeyValuePair<int, int>(x, y);
            saperParts = new SaperPart[x, y];

            // Creating grid
            for (int i = 0; i < x; i++)
            {
                mainGameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < y; j++)
            {
                mainGameGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Adding saper parts
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    saperParts[i, j] = new SaperPart(i, j, mainGameGrid, this);
                }
            }
        }
        internal void FirstClick()
        {
            Random random = new Random();

            // Setting tile type for each saper part
            for (int i = 0; i < gridDimentions.Key; i++)
            {
                for (int j = 0; j < gridDimentions.Value; j++)
                {
                    // To not set mine on first tile clicked
                    if (saperParts[i, j].saperTile == SaperTileType.None)
                    {
                        if (random.Next(101) <= minesPercent)
                        {
                            saperParts[i, j].saperTile = SaperTileType.Mine;
                            mines++;
                        }
                        else
                            saperParts[i, j].saperTile = SaperTileType.Empty;
                    }
                }
            }

            // Checking how many mines surround each part
            for (int i = 0; i < gridDimentions.Key; i++)
            {
                for (int j = 0; j < gridDimentions.Value; j++)
                {
                    int minesSurrounding = 0;
                    for (int l = -1; l <= 1; l++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            if (l == 0 && k == 0)
                                continue;
                            // Not to leave game grid dimentions
                            if ((i - l >= 0 && j - k >= 0) && (i - l < gridDimentions.Key && j - k < gridDimentions.Value))
                            {
                                if (saperParts[i - l, j - k].saperTile == SaperTileType.Mine)
                                    minesSurrounding++;
                            }
                        }
                    }
                    saperParts[i, j].surroundingMines = minesSurrounding;
                }
            }
            saperWindow.SetMinesLeft(mines);
        }
        internal void GameOver()
        {
            if (onGameOver != null)
                onGameOver.Invoke();

            for (int i = 0; i < gridDimentions.Key; i++)
            {
                for (int j = 0; j < gridDimentions.Value; j++)
                {
                    if (saperParts[i, j].saperTile == SaperTileType.Mine)
                        saperParts[i, j].Expload();
                    else
                        saperParts[i, j].Discover();
                }
            }
        }
        /// <summary>
        /// Checks if part in position have mine
        /// </summary>
        internal bool CheckIfPartHaveMine(KeyValuePair<int, int> gridPosition)
        {
            if (gridPosition.Key < 0 || gridPosition.Key >= gridDimentions.Key
                || gridPosition.Value < 0 || gridPosition.Value >= gridDimentions.Value)
                return false;
            if (saperParts[gridPosition.Key, gridPosition.Value].saperTile == SaperTileType.Mine)
                return true;

            return false;
        }
        internal void DiscoverPart(KeyValuePair<int, int> gridPosition)
        {
            if (gridPosition.Key < 0 || gridPosition.Key >= gridDimentions.Key
                || gridPosition.Value < 0 || gridPosition.Value >= gridDimentions.Value)
                return;
            saperParts[gridPosition.Key, gridPosition.Value].Discover();
        }
        /// <summary>
        /// Clears the grid
        /// </summary>
        internal void Delete()
        {
            foreach (SaperPart part in saperParts)
            {
                part.Delete();
            }

            mainGameGrid.Children.Clear();
            mainGameGrid.ColumnDefinitions.Clear();
            mainGameGrid.RowDefinitions.Clear();
        }
        /// <summary>
        /// Used to caluculate mines left to flag
        /// </summary>
        internal void ChangeFlagNumber(bool added)
        {
            if (added)
                flags++;
            else
                flags--;

            saperWindow.SetMinesLeft(mines - flags);

            if (mines == flags)
                Win();
        }

        private void Win()
        {
            foreach (SaperPart part in saperParts)
            {
                if (!part.flagged)
                {
                    if (part.saperTile == SaperTileType.Mine)
                    {
                        part.Expload();
                        break;
                    }
                    else
                        part.Discover();
                }
            }
        }

        internal delegate void GameOverEvent();
    }
}
