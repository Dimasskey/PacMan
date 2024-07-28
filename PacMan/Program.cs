using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace PacMan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            bool isActive = true;
            char[,] map = ReadMap("map.txt");

            int pacmanX = 1;
            int pacmanY = 1;
            int score = 0;


            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            Task.Run(() =>
            {
                while(ExitGame(pressedKey, isActive, ref score))
                {
                    pressedKey = Console.ReadKey();

                }
            });

            while (ExitGame(pressedKey, isActive, ref score))
            {
                Console.Clear();

                HandleInput(pressedKey, ref pacmanX, ref pacmanY, map, ref score);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                DrawMap(map);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(pacmanX, pacmanY);
                Console.Write("@");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(27, 0);
                Console.Write($"Score: {score}");

                Thread.Sleep(500);
            }
            
            
        }

        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines("map.txt");

            char[,] map = new char[GetMaxLenghtOfLine(file), file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[y][x];

            return map;
                
            
        }

        private static bool ExitGame(ConsoleKeyInfo pressedKey, bool isActive, ref int score)
        {
            if (pressedKey.Key == ConsoleKey.Z)
            {
                Console.Clear();
                Console.Write($"Игра завершена.\n Вы набрали: {score}");
                Console.ReadKey();
                isActive = false;
            }

            return isActive;
        }

        private static void HandleInput(ConsoleKeyInfo pressedKey, ref int pacmanX, ref int pacmanY, char[,] map, ref int score)
        {
            int[] direction = GetDirection(pressedKey);

            int nextPacmanPositionX = pacmanX + direction[0];
            int nextPacmanPositionY = pacmanY + direction[1];

            char nextCell = map[nextPacmanPositionX, nextPacmanPositionY];

            if (nextCell == ' ' || nextCell == '.')
            {
                pacmanX = nextPacmanPositionX;
                pacmanY = nextPacmanPositionY;

                if (nextCell == '.')
                {
                    score += 1;
                    map[nextPacmanPositionX, nextPacmanPositionY] = ' ';
                }
            } 
        }

        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };

            if (pressedKey.Key == ConsoleKey.UpArrow)
                direction[1] = -1;
            else if (pressedKey.Key == ConsoleKey.DownArrow)
                direction[1] = 1;
            else if (pressedKey.Key == ConsoleKey.LeftArrow)
                direction[0] = -1;
            else if (pressedKey.Key == ConsoleKey.RightArrow)
                direction[0] = 1;

            return direction;
        }

        private static void DrawMap(char[,] map)
        {
            
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0;x < map.GetLength(0); x++)
                { 
                    Console.Write(map[x, y]);
                }
                Console.Write("\n");
            }
        }

        private static int GetMaxLenghtOfLine(string[] lines)
        {
            int maxLenght = lines[0].Length;

            foreach (var line in lines)
                if (line.Length > maxLenght)
                    maxLenght = line.Length;
            
            return maxLenght;
        }
    }
}
