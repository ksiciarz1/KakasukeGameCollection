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
        internal CheckerPice SelectedPice
        {
            set
            {
                selectedPice = value;
                selectedToDelete.Clear();
            }
            get
            {
                return selectedPice;
            }
        }
        private CheckerPice selectedPice;
        internal List<CheckerPice> selectedToDelete = new List<CheckerPice>();

        internal CheckerSelection() { }
        internal CheckerSelection(CheckerPice selectedPice)
        {
            SelectedPice = selectedPice;
        }

        internal void DeleteSelectedPices()
        {
            throw new NotImplementedException();
        }

    }
}
