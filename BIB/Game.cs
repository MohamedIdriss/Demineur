using System;

namespace BIB
{
    public enum Level {Beginner=10,Intermediate=20,Advanced=40};
    public class Game
    {
        
        public int Bombs { get; set; } 
        public int[,] Grid { get; } 
        #region Constructeur
        public Game(Level level)
        {
            Bombs = (int)level;
            switch (level)
            {
                case Level.Beginner:
                    Grid = new int[9, 9];
                    break;
                case Level.Intermediate:
                    Grid = new int[12, 12];
                    break;
                case Level.Advanced:
                    Grid = new int[12, 16];
                    break;
            }
            SetBombs();
            fillGrid();
        }
        public Game(Level level, int bombs) : this(level)
        {
            this.Bombs = bombs;
        }
        #endregion
        private void SetBombs()
        {
            Random r = new Random();
            int c = Bombs;
            while (c > 0)
            {
                int i = r.Next(0, Grid.GetLength(0));
                int j = r.Next(0, Grid.GetLength(1));
                if (Grid[i, j] != 9)
                {
                    Grid[i, j] = 9;
                    c--;
                }
            }
        }
        private void fillGrid()
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j] != 9)
                    {
                        Grid[i, j] = getNum(i, j);
                    }

                }
            }
        }
        private int getNum(int i,int j)
        {
            int bmb = 0;
            for (int x = i-1; x <= i+1; x++)
            {
                if(x > -1 && x < Grid.GetLength(0))
                    for (int y = j-1; y <= j+1 ; y++)
                    {
                        if ((y > -1 && y < Grid.GetLength(1)) && (x != i || y != j) && (Grid[x, y] == 9))
                        {
                                bmb++;
                        }
                    }
            }
            

            return (bmb);
        }
    }
}
