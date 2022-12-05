using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SaperGame
{
    /// <summary>
    /// Interaction logic for AdvancedStart.xaml
    /// </summary>
    public partial class AdvancedStart : Window
    {
        private SaperWindow parent;

        internal AdvancedStart(SaperWindow parent)
        {
            this.parent = parent;
            InitializeComponent();

            Closed += (s, e) =>
            {
                parent.AdvancedSettingsClosed();
            };

        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            int width = Convert.ToInt16(WidthTextBox.Text);
            int height = Convert.ToInt16(HeightTextBox.Text);
            int minesPercent = Convert.ToInt16(MinesTextBox.Text);

            if (width > 30)
                width = 30;
            else if (width < 0)
                width = 1;
            if (height > 30)
                height = 30;
            else if (height < 0)
                height = 0;
            if (minesPercent > 90)
                minesPercent = 90;
            else if (minesPercent < 5)
                minesPercent = 5;

            parent.AdvancedSettingConfirmed(width, height, minesPercent);
            Close();
        }
    }
}
