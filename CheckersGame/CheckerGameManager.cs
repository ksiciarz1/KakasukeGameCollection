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
        private List<CheckerPiece> checkerPieces = new List<CheckerPiece>();
        private CheckerSelection checkerSelection = new CheckerSelection();
        private List<TileStatus> selectedTiles = new List<TileStatus>();
        private TileStatus[,] tiles = new TileStatus[8, 8];

        internal readonly Grid GameGrid;

        public CheckerGameManager(CheckerWindow parent)
        {
            Parent = parent;
            GameGrid = parent.GameGrid;

            // Creating pieces and Tiles
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tiles[i, j] = new TileStatus(new GridPosition(i, j), GameGrid, this);
                    TileStatus currentTile = tiles[i, j];
                    currentTile.onTileClicked += ConfirmMove;

                    if (!((i + j) % 2 == 0))
                    {
                        GridPosition currentPosition = new GridPosition(i, j);
                        if (j < 3) // White Piece
                        {
                            currentTile.AddPiece(CheckerColor.White);
                            currentTile.checkerPiece.onPieceSelected += SelectPiece;
                        }
                        else if (j > 4) // Red Piece
                        {
                            currentTile.AddPiece(CheckerColor.Red);
                            currentTile.checkerPiece.onPieceSelected += SelectPiece;
                        }
                    }
                }
            }

        }

        private void ConfirmMove(TileStatus tile)
        {
            if (tile.checkerPiece == null)
            {
                if (selectedTiles.Contains(tile))
                {
                    if (checkerSelection.SelectedPiece != null)
                    {
                        CheckerPiece tileCheckerPiece = checkerSelection.SelectedPiece;
                        tileCheckerPiece.MoveImage(tile.gridPosition);
                        tileCheckerPiece.parentTile = tile;
                        tile.checkerPiece = tileCheckerPiece;
                        checkerSelection.SelectedPiece = null;

                        foreach (TileStatus pieceToDelete in checkerSelection.selectedToDelete)
                            pieceToDelete.checkerPiece.Delete();
                        checkerSelection.selectedToDelete.Clear();

                        foreach (TileStatus selectedTile in selectedTiles)
                            selectedTile.Unselect();
                        selectedTiles.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Selects the piece and hightlights movable tiles
        /// </summary>
        /// <param name="piece">Selected piece</param>
        private void SelectPiece(CheckerPiece piece)
        {
            checkerSelection.SelectedPiece?.Unselect();
            checkerSelection.selectedToDelete.Clear();
            checkerSelection.SelectedPiece = piece;

            SelectSurroundingTiles(piece);
        }

        private void SelectSurroundingTiles(CheckerPiece piece)
        {
            if (selectedTiles.Count > 0)
                foreach (TileStatus tile in selectedTiles)
                    tile.Unselect();

            int x = piece.gridPosition.x;
            int y = piece.gridPosition.y;

            for (int i = -1; i < 2; i += 2)
            {
                for (int j = -1; j < 2; j += 2)
                {
                    if (i == 0 && j == 0)
                        continue;
                    try
                    {
                        TileStatus tile = tiles[x - i, y - j];
                        if (tile.checkerPiece == null)
                        {
                            tile.Select();
                            selectedTiles.Add(tile);
                        }
                        else if (tile.checkerPiece.Color != piece.Color)
                        {
                            TileStatus tileToDelete = tile;
                            tile = tiles[x - 2 * i, y - 2 * j];
                            if (tile.checkerPiece == null)
                            {
                                tile.Select();
                                selectedTiles.Add(tile);
                                checkerSelection.selectedToDelete.Add(tileToDelete);
                            }
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }

        }
    }
}
