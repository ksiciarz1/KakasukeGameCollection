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
        private readonly Grid gameGrid;
        public bool Running { get => running; }
        private bool running = false;
        private Apple apple;
        public event ScoreAdded scoreAdded;

        public Snake(KeyValuePair<int, int> startingPosition, Grid gameGrid)
        {
            this.gameGrid = gameGrid;
            SnakePart head = new SnakePart(startingPosition, this);
            head.SetDirection(SnakeDirection.Right);
            parts.Add(head);
            gameGrid.KeyDown += KeyDownEvent;
            StartGameLoop();
        }

        public void AddPart()
        {
            if (parts.Count == 1) // Don't change head to body upon adding new part
                parts.Last().SetImage(SnakeImage.Head);
            else
                parts.Last().SetImage(SnakeImage.Body);
            parts.Add(new SnakePart(parts.Last().positionOnGrid, this, SnakeImage.Tail));
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
        public void SetHeadDirection(SnakeDirection direction) => parts[0].directionToChangeTo = direction; // From keyboard
        private void SnakeMovement()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0) // skip head bcs head changes direction based on keyboard input
                    parts[i].directionToChangeTo = GetDirectionToPreviousPart(i);
            }

            // Changing snake image if snake is turning
            for (int i = 1; i < parts.Count - 1; i++)
            {
                if (parts[i - 1].directionToChangeTo != parts[i].directionToChangeTo && parts[i].directionToChangeTo != SnakeDirection.None)
                {
                    switch (parts[i].directionToChangeTo)
                    {
                        case SnakeDirection.Left:
                            if (parts[i - 1].directionToChangeTo == SnakeDirection.Down)
                                parts[i].SetImage(SnakeImage.TurnRight);
                            else
                                parts[i].SetImage(SnakeImage.TurnLeft);
                            break;
                        case SnakeDirection.Up:
                            if (parts[i - 1].directionToChangeTo == SnakeDirection.Left)
                                parts[i].SetImage(SnakeImage.TurnRight);
                            else
                                parts[i].SetImage(SnakeImage.TurnLeft);
                            break;
                        case SnakeDirection.Right:
                            if (parts[i - 1].directionToChangeTo == SnakeDirection.Up)
                                parts[i].SetImage(SnakeImage.TurnRight);
                            else
                                parts[i].SetImage(SnakeImage.TurnLeft);
                            break;
                        case SnakeDirection.Down:
                            if (parts[i - 1].directionToChangeTo == SnakeDirection.Right)
                                parts[i].SetImage(SnakeImage.TurnRight);
                            else
                                parts[i].SetImage(SnakeImage.TurnLeft);
                            break;
                    }
                }
                else
                    parts[i].SetImage(SnakeImage.Body);
            }

            // Keep loops seperate - all parts need the correct direction before any moves
            foreach (SnakePart snakepart in parts)
            {
                try
                {
                    snakepart.MoveOnTick();
                }
                catch (Exception ex) when (ex is OutOfMapException || ex is PartsCollisionException)
                {
                    // TODO: Better Game Over
                    GameOver();
                    break;
                }
            }

            // Check if snake parts are coliding by checking if they have the same grid position
            KeyValuePair<int, int>[] snakePartsPositions = new KeyValuePair<int, int>[parts.Count];
            for (int i = 0; i < parts.Count; i++)
            {
                snakePartsPositions[i] = parts[i].positionOnGrid;
            }
            var duplicates = snakePartsPositions.GroupBy(x => x) // Group by positions
                .Where(g => g.Count() > 1) // When same position is occuring more than once
                .ToArray();
            if (duplicates.Length > 0)
                GameOver();
        }


        /// <summary>
        /// One tick of Game Loop
        /// </summary>
        public void Tick()
        {
            SnakeMovement();
            if (parts.Count > 0)
                if (parts[0].positionOnGrid.Key == apple.positionOnGrid.Key
                    && parts[0].positionOnGrid.Value == apple.positionOnGrid.Value)
                    AppleColided();
        }

        public void setGridPosition(Image image, KeyValuePair<int, int> position)
        {
            image.SetValue(Grid.ColumnProperty, position.Key);
            image.SetValue(Grid.RowProperty, position.Value);
        }

        private void StartGameLoop()
        {
            running = true;
            AddPart();
            CreateApple();
            GameLoop();
        }

        private void CreateApple()
        {
            bool foundGoodPosition = false;
            KeyValuePair<int, int> positionOngrid = new KeyValuePair<int, int>();
            while (!foundGoodPosition)
            {
                foundGoodPosition = true;
                Random rand = new Random();
                positionOngrid = new KeyValuePair<int, int>(rand.Next(16), rand.Next(16));
                foreach (SnakePart part in parts)
                {
                    if (part.positionOnGrid.Key == positionOngrid.Key
                        && part.positionOnGrid.Value == positionOngrid.Value)
                        foundGoodPosition = false;
                }
            }
            apple = new Apple(positionOngrid, gameGrid);
        }
        private void AppleColided()
        {
            apple.SnakeCollided();
            scoreAdded();
            AddPart();
            CreateApple();
        }

        private async void GameLoop()
        {
            while (running)
            {
                Tick();
                await Task.Delay(512); // 8 for 60FPS
            }
        }
        private void GameOver()
        {
            foreach (SnakePart snakePart in parts)
            {
                snakePart.Delete();
            }
            parts.Clear();
            StopGameLoop();
        }
        void StopGameLoop() { running = false; }

        public delegate void ScoreAdded();

        /// <summary>
        /// Keyboard controls. Changes the snake head direction based on input
        /// </summary>
        public void KeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.W:
                case System.Windows.Input.Key.Up:
                    parts[0].SetDirection(SnakeDirection.Up);
                    break;
                case System.Windows.Input.Key.D:
                case System.Windows.Input.Key.Right:
                    parts[0].SetDirection(SnakeDirection.Right);
                    break;
                case System.Windows.Input.Key.S:
                case System.Windows.Input.Key.Down:
                    parts[0].SetDirection(SnakeDirection.Down);
                    break;
                case System.Windows.Input.Key.A:
                case System.Windows.Input.Key.Left:
                    parts[0].SetDirection(SnakeDirection.Left);
                    break;
                case System.Windows.Input.Key.E: // HACK
                    AddPart();
                    break;
            }
        }
    }

}
