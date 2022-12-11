using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame
{
    /// <summary>
    /// Class represeting a take posibility
    /// </summary>
    internal class Take
    {
        internal CheckerPiece TakingPiece { get; init; }
        internal TileStatus TileToMove { get; init; } // Piece will move to this tile if taking
        internal CheckerPiece TakenPiece { get; init; }

        internal Take(CheckerPiece takingPiece, TileStatus tileToMove, CheckerPiece takenPiece)
        {
            TakingPiece = takingPiece;
            TileToMove = tileToMove;
            TakenPiece = takenPiece;
        }

        internal void PieceIsTaking()
        {
            TakingPiece.Unselect();
            TakenPiece.Delete();
        }
    }
}
