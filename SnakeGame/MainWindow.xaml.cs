﻿using System;
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

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isFullscreen;
        readonly Window parent; // Main window with game select
        Snake? snake;
        public bool running = true; // running game loop

        public MainWindow()
        {
            InitializeComponent();
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized) // TODO: Make this load up and save up, with resolution in settings
                isFullscreen = true;
            else
                isFullscreen = false;
            Focus();
            StartGame();
        }
        public MainWindow(Window parent)
        {
            this.parent = parent;
            InitializeComponent();
            if (WindowState == WindowState.Maximized || WindowState == WindowState.Minimized) // TODO: Make this load up and save up, with resolution in settings
                isFullscreen = true;
            else
                isFullscreen = false;

            Visibility = Visibility.Visible;
            Focus();
            StartGame();
        }

        private void StartGame()
        {
            KeyValuePair<int, int> position = new KeyValuePair<int, int>(new Random().Next(2, 13), new Random().Next(2, 13));
            snake = new Snake(position, MainGameGrid);

            snake.StartGameLoop();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (parent != null) // HACK
            {
                parent.Visibility = Visibility.Visible; // Bring up the main window for game select before closing this window
                parent.Focus();
            }
            Close();
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                isFullscreen = false;
            }
            else
            {
                WindowState = WindowState.Maximized;
                isFullscreen = true;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                if (isFullscreen)
                    WindowState = WindowState.Maximized;
                else
                    WindowState = WindowState.Normal;
            }
            else
                WindowState = WindowState.Minimized;
        }
    }
}
