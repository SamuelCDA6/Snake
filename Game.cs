using System;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace Snake
{
    public class Game
    {
        private int _GameWidth, _GameHeight, _Score = 0, _HighScore;
        private Random _RandNb = new();
        private Snake _Snake1;
        private Pixel _Fruit = new(0,0,2);
        private Stopwatch _PlayTime = new();

        private static AudioManager _PowerUp = new AudioManager(System.AppDomain.CurrentDomain.BaseDirectory + "Sounds/SFX_powerUp6.wav");
        private static AudioManager _Lose = new AudioManager(System.AppDomain.CurrentDomain.BaseDirectory + "Sounds/StingerFail3a.wav");

        public int GameWidth { get => _GameWidth; private set => _GameWidth = value; }
        public int GameHeight { get => _GameHeight; private set => _GameHeight = value; }
        public Random RandNb { get => _RandNb; private set => _RandNb = value; }
        public Snake Snake1 { get => _Snake1; private set => _Snake1 = value; }
        public int Score { get => _Score; private set => _Score = value; }
        public Pixel Fruit { get => _Fruit; private set => _Fruit = value; }
        public Stopwatch PlayTime { get => _PlayTime; private set => _PlayTime = value; }
        public int HighScore { get => _HighScore; private set => _HighScore = value; }
        public static AudioManager PowerUp { get => _PowerUp; private set => _PowerUp = value; }
        public static AudioManager Lose { get => _Lose; private set => _Lose = value; }

        public Game(int screenwidth, int screenheight)
        {
            GameWidth = screenwidth;
            GameHeight = screenheight;
        }

        public void Start()
        {
            InitDisplay();

            do
            {
                // Higher is the score, smaller is the game loop
                Thread.Sleep((Score < 350)? 400 - Score : 50);

                Input();

                // Update the timer displayed
                WriteText($"{String.Format("{0:00}:{1:00}", PlayTime.Elapsed.Minutes, PlayTime.Elapsed.Seconds)}", 9, 4, ConsoleColor.Gray);
                
                // Update the score and display it 
                // WriteText($"{Score += 1}", 10, 8);

                if (Score > HighScore)
                {
                    WriteText($"{Score}", 14, 6);
                }

                MoveSnake();

            } while (Snake1.IsAlive);
                        
            PlayTime.Stop();

            GameOver();
            Console.ReadKey();

        }

        /// <summary>
        /// Initialize display for the grid and various data
        /// </summary>
        private void InitDisplay()
        {
            Console.Clear();
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(GameWidth + 21, GameHeight + 2);
                Console.SetBufferSize(GameWidth + 21, GameHeight + 2);
            }
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;

            Snake1 = new(new(GameWidth / 2 + 20, GameHeight / 2, 0));

            Console.ForegroundColor = ConsoleColor.Gray;
            HighScore = Menu.HighScore.GetBestScore();
            WriteText($"HighScore : {HighScore}", 2, 6);

            WriteText($"Time : {String.Format("{0:00}:{1:00}", PlayTime.Elapsed.TotalMinutes, PlayTime.Elapsed.TotalSeconds)}", 2, 4, ConsoleColor.Gray);

            WriteText($"Score : {Score}", 2, 8);

            

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = GameWidth + 20; i > 19; i -= 2)
            {
                WriteText("■", i, 0);
                WriteText("■", i, GameHeight);
            }

            for (int i = GameHeight - 1; i > 0; i--)
            {
                WriteText("■", 20, i);
                WriteText("■", GameWidth + 20, i);
            }

            DisplayPixel(Snake1.Head);
            MakeFruit();
            PlayTime.Reset();
            PlayTime.Start();
        }

        /// <summary>
        /// Make a fruit a a random position
        /// </summary>
        private void MakeFruit()
        {
            do 
            {
                do
                {
                    Fruit.Xpos = RandNb.Next(21, GameWidth + 19);
                } while (Fruit.Xpos % 2 != 0);

                Fruit.Ypos = RandNb.Next(1, GameHeight);

                // Remake the fruit if it's within the snake
            } while (Snake1.Tail.Contains(new(Fruit.Xpos, Fruit.Ypos, 1)) || Snake1.Head.Equals(Fruit));

            DisplayPixel(Fruit);
        }

        /// <summary>
        /// Display a pixel at the pixel position in parameter with the shape appropriate
        /// </summary>
        /// <param name="p">Pixel to display</param>
        private void DisplayPixel(Pixel p)
        {
            Console.SetCursorPosition(p.Xpos, p.Ypos);

            // switch for displaying the right character depending of the type of pixel in parameter
            switch (p.Shape)
            {
                case 0: // Head
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    switch (Snake1.Dir) // 
                    {
                        case Snake.Direction.Up:
                            Console.WriteLine("\u25B2");
                            break;
                        case Snake.Direction.Down:
                            Console.WriteLine("\u25BC");
                            break;
                        case Snake.Direction.Left:
                            Console.WriteLine("\u25C0");
                            break;
                        case Snake.Direction.Right:
                            Console.WriteLine("\u25B6");
                            break;
                        default:
                            break;
                    }
                    break;

                case 1: // Tail
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\u25A0");
                    break;
                case 2: // Fruit
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\u25CF");
                    break;
                case 3: // Erase Pixel
                    Console.WriteLine(" ");
                    break;
                default:
                    break;
            }

        }

        private void WriteText(string text, int x, int y, ConsoleColor foreground)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = foreground;
            Console.Write(text);
        }
        private void WriteText(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        private void MoveSnake()
        {
            // Save the old snake head position with the shape of a tail for after
            Pixel SnakeOldPos = new(Snake1.Head, 1);

            // Change the head position
            switch (Snake1.Dir)
            {
                case Snake.Direction.Up:
                    Snake1.Head.Ypos--;
                    break;
                case Snake.Direction.Down:
                    Snake1.Head.Ypos++;
                    break;
                case Snake.Direction.Left:
                    Snake1.Head.Xpos -= 2;
                    break;
                case Snake.Direction.Right:
                    Snake1.Head.Xpos += 2;
                    break;
                default:
                    break;
            }

            TestSnakePosition(SnakeOldPos);

        }

        /// <summary>
        /// Test the new position of the snake 
        /// </summary>
        private void TestSnakePosition(Pixel snakeOldPos)
        {
            // If the head encounter the tail or the Border of the gamezone, Snake is dead
            Snake1.IsAlive = !Snake1.Tail.Contains(Snake1.Head)
                && Snake1.Head.Xpos > 20 && Snake1.Head.Xpos < GameWidth + 19
                && Snake1.Head.Ypos > 0 && Snake1.Head.Ypos < GameHeight;

            if (Snake1.IsAlive)
            {

                if (Snake1.Head.Equals(Fruit))
                {
                    PowerUp.PlaySound();
                    Snake1.AddTail(snakeOldPos);
                    MakeFruit();

                    if (Snake1.Tail.Count > 10)
                    {
                        //TODO BlueFruit
                    }
                    // Increase the score and display the new score
                    WriteText($"{Score += 10}", 10, 8, ConsoleColor.Gray);
                }
                // Display the new head
                DisplayPixel(Snake1.Head);

                // if the snake as a tail Remove the last tail part
                // and add a new tail part in the old snake position
                if (Snake1.Tail.Count != 0)
                {
                    WriteText(" ", Snake1.Tail[0].Xpos, Snake1.Tail[0].Ypos);
                    Snake1.Tail.RemoveAt(0);
                    Snake1.Tail.Add(snakeOldPos);
                    DisplayPixel(snakeOldPos);
                }
                else // Else Remove the old Head
                {
                    WriteText(" ", snakeOldPos.Xpos, snakeOldPos.Ypos);
                }
            }
        }

        /// <summary>
        /// Stock the keyboard input to change the snake direction
        /// </summary>
        private void Input()
        {
            if (!Console.KeyAvailable) return;

            ConsoleKey keyPressed = Console.ReadKey(true).Key;
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            } 

            switch (keyPressed)
            {
                case ConsoleKey.Escape:
                    break;
                case ConsoleKey.LeftArrow:
                    if (Snake1.Dir != Snake.Direction.Right)
                    {
                        Snake1.Dir = Snake.Direction.Left;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (Snake1.Dir != Snake.Direction.Down)
                    {
                        Snake1.Dir = Snake.Direction.Up;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (Snake1.Dir != Snake.Direction.Left)
                    {
                        Snake1.Dir = Snake.Direction.Right;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (Snake1.Dir != Snake.Direction.Up)
                    {
                        Snake1.Dir = Snake.Direction.Down;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// At the endgame store the score if better
        /// </summary>
        private void GameOver()
        {
            Lose.PlaySound();
            string path = @"HighScoreSnake.txt";
            HighScore highScore = new(path);

            WriteText("GAME OVER", GameWidth / 2 + 16, Console.BufferHeight / 2 - 1, ConsoleColor.Red);

            if (highScore.IsHighScore(Score))
            {                
                WriteText("Entrez votre nom :", 1, 12, ConsoleColor.Gray);

                Console.SetCursorPosition(6, 13);
                Console.CursorVisible = true;

                string name = Console.ReadLine();

                highScore.SaveScore(Score, name);
            }
        }
    }
}
