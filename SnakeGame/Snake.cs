using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SnakeGame
{
    internal class Snake
    {
        private List<SnakePart> parts = new List<SnakePart>();
        public Grid GameGrid { get => gameGrid; }
        private Grid gameGrid;
        public bool Running { get => running; }
        private bool running = false;

        public Snake(KeyValuePair<int, int> startingPosition, Grid gameGrid)
        {
            this.gameGrid = gameGrid;
            SnakePart head = new SnakePart(startingPosition, this);
            head.SetDirection(SnakeDirection.Right);
            parts.Add(head);
        }

        public void AddPart()
        {
            parts.Add(new SnakePart(parts.Last().positionOnGrid, this));
        }

        public void SetHeadDirection(SnakeDirection direction) => parts[0].SnakeDirection = direction; // From keyboard

        public void Tick()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0) // skip head
                    parts[i].SnakeDirection = GetDirectionToPreviousPart(i);
                parts[i].MoveOnTick();
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
            else if (xDiff == 0) // if previous part is lower on the grid the diff will be negative
            {
                if (yDiff < 0) return SnakeDirection.Down;
                else return SnakeDirection.Up;
            }
            else // if previous part is on right on the grid the diff will be negative
            {
                if (xDiff < 0) return SnakeDirection.Right;
                else return SnakeDirection.Left;
            }
        }

        void StopGameLoop() { running = false; }

        public void StartGameLoop()
        {
            running = true;
            GameLoop();
        }

        private async void GameLoop()
        {
            while (running)
            {
                Tick();
                await Task.Delay(512); // 8 for 60FPS
            }
        }

        public void setGridPosition(Image image, KeyValuePair<int, int> position)
        {
            image.SetValue(Grid.ColumnProperty, position.Key);
            image.SetValue(Grid.RowProperty, position.Value);
        }

    }
}
