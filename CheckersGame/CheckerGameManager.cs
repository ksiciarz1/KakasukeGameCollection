using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CheckersGame
{
    internal class CheckerGameManager
    {
        private readonly CheckerWindow Parent;
        private List<CheckerPice> checkerPices = new List<CheckerPice>();
        private CheckerSelection checkerSelection = new CheckerSelection();
        private List<TileStatus> selectedTiles = new List<TileStatus>();
        private TileStatus[,] tiles = new TileStatus[8, 8];

        internal readonly Grid GameGrid;

        public CheckerGameManager(CheckerWindow parent)
        {
            Parent = parent;
            GameGrid = parent.GameGrid;

            // Creating pices
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!((i + j) % 2 == 0))
                    {
                        GridPosition currentPosition = new GridPosition(i, j);
                        if (j < 3) // White
                        {
                            CheckerPice pice = new CheckerPice(currentPosition, CheckerColor.White, this);
                            checkerPices.Add(pice);
                            pice.onPiceSelected += SelectPice;
                        }
                        else if (j > 4) // Red
                        {
                            CheckerPice pice = new CheckerPice(currentPosition, CheckerColor.Red, this);
                            checkerPices.Add(pice);
                            pice.onPiceSelected += SelectPice;
                        }
                    }

                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tiles[i, j] = new TileStatus(new GridPosition(i, j), GameGrid);
                    if ((i + j) % 2 == 0)
                        if (!(j < 3 && j > 4))
                            tiles[i, j].occupied = true;
                }
            }
        }

        /// <summary>
        /// Selects the pice and hightlights movable tiles
        /// </summary>
        /// <param name="pice">Selected pice</param>
        private void SelectPice(CheckerPice pice)
        {
            if (checkerSelection.SelectedPice != null)
                checkerSelection.SelectedPice.Unselect();
            checkerSelection.SelectedPice = pice;

            SelectSurroundingTiles(pice);
        }

        private void SelectSurroundingTiles(CheckerPice pice)
        {
            // TODO: Select only tiles in front of pice
            if (selectedTiles.Count > 0)
                foreach (TileStatus tile in selectedTiles)
                    tile.Unselect();

            int x = pice.gridPosition.x;
            int y = pice.gridPosition.y;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if ((x - i >= 0 && x - i < 8)
                        && (y - j >= 0 && y - j < 8))
                    {
                        if (!tiles[x - i, y - j].occupied)
                        {
                            tiles[x - i, y - j].Select();
                            selectedTiles.Add(tiles[x - i, y - j]);
                        }
                    }
                }
            }
        }

    }
}
