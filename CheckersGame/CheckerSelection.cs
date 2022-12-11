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
        private CheckerPiece? selectedPiece;
        internal CheckerPiece? SelectedPiece
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
        internal List<TileStatus> selectedToDelete = new List<TileStatus>();

        internal CheckerSelection() { }

        internal void SelectPiece(CheckerPiece piece)
        {
            SelectedPiece?.Unselect();
            selectedToDelete.Clear();
            SelectedPiece = piece;
        }
    }
}
