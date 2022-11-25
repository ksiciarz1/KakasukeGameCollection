using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private List<SnakePart> parts = new List<SnakePart>(); // Parts of snake including head, body and tail
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
            gameGrid.KeyDown += KeyDownEvent;
        }

        public void AddPart()
        {
            if (parts.Count != 1) // Don't change head to body upon adding new part
                parts.Last().SetImage(SnakeImage.Body);
            parts.Add(new SnakePart(parts.Last().positionOnGrid, this, SnakeImage.Tail));
        }

        public void SetHeadDirection(SnakeDirection direction) => parts[0].directionToChangeTo = direction; // From keyboard

        /// <summary>
        /// One tick of Game Loop
        /// </summary>
        public void Tick()
        {
            SnakeMovement();
        }

        private void SnakeMovement() // TODO: snake parts can (shouldn't) be in the same place
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0) // skip head bcs head changes direction based on keyboard input
                    parts[i].directionToChangeTo = GetDirectionToPreviousPart(i);
            } // keep loops seperate - all parts need the correct direction before any moves
            foreach (SnakePart snakepart in parts)
            {
                try
                {
                    snakepart.MoveOnTick();
                }
                catch (Exception ex) when (ex is OutOfMapException || ex is PartsCollisionException)
                {
                    // TODO: Game Over
                    foreach (SnakePart snakePart in parts)
                    {
                        snakePart.Delete();
                    }
                    parts.Clear();
                    StopGameLoop();
                    break;
                }
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
                if (yDiff > 0) return SnakeDirection.Down;
                else return SnakeDirection.Up;
            }
            else // if previous part is on right on the grid the diff will be negative
            {
                if (xDiff > 0) return SnakeDirection.Right;
                else return SnakeDirection.Left;
            }
        }

        private async void GameLoop()
        {
            while (running)
            {
                Tick();
                await Task.Delay(512); // 8 for 60FPS
            }
        }
        public void StartGameLoop()
        {
            running = true;
            AddPart();
            AddPart();
            AddPart();
            AddPart();
            GameLoop();
        }
        void StopGameLoop() { running = false; }

        public void setGridPosition(Image image, KeyValuePair<int, int> position)
        {
            image.SetValue(Grid.ColumnProperty, position.Key);
            image.SetValue(Grid.RowProperty, position.Value);
        }

        /// <summary>
        /// Keyboard controls. Changes the snake head direction based on input
        /// </summary>
        public void KeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case (System.Windows.Input.Key.W):
                case (System.Windows.Input.Key.Up):
                    parts[0].SetDirection(SnakeDirection.Up);
                    break;
                case (System.Windows.Input.Key.D):
                case (System.Windows.Input.Key.Right):
                    parts[0].SetDirection(SnakeDirection.Right);
                    break;
                case (System.Windows.Input.Key.S):
                case (System.Windows.Input.Key.Down):
                    parts[0].SetDirection(SnakeDirection.Down);
                    break;
                case (System.Windows.Input.Key.A):
                case (System.Windows.Input.Key.Left):
                    parts[0].SetDirection(SnakeDirection.Left);
                    break;
            }
        }
    }
}
