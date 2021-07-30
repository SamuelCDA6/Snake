using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Menu
    {
        private static string _Path = @"HighScoreSnake.txt";
        private static HighScore _HighScore = new(Path);             
        
        private static AudioManager _Sounds = new AudioManager(System.AppDomain.CurrentDomain.BaseDirectory + "Sounds/SFX_UIGeneric2.wav");
        
        public static string Path { get => _Path; private set => _Path = value; }
        public static HighScore HighScore { get => _HighScore; private set => _HighScore = value; }
        public static AudioManager Sounds { get => _Sounds; private set => _Sounds = value; }
        

        public static void Start()
        {
            // Music.PlayMusic();
            string[] menu = { "   Play   ", "HighScores", "   Exit   " };
            ConsoleKey keyPressed;
            int posCursor;

            do
            {

                InitDisplay();

                posCursor = 0;
                do
                {
                    keyPressed = Console.ReadKey().Key;
                    if (keyPressed == ConsoleKey.UpArrow)
                    {                        
                        WriteText(menu[posCursor], 25, 10 + posCursor * 4, ConsoleColor.White);
                        posCursor = posCursor == 0 ? 2 : posCursor - 1;
                        WriteText(menu[posCursor], 25, 10 + posCursor * 4, ConsoleColor.Green);
                    }

                    if (keyPressed == ConsoleKey.DownArrow)
                    {
                        WriteText(menu[posCursor], 25, 10 + posCursor * 4, ConsoleColor.White);
                        posCursor = posCursor == 2 ? 0 : posCursor + 1;
                        WriteText(menu[posCursor], 25, 10 + posCursor * 4, ConsoleColor.Green);
                    }

                    Sounds.PlaySound();
                } while (keyPressed != ConsoleKey.Enter);

                switch (posCursor)
                {
                    case 0:
                        Game game = new(48, 24);
                        game.Start();
                        break;
                    case 1:
                        HighScoreMenu();
                        break;
                    default:
                        break;
                }

            } while (posCursor != 2);
            Console.ForegroundColor = ConsoleColor.Black;
        }

        private static void InitDisplay()
        {
            Console.Clear();
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(60, 25);
                Console.SetBufferSize(60, 25);
            }
            Console.CursorVisible = false;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteText(" ________________________________ ", 13, 0);
            WriteText("|                                |", 13, 1);
            WriteText("|     ¤  Snake By Samuel  ¤      |", 13, 2);
            WriteText("|________________________________|", 13, 3);

            Console.ForegroundColor = ConsoleColor.Green;
            WriteText("   Play   ", 25, 10);
            Console.ForegroundColor = ConsoleColor.White;
            WriteText("HighScores", 25, 14);
            WriteText("   Exit   ", 25, 18);

        }

        private static void WriteText(string text, int x, int y, ConsoleColor foreground)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = foreground;
            Console.Write(text);
        }
        private static void WriteText(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        private static void HighScoreMenu()
        {
            Console.Clear();
            HighScore.Scores.Clear();
            HighScore.MakeHighScoreList();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteText(" ________________________________ ", 13, 0);
            WriteText("|                                |", 13, 1);
            WriteText("|     ¤  HighScore Snake  ¤      |", 13, 2);
            WriteText("|________________________________|", 13, 3);

            Console.ForegroundColor = ConsoleColor.Gray;

            int y = 0;
            foreach (ScoreLine sl in HighScore.Scores)
            {
                WriteText($"{y + 1}.\t{sl.Score}", 20, 8 + y);
                WriteText($"{sl.Name}", 30, 8 + y++);
            }

            Console.ReadKey(false);
        }
    }
}
