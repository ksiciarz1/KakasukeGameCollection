using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaperGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SaperWindow : Window
    {
        private Saper saper;
        Window? parent;
        public SaperWindow()
        {
            InitializeComponent();

            Random rand = new Random();
            int columnSize = rand.Next(8, 20);
            int rowSize = rand.Next(8, 20);
            saper = new Saper(columnSize, rowSize, this);
        }

        public SaperWindow(Window parent) : this()
        {
            this.parent = parent;
        }

        internal void SetWindowSize(int x, int y)
        {
            myWindow.Width = x * 35;
            myWindow.Height = y * 35;
        }
    }
}
