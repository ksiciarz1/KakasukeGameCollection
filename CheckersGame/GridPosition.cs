using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame
{
    internal struct GridPosition
    {
        public int x;
        public int y;

        public GridPosition()
        {
            x = 0;
            y = 0;
        }
        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public GridPosition(GridPosition gridPosition)
        {
            x = gridPosition.x;
            y = gridPosition.y;
        }
    }
}
