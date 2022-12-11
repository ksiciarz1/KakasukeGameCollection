using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CheckersGame
{
    internal class CheckerGameManager
    {
        private readonly CheckerWindow Parent;
        private int checkersLeft = 0;
        /// <summary>
        /// Object with selected piece and all pieces to delete after move
        /// </summary>
        private CheckerSelection checkerSelection = new CheckerSelection();
        /// <summary>
        /// Tiles selected for avaible move
        /// </summary>
        private List<TileStatus> selectedTiles = new List<TileStatus>();
        private TileStatus[,] tiles = new TileStatus[8, 8];

        private bool redTurn = true;
        private CheckerPiece? comboPiece;

        internal readonly Grid GameGrid;

        public CheckerGameManager(CheckerWindow parent)
        {
            Parent = parent;
            GameGrid = parent.GameGrid;

            // Creating pieces and Tiles
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    tiles[i, j] = new TileStatus(new GridPosition(i, j), GameGrid, this);
                    TileStatus currentTile = tiles[i, j];
                    currentTile.onTileClicked += ConfirmMove;

                    if (!((i + j) % 2 == 0))
                    {
                        if (j < 3) // White Piece
                        {
                            currentTile.AddPiece(CheckerColor.White);
                            currentTile.checkerPiece.onPieceSelected += SelectPiece;
                            checkersLeft++;
                        }
                        else if (j > 4) // Red Piece
                        {
                            currentTile.AddPiece(CheckerColor.Red);
                            currentTile.checkerPiece.onPieceSelected += SelectPiece;
                            checkersLeft++;
                        }
                    }
                }

        }

        /// <summary>
        /// When a piece is selected, tile click confirms piece move
        /// </summary>
        /// <param name="tile">Tile that activated this</param>
        private void ConfirmMove(TileStatus tile)
        {
            if (tile.checkerPiece == null
                && checkerSelection.SelectedPiece != null)
            {
                CheckerPiece tileCheckerPiece = checkerSelection.SelectedPiece;
                tileCheckerPiece.MoveImage(tile.gridPosition);
                tileCheckerPiece.parentTile = tile;
                tile.checkerPiece = tileCheckerPiece;
                checkerSelection.SelectedPiece = null;

                if (CheckIfTakingIsPossible(tileCheckerPiece) || checkerSelection.selectedToDelete.Count != 0)
                    comboPiece = tileCheckerPiece;

                else
                    NextTurn();


                checkersLeft -= checkerSelection.selectedToDelete.Count;
                foreach (TileStatus pieceToDelete in checkerSelection.selectedToDelete)
                    pieceToDelete.checkerPiece.Delete();

                foreach (TileStatus selectedTile in selectedTiles)
                    selectedTile.Unselect();

            }
            if (checkersLeft <= 0)
                WinLose();
        }

        private void NextTurn()
        {
            redTurn = !redTurn;
            comboPiece = null;
        }


        /// <summary>
        /// Selects the piece and hightlights movable tiles
        /// </summary>
        /// <param name="piece">Selected piece</param>
        private void SelectPiece(CheckerPiece piece)
        {
            if (comboPiece != null && piece != comboPiece)
                return;

            else if ((piece.Color == CheckerColor.Red && redTurn)
                || (piece.Color == CheckerColor.White && !redTurn))
            {
                piece.Select();
                checkerSelection.SelectPiece(piece);
                SelectSurroundingTiles(piece);
            }
        }

        private void SelectSurroundingTiles(CheckerPiece piece)
        {
            if (selectedTiles.Count > 0)
                foreach (TileStatus tile in selectedTiles)
                    tile.Unselect();

            int x = piece.gridPosition.x;
            int y = piece.gridPosition.y;

            for (int i = -1; i < 2; i += 2)
                for (int j = -1; j < 2; j += 2)
                {
                    if (i == 0 && j == 0)
                        continue;

                    try
                    {
                        TileStatus tile = tiles[x - i, y - j];
                        if (tile.checkerPiece == null) // Target tile is empty
                        {
                            if (piece.promoted)
                            {
                                SelectTile(tile);
                                continue;
                            }
                            // Moving only in forward direction according to piece
                            else if (piece.Color == CheckerColor.White
                                && j < 0
                                || piece.Color == CheckerColor.Red
                                && j > 0)
                            {
                                SelectTile(tile);
                            }
                        }
                        else if (tile.checkerPiece.Color != piece.Color) // Take attempt
                        {
                            TileStatus tileToDelete = tile;
                            tile = tiles[x - 2 * i, y - 2 * j];
                            if (tile.checkerPiece == null &&
                                (piece.Color == CheckerColor.White
                                && j < 0
                                || piece.Color == CheckerColor.Red
                                && j > 0))
                            {
                                SelectTile(tile);
                                checkerSelection.selectedToDelete.Add(tileToDelete);
                            }
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                }

        }

        private void SelectTile(TileStatus tile)
        {
            tile.Select();
            selectedTiles.Add(tile);
        }

        private bool CheckIfTakingIsPossible(CheckerPiece piece)
        {
            bool isPossible = false;
            if (redTurn && piece.Color == CheckerColor.Red
                || !redTurn && piece.Color == CheckerColor.White)
            {
                int x = piece.gridPosition.x;
                int y = piece.gridPosition.y;

                for (int i = -1; i < 2; i += 2)
                    for (int j = -1; j < 2; j += 2)
                    {
                        if (i == 0 && j == 0
                            || (piece.Color == CheckerColor.White && j < 0)
                            || (piece.Color == CheckerColor.Red && j > 0))
                            continue;

                        try
                        {
                            TileStatus tile = tiles[x - i, y - j];
                            if (tile.checkerPiece == null) // Target tile is empty
                                continue;
                            else if (tile.checkerPiece.Color != piece.Color) // Take attempt
                            {
                                tile = tiles[x - 2 * i, y - 2 * j];
                                if (tile.checkerPiece == null)
                                    isPossible = true;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
            }
            return isPossible;
        }

        private void WinLose()
        {
            Delete();
            Parent.NewGame();
        }

        private void Delete()
        {
            foreach (TileStatus tile in tiles)
            {
                tile.Delete();
            }
        }
    }

}
