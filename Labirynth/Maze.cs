using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Maze
{
    public enum Directions
    {
        N = 1,
        S = 2,
        E = 4,
        W = 8
    }
        
    public class Grid
    {
        const int _rowDimension = 0;
        const int _columnDimension = 1;

        public int MinSize{get; private set;} = 3;
        public int MaxSize{get; private set;} = 10;
        public int[,] Cells { get; private set;}

        Dictionary<Directions, int> DX = new Dictionary<Directions, int>
        {
            { Directions.N, 0 },
            { Directions.S, 0 },
            { Directions.E, 1 },
            { Directions.W, -1 }
        };
 
        Dictionary<Directions, int> DY = new Dictionary<Directions, int>
        {
            { Directions.N, -1 },
            { Directions.S, 1 },
            { Directions.E, 0 },
            { Directions.W, 0 }
        };
 
        Dictionary<Directions, Directions> Opposite = new Dictionary<Directions, Directions>
        {
            { Directions.N, Directions.S },
            { Directions.S, Directions.N },
            { Directions.E, Directions.W },
            { Directions.W, Directions.E }
        };

        public Grid() : this(3, 3){}
        public Grid(int rows, int cols)
        {
            Cells = Initialise(rows, cols);
        }

        int[,] Initialise(int rows, int cols)
        {
            if(rows < MinSize) rows = MinSize;
            if(rows > MaxSize) rows = MaxSize;
            if(cols < MinSize) cols = MinSize;
            if(cols > MaxSize) cols = MaxSize;
            var cells = new int[rows, cols];
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    cells[i,j] = 0;
                }
            }
            return cells;
        }

        public void Generate()
        {
            var cells = this.Cells;
            CarvePassagers(0, 0, ref cells);
            this.Cells = cells;
        }

        void CarvePassagers(int cX, int cY, ref int[,] grid)
        {
             var directions = new List<Directions>
            {
                Directions.N,
                Directions.S,
                Directions.E,
                Directions.W
            }
            .OrderBy(x => Guid.NewGuid());
            foreach (var direction in directions)
            {
                var nextX = cX + DX[direction];
                var nextY = cY + DY[direction];
 
                if (IsOutOfBounds(nextX, nextY, grid))
                    continue;
 
                if (grid[nextY, nextX] != 0) // has been visited
                    continue;
 
                grid[cY, cX] |= (int)direction;
                grid[nextY, nextX] |= (int)Opposite[direction];
 
                CarvePassagers(nextX, nextY, ref grid);
            }
        }

        private bool IsOutOfBounds(int x, int y, int[,] grid)
        {
            if (x < 0 || x > grid.GetLength(_rowDimension) - 1)
                return true;
 
            if (y < 0 || y > grid.GetLength(_columnDimension) - 1)
                return true;
 
            return false;
        }

        public void Print()
        {
            var rows = Cells.GetLength(_rowDimension);
            var columns = Cells.GetLength(_columnDimension);

            // Top
            for (int i = 0; i < columns; i++)
                Console.Write(" _");
            Console.WriteLine();

            for (int y = 0; y < rows; y++)
            {
                Console.Write("|");
 
                for (int x = 0; x < columns; x++)
                {
                    var directions = (Directions)Cells[y, x];
 
                    var s = directions.HasFlag(Directions.S) ? " " : "_";
 
                    Console.Write(s);
 
                    s = directions.HasFlag(Directions.E) ? " " : "|";
 
                    Console.Write(s);                   
                }
 
                Console.WriteLine();
            }
        }

        public void PrintNums()
        {
            for(int i = 0; i < Cells.GetLength(_rowDimension); i++)
            {
                for(int j = 0; j < Cells.GetLength(_columnDimension); j++)
                {
                    Console.Write(Cells[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}