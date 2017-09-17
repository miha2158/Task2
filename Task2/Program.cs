using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task2
{
    public class Solver
    {
        private int startX;
        private int startY;

        private int endX;
        private int endY;

        private short[,] maze;
        private bool[,] visited;

        List<int> Endings = new List<int>(0);

        public Solver(int startX, int startY, int endX, int endY, short[,] maze)
        {
            this.maze = maze;
            visited = new bool[maze.GetLength(0), maze.GetLength(1)];
            for (int i0 = 0; i0 < maze.GetLength(0); i0++)
                for (int i1 = 0; i1 < maze.GetLength(1); i1++)
                    visited[i0, i1] = maze[i0, i1] == -1;

            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
        }

        public int startstep()
        {
            int result;
            step(startX, startY, out result, 0);
            if (Endings.Count != 0)
                return Endings.Min();
            return result;
        }
        public void step(int posX, int posY, out int price, int priceP)
        {
            price = maze[posX, posY];

            if (visited[posX, posY])
            {
                price = int.MaxValue;
                return;
            }
            visited[posX, posY] = true;

            if (posX == endX && posY == endY)
            {
                priceP += price; 
                Endings.Add(priceP);
                visited[posX, posY] = false;
                return;
            }
            priceP += price;

            var priceMin = new int[4];
            priceMin[0] = stepfast(posX, posY, 1, 0, priceP);
            priceMin[1] = stepfast(posX, posY, 0, 1, priceP);
            priceMin[2] = stepfast(posX, posY, -1, 0, priceP);
            priceMin[3] = stepfast(posX, posY, 0, -1, priceP);

            int pMin = priceMin.Min();
                price += pMin;
        }
        private int stepfast(int posX, int posY, int x, int y, int priceP)
        {
            int price = int.MaxValue;
            if (posX + x >= maze.GetLength(0) || posY + y >= maze.GetLength(1) || posX + x < 0 || posY + y < 0)
                return price;
            if (visited[posX + x, posY + y])
                return price;

            step(posX + x, posY + y, out price, priceP);
            
            return price;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] arr;
            {
                string input;
                using (var fs = new FileStream("INPUT.TXT", FileMode.OpenOrCreate))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        input = sr.ReadToEnd();
                    }
                }
                arr = input.Split('\n');
                arr = arr.Select(str => str.Trim()).Where(s => s != string.Empty).ToArray();
            }

            var dims = arr[0].Split(' ').Select(i => short.Parse(i)).ToArray();

            var fieldArray = new short[dims[0], dims[1]];
            int startX = 0, startY = 0, endX = 0, endY = 0;

            dims = arr[1].Split(' ').Select(i => short.Parse(i)).ToArray();
            
            for (int i0 = 0; i0 < fieldArray.GetLength(0); i0++)
                for (int i1 = 0; i1 < fieldArray.GetLength(1); i1++)
                {
                    switch (arr[i0 + 2][i1])
                    {
                        case '.':
                            fieldArray[i0, i1] = 0;
                            break;
                        case 'S':
                            fieldArray[i0, i1] = 0;
                            startX = i0;
                            startY = i1;
                            break;
                        case 'E':
                            fieldArray[i0, i1] = 0;
                            endX = i0;
                            endY = i1;
                            break;
                        case 'X':
                            fieldArray[i0, i1] = -1;
                            break;

                        case 'R':
                            fieldArray[i0, i1] = dims[0];
                            break;
                        case 'G':
                            fieldArray[i0, i1] = dims[1];
                            break;
                        case 'B':
                            fieldArray[i0, i1] = dims[2];
                            break;
                        case 'Y':
                            fieldArray[i0, i1] = dims[3];
                            break;
                    }
                }
            /*
            for (int i0 = 0; i0 < fieldArray.GetLength(0); i0++)
            {
                for (int i1 = 0; i1 < fieldArray.GetLength(1); i1++)
                    if(fieldArray[i0, i1] == -1)
                        Console.Write("{0} ", fieldArray[i0, i1]);
                else Console.Write(" {0} ", fieldArray[i0, i1]);
                Console.WriteLine();
            }
            */
            var res = new Solver(startX, startY, endX, endY, fieldArray);
            int result = res.startstep();



            using (var fs = new FileStream("OUTPUT.TXT", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {

                    if (result == 0 || result == int.MaxValue)
                        sw.Write("Sleep");
                    else
                        sw.Write(result);
                }
            }
        }
    }
}