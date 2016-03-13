using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class KeypadModule
    {
        public static String PSI = "psi";
        public static String Q = "q";
        public static String A = "a";
        public static String LAMBDA = "lambda";
        public static String LIGHTNING = "lightning";
        public static String CAT = "cat";
        public static String H = "h";
        public static String MOON = "moon";
        public static String EURO = "euro";
        public static String SNAKE = "snake";
        public static String WHITESTAR = "whitestar";
        public static String QUESTIONMARK = "questionmark";
        public static String TRADEMARK = "trademark";
        public static String NOSE = "nose";
        public static String CROSS = "cross";
        public static String HALFTHREE = "halfthree";
        public static String SIX = "six";
        public static String PHARAGRAPH = "pharagraph";
        public static String B = "b";
        public static String SMILEY = "smiley";
        public static String C = "c";
        public static String THREE = "three";
        public static String BLACKSTAR = "blackstar";
        public static String STITCHES = "stitches";
        public static String AE = "norwegian";
        public static String N = "n";
        public static String OMEGA = "omega";

        Speaker talk;
        Listener rec;
        String[] input;
        public KeypadModule(Speaker talk, Listener rec, String[] input)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            solve();
        }

        private void solve()
        {
            String[,] matrix = getMatrix();
            int column = getColumn(matrix);
            if(column == -1)
            {
                talk.speakAsync("Impossible combination of symbols. Please repeat!");
                return;
            }
            sayAnswer(matrix, column);
        }

        private void sayAnswer(String[,] matrix, int column)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Press ");
            for(int y = 0; y < matrix.GetLength(1); y++)
            {
                foreach(String symbol in input)
                {
                    if (matrix[column, y].Equals(symbol))
                        sb.Append(symbol + ", ");
                }
            }
            sb.Remove(sb.Length - 2, 2);
            talk.speakAsync(sb.ToString());
        }

        private int getColumn(String[,] matrix)
        {
            for(int x = 0; x < matrix.GetLength(0); x++)
            {
                int symbolsMatch = 0;
                for(int y = 0; y < matrix.GetLength(1); y++)
                {
                    foreach(String symbol in input)
                    {
                        if (matrix[x, y].Equals(symbol))
                            symbolsMatch++;
                    }
                }
                if (symbolsMatch == 4)
                    return x;
            }
            return -1;
        }

        private string[,] getMatrix()
        {
            String[,] matrix = new string[6, 7];

            matrix[0, 0] = Q;
            matrix[0, 1] = A;
            matrix[0, 2] = LAMBDA;
            matrix[0, 3] = LIGHTNING;
            matrix[0, 4] = CAT;
            matrix[0, 5] = H;
            matrix[0, 6] = MOON;

            matrix[1, 0] = EURO;
            matrix[1, 1] = Q;
            matrix[1, 2] = MOON;
            matrix[1, 3] = SNAKE;
            matrix[1, 4] = WHITESTAR;
            matrix[1, 5] = H;
            matrix[1, 6] = QUESTIONMARK;

            matrix[2, 0] = TRADEMARK;
            matrix[2, 1] = NOSE;
            matrix[2, 2] = SNAKE;
            matrix[2, 3] = CROSS;
            matrix[2, 4] = HALFTHREE;
            matrix[2, 5] = LAMBDA;
            matrix[2, 6] = WHITESTAR;

            matrix[3, 0] = SIX;
            matrix[3, 1] = PHARAGRAPH;
            matrix[3, 2] = B;
            matrix[3, 3] = CAT;
            matrix[3, 4] = CROSS;
            matrix[3, 5] = QUESTIONMARK;
            matrix[3, 6] = SMILEY;

            matrix[4, 0] = PSI;
            matrix[4, 1] = SMILEY;
            matrix[4, 2] = B;
            matrix[4, 3] = C;
            matrix[4, 4] = PHARAGRAPH;
            matrix[4, 5] = THREE;
            matrix[4, 6] = BLACKSTAR;

            matrix[5, 0] = SIX;
            matrix[5, 1] = EURO;
            matrix[5, 2] = STITCHES;
            matrix[5, 3] = AE;
            matrix[5, 4] = PSI;
            matrix[5, 5] = N;
            matrix[5, 6] = OMEGA;

            return matrix;
        }
    }
}
