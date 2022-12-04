using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SaperGame
{
    class Saper
    {
        private Grid mainGameGrid;
        private readonly KeyValuePair<int, int> gridDimentions;
        private SaperPart[,] saperParts;
        private const int minePercent = 5;
        GameOverEvent onGameOver;

        public Saper(int x, int y, SaperWindow myWindow)
        {
            mainGameGrid = myWindow.MainGameGrid;
            gridDimentions = new KeyValuePair<int, int>(x, y);
            saperParts = new SaperPart[x, y];
            myWindow.SetWindowSize(x, y);

            for (int i = 0; i < x; i++)
            {
                mainGameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < y; j++)
            {
                mainGameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    saperParts[i, j] = new SaperPart(i, j, mainGameGrid, this);
                }
            }


        }
        public void GameOver()
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
        public void FirstClick()
        {
            Random random = new Random();

            for (int i = 0; i < gridDimentions.Key; i++)
            {
                for (int j = 0; j < gridDimentions.Value; j++)
                {
                    if (saperParts[i, j].saperTile == SaperTileType.None)
                    {
                        if (random.Next(101) <= minePercent)
                            saperParts[i, j].saperTile = SaperTileType.Mine;
                        else
                            saperParts[i, j].saperTile = SaperTileType.Empty;
                    }
                }
            }

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
                    minesSurrounding = 0;
                }
            }
        }

        public bool CheckIfPartHaveMine(KeyValuePair<int, int> gridPosition)
        {
            try
            {
                if (saperParts[gridPosition.Key, gridPosition.Value].saperTile == SaperTileType.Mine)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public void DiscoverPart(KeyValuePair<int, int> gridPosition)
        {
            try
            {
                saperParts[gridPosition.Key, gridPosition.Value].Discover();
            }
            catch { }
        }

        public delegate void GameOverEvent();
    }
}
