using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CheckersGame
{
    internal class CheckerSelection
    {
        internal CheckerPiece SelectedPiece
        {
            set
            {
                selectedPiece = value;
            }
            get
            {
                return selectedPiece;
            }
        }
        private CheckerPiece selectedPiece;
        internal List<TileStatus> selectedToDelete = new List<TileStatus>();

        internal CheckerSelection() { }
        internal CheckerSelection(CheckerPiece selectedPiece)
        {
            SelectedPiece = selectedPiece;
        }
    }
}
