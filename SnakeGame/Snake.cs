using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Snake
    {
        List<SnakePart> parts = new List<SnakePart>();

        public Snake(KeyValuePair<int, int> startingPosition)
        {
            parts.Add(new SnakePart(startingPosition));
        }

        public void AddPart()
        {
            parts.Add(new SnakePart(parts.Last().positionOnGrid));
        }

        public void SetHeadDirection(SnakeDirection direction) => parts[0].SnakeDirection = direction; // From keyboard

        public void Tick()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0) // skip head
                    parts[i].SnakeDirection = GetDirectionToPreviousPart(i);
                parts[i].MovedOnTick();
            }
        }

        /// <summary>
        /// Get direction to previous snake part
        /// </summary>
        /// <param name="index">Index of current part</param>
        private SnakeDirection GetDirectionToPreviousPart(int index)
        {
            int xDiff = parts[index - 1].positionOnGrid.Key - parts[index].positionOnGrid.Key;
            int yDiff = parts[index - 1].positionOnGrid.Value - parts[index].positionOnGrid.Value;

            if (xDiff == 0 && yDiff == 0)
            {
                // Parts are in the same place
                return SnakeDirection.None;
            }
            else if (xDiff == 0) // if previous part is lower on the grid the diff will be positive
            {
                if (yDiff > 0) return SnakeDirection.Down;
                else return SnakeDirection.Up;
            }
            else // if previous part is on right on the grid the diff will be positive
            {
                if (xDiff > 0) return SnakeDirection.Right;
                else return SnakeDirection.Left;
            }
        }
    }
}
