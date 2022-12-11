using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
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
        private CheckerPiece? selectedPiece;
        private List<Take> takes = new List<Take>();
        /// <summary>
        /// Tiles selected for avaible move
        /// </summary>
        private List<TileStatus> selectedTiles = new List<TileStatus>();
        private TileStatus[,] tiles = new TileStatus[8, 8];

        internal readonly Grid GameGrid;
        private bool redTurn = true;
        private bool takenThisTurn = false;
        private CheckerPiece? comboPiece;

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
                && selectedPiece != null)
            {
                if (takes.Count > 0)
                {
                    Take? take = takes.Find(take =>
                    {
                        return take.TakingPiece == selectedPiece && take.TileToMove == tile;
                    });
                    if (take != null)
                    {
                        take.PieceIsTaking();
                        takenThisTurn = true;
                    }
                    takes.Clear();
                }

                foreach (TileStatus selectedTile in selectedTiles)
                    selectedTile.Unselect();

                selectedPiece.MoveImage(tile.gridPosition);
                selectedPiece.parentTile = tile;
                tile.checkerPiece = selectedPiece;

                HandleTurns();

                selectedPiece = null;
                checkersLeft -= takes.Count;

            }
            if (checkersLeft <= 0)
                WinLose();
        }

        private void HandleTurns()
        {
            if (CheckIfTakingIsPossible(selectedPiece) && takenThisTurn)
                comboPiece = selectedPiece;

            else
            {
                redTurn = !redTurn;
                selectedPiece = null;
                takes.Clear();
                comboPiece = null;
                takenThisTurn = false;
            }
        }

        /// <summary>
        /// Selects the piece and hightlights movable tiles
        /// </summary>
        /// <param name="piece">Selected piece</param>
        private void SelectPiece(CheckerPiece piece)
        {
            selectedPiece?.Unselect();
            if (comboPiece != null)
            {
                if (piece == comboPiece)
                {
                    selectedPiece = comboPiece;
                    piece.Select();
                    SelectSurroundingTiles(piece);
                }
            }
            else if ((piece.Color == CheckerColor.Red && redTurn)
                || (piece.Color == CheckerColor.White && !redTurn))
            {
                piece.Select();
                selectedPiece = piece;
                SelectSurroundingTiles(piece);
            }
        }

        private void SelectSurroundingTiles(CheckerPiece piece)
        {
            foreach (TileStatus tile in selectedTiles)
                tile.Unselect();

            int x = piece.gridPosition.x;
            int y = piece.gridPosition.y;

            for (int i = -1; i < 2; i += 2)
                for (int j = -1; j < 2; j += 2)
                {
                    if (i == 0 && j == 0) // Can't move to itself
                        continue;
                    if (!piece.promoted && // Can't move backwards if not promoted
                        ((piece.Color == CheckerColor.White && j > 0) || (piece.Color == CheckerColor.Red && j < 0)))
                        continue;

                    try
                    {
                        TileStatus tile = tiles[x - i, y - j];
                        if (tile.checkerPiece == null) // Target tile is empty
                            SelectTile(tile);
                        else if (tile.checkerPiece.Color != piece.Color)
                            TakeAttempt(piece, i, j, tile);
                    }
                    catch (IndexOutOfRangeException) { }
                }

        }

        private TileStatus TakeAttempt(CheckerPiece piece, int i, int j, TileStatus tile)
        {
            int x = piece.gridPosition.x;
            int y = piece.gridPosition.y;
            TileStatus tileToDelete = tile;
            tile = tiles[x - 2 * i, y - 2 * j];
            if (tile.checkerPiece == null)
            {
                SelectTile(tile);
                takes.Add(new Take(piece, tile, tileToDelete.checkerPiece));
            }

            return tile;
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
                        if (i == 0 && j == 0)
                            continue;
                        if (!piece.promoted && // Can't move backwards if not promoted
                        ((piece.Color == CheckerColor.White && j > 0) || (piece.Color == CheckerColor.Red && j < 0)))
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
