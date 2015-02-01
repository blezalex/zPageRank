using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    public static class Matrix
    {
        public static double[] Multiply(double[,] m, double[] v)
        {
            if (v.Length != m.GetLength(1))
                throw new ArgumentException("m length != v length");

            var result = new double[m.GetLength(0)];
            for (var r = 0; r < m.GetLength(0); r++)
            {
                for (int c = 0; c < m.GetLength(1); c++)
                {
                    result[c] += v[r] * m[r, c];
                }

            }

            return result;
        }

        public static double[,] Multiply(double beta, double[,] m)
        {
            var result = (double[,])m.Clone();

            for (int r = 0; r < result.GetLength(0); r++)
            {
                for (int c = 0; c < result.GetLength(1); c++)
                {
                    result[r, c] *= beta;
                }
            }

            return result;
        }

        public static double[,] Sum(double[,] a, double[,] b)
        {
            var result = (double[,])a.Clone();

            for (int r = 0; r < result.GetLength(0); r++)
            {
                for (int c = 0; c < result.GetLength(1); c++)
                {
                    result[r, c] = a[r, c] + b[r, c];
                }
            }

            return result;
        }

    }
}
