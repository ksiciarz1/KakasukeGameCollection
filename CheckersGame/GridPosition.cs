using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame
{
    public struct GridPosition
    {
        public int x = 0;
        public int y = 0;

        public GridPosition() { }
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


        public static bool operator ==(GridPosition grid1, GridPosition grid2)
        {
            if ((grid1.x == grid2.x) && (grid1.y == grid2.y))
            {

            }
            return (grid1.x == grid2.x) && (grid1.y == grid2.y);
        }
        public static bool operator !=(GridPosition grid1, GridPosition grid2)
        {
            return !(grid1 == grid2);
        }
        public static GridPosition Empty = new GridPosition();

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj is GridPosition)
            {
                return Equals((GridPosition)obj);
            }
            return false;
        }
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
