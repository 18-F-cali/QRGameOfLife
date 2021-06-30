using System;
using System.Collections.Generic;
using System.Linq;

namespace QRGameOfLife
{
    class Program
    {
        static int gridSize;
        static string[,] grid;
        static string block = "\u25A0";
        static List<(int, int, bool)> newState = new List<(int, int, bool)>(); 
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Grid size");
            gridSize = int.Parse(Console.ReadLine());
            grid = new string[gridSize, gridSize];
            DisplayEmptyGrid();
            RandomCellsFill();
            DisplayGrid();

            //should stop when same pattern is repeated twice
            while (true)
            {
                CheckPattern();
                makePattern();
                DisplayGrid();
                if (exitState())
                {
                    Console.Clear();
                    Console.WriteLine("Exiting...");
                    System.Threading.Thread.Sleep(1000);
                    System.Environment.Exit(0);
                }
                newState.Clear();
            }
        }

        private static bool exitState()
        {
            if (newState.Count == gridSize * gridSize)
            {
                if (newState.All(x => x.Item3 == false)
                   || newState.TrueForAll(x => x.Item3 == true))
                {
                    return true;
                }
            }
            return false;
        }

        private static void DisplayEmptyGrid()
        {
            Console.Clear();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Console.Write($" {block} ");
                }
                Console.WriteLine();
            }
            System.Threading.Thread.Sleep(1000);
        }

        static Random rnd = new Random();
        private static void RandomCellsFill()
        {
            int fillCount = rnd.Next(0, (gridSize * gridSize ) /2);
            var uniqueCells = new HashSet<(int,int)>();
            
            for (int k = 0; k < fillCount; k++)
            {
                int i = rnd.Next(0, gridSize);
                int j = rnd.Next(0, gridSize);
                if (uniqueCells.Add((i, j)))
                {
                    grid[i, j] = setBlock();
                    newState.Add((i, j, true));
                }
            }
        }
        private static string setBlock()
        {
            return block;
        }

        private static string resetBlock()
        {
            return string.Empty;
        }
        private static bool isSet(int row, int column)
        {
            if ((row >= 0 && row < gridSize) && (column >= 0 && column < gridSize))
            {
                if(grid[row,column] == block)
                return true;
            }

            return false;
        }
        private static void CheckPattern()
        {
            for (int i = 0; i < gridSize ; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int totalAlive = CheckNeighbouringCells(i,j);

                    //if totalAlive == 2; no change
                    if (totalAlive >= 3)
                    {
                        newState.Add((i, j, true));
                    }
                    else if (totalAlive < 2)
                    {
                        newState.Add((i, j, false));
                    }
                }
            }
        }

        private static int CheckNeighbouringCells(int row, int column)
        {
            int totalAlive = 0;
            if (isSet(row - 1, column - 1)) totalAlive++;
            if (isSet(row - 1, column)) totalAlive++;
            if (isSet(row - 1, column + 1)) totalAlive++;
            if (isSet(row, column - 1)) totalAlive++;
            if (isSet(row, column + 1)) totalAlive++;
            if (isSet(row + 1, column - 1)) totalAlive++;
            if (isSet(row + 1, column)) totalAlive++;
            if (isSet(row + 1, column + 1)) totalAlive++;
            return totalAlive;
        }

        private static void makePattern()
        {
            foreach (var item in newState)
            {
                if(item.Item3 == true)
                {
                    grid[item.Item1,item.Item2] = setBlock();
                }
                else
                {
                    grid[item.Item1,item.Item2] = resetBlock();
                }
            }
            
        }
        private static void DisplayGrid()
        {
            Console.Clear();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Console.Write($" {grid[i, j]} ");
                }
                Console.WriteLine();
            }
            System.Threading.Thread.Sleep(1000);
        }
    }
}
