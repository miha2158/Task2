using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task2
{
    class Program
    {
        static int Solve(ushort startX, ushort startY, ushort endX, ushort endY, short[,] maze)
        {
            var visited = new bool?[maze.GetLength(0), maze.GetLength(1)];

            for (int i0 = 0; i0 < maze.GetLength(0); i0++)
                for (int i1 = 0; i1 < maze.GetLength(1); i1++)
                    visited[i0, i1] = maze[i0, i1] == 0? (bool?)true: null;

            int price;
            step(startX, startY, endX, endY, maze, visited, out price);
            return price;
        }
        static void step(ushort posX, ushort posY, ushort endX, ushort endY, 
            short[,] maze, bool?[,] visited, out int price)
        {
            visited[posX, posY] = true;
            price = maze[posX, posY];

            if(posX == endX && posY == endY)
                return;

            int priceMin = short.MaxValue;
            if (visited[posX + 1, posY] != true)
                step((ushort)(posX + 1), posY, endX, endY, maze, visited, out priceMin);

            int priceP = short.MaxValue;
            if (visited[posX - 1, posY] != true)
                step((ushort)(posX - 1), posY, endX, endY, maze, visited, out priceP);
            if (priceP < priceMin)
                priceMin = priceP;

            if (visited[posX, posY - 1] != true)
                step(posX, (ushort)(posY - 1), endX, endY, maze, visited, out priceP);
            if (priceP < priceMin)
                priceMin = priceP;

            if (visited[posX, posY + 1] != true)
                step(posX, (ushort)(posY + 1), endX, endY, maze, visited, out priceP);
            if (priceP < priceMin)
                priceMin = priceP;

            if (priceMin == short.MaxValue || priceMin == 0)
                price = 0;
            else
                price += priceMin;
        }

        static void Main(string[] args)
        {
            string[] arr;
            {
                string input;
                using (var fs = new FileStream("INPUT.TXT", FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        input = sr.ReadToEnd();
                    }
                }
                arr = input.Split('\n');
                arr = arr.Select(str => str.Trim()).Where(s => s != string.Empty).ToArray();
            }
            var dims = arr[0].Split(' ').Select(short.Parse).ToArray();
            short[,] fieldArray = new short[dims[0], dims[1]];

            ushort startX = 0, startY = 0, endX = 0, endY = 0;
            dims = arr[1].Split(' ').Select(short.Parse).ToArray();
            for (ushort i0 = 0; i0 < fieldArray.GetLength(0); i0++)
                for (ushort i1 = 0; i1 < fieldArray.GetLength(1); i1++)
                {
                    switch (arr[i0+2][i1])
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

            int result = Solve(startX, startY, endX, endY, fieldArray);



            using (var fs = new FileStream("OUTPUT.TXT", FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {

                    if (result == 0)
                        sw.Write("Sleep");
                    sw.Write(result);
                }
            }
        }
    }
}