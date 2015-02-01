using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    class Rank
    {
        public static double[] Calculate(double[,] links, double beta = 0.85, double epsilon = 0.001)
        {
            var rank = new double[] { 1, 1, 1 };

            // add random jumps
            var M = Matrix.Multiply(beta, links);

            var jumpMatrix = new double[M.GetLength(0), M.GetLength(1)];

            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    jumpMatrix[i, j] = 1.0 / M.GetLength(0);
                }
            }

            var rndJumps = Matrix.Multiply(1 - beta, jumpMatrix);
            var A = Matrix.Sum(M, rndJumps);

            // calculate rank
            var newR = rank;

            do
            {
                rank = newR;

                newR = Matrix.Multiply(A, rank);
            }
            while (Diff(newR, rank) > epsilon);
            rank = newR;

            return rank;
        }

        private static double Diff(double[] r1, double[] r2)
        {
            double diff = 0;
            for (var i = 0; i < r1.Length; i++)
            {
                diff += Math.Abs(r1[i] - r2[i]);
            }

            return diff;
        }
    }
}
