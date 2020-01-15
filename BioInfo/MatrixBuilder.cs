using System;

namespace BioInfo
{
    public class MatrixBuilder
    {
        public static (double result, double[,] d) GlobalWithPenalty(string u, string w, double[,] matrix)
            => BuildMatrix(u, w, matrix);


        private static (double result, double[,] d) BuildMatrix(string u, string w, double[,] matrix)
        {
            int m, n;
            double[,] S, A, B, C;
            m = u.Length;
            n = w.Length;

            S = new double[n + 1, m + 1];
            A = new double[n + 1, m + 1];
            B = new double[n + 1, m + 1];
            C = new double[n + 1, m + 1];

            PopulateWithNegativeInf(n, m, S, A, B, C);

            S[0, 0] = 0;
            for (var j = 1; j <= m; j++)
            {
                S[0, j] = Penalty.Value(j);
                A[0, j] = Penalty.Value(j);
            }

            for (var i = 1; i <= n; i++)
            {
                S[i, 0] = Penalty.Value(i);
                B[i, 0] = Penalty.Value(i);
            }

            for (var i = 1; i <= n; i++)
            for (var j = 1; j <= m; j++)
            {
                for (var k = 0; k < j - 1; k++) 
                    A[i, j] = Math.Max(B[i, k], C[i, k] + Penalty.Value(j - k));

                for (var k = 0; k < i - 1; k++)
                    B[i, j] = Math.Max(A[k, j], C[k, j] + Penalty.Value(i - k));

                C[i,j] = S[i - 1, j - 1] + GetValue(matrix, u[j - 1], w[i - 1]);
                S[i, j] = Math.Max(A[i,j], Math.Max(B[i,j], C[i,j]));
            }

            return (S[n, m], S);
        }

        private static void PopulateWithNegativeInf(int n, int m, double[,] S, double[,] A, double[,] B, double[,] C)
        {
            for (var i = 0; i <= n; i++)
            for (var j = 0; j <= m; j++)
            {
                S[i, j] = double.NegativeInfinity;
                A[i, j] = double.NegativeInfinity;
                B[i, j] = double.NegativeInfinity;
                C[i, j] = double.NegativeInfinity;
            }
        }

        static double GetValue(double[,] matrix, char u, char w)
        {
            int i, j;
            i = u switch
            {
                '-' => 0,
                'A' => 1,
                'G' => 2,
                'C' => 3,
                'T' => 4,
            };
            j = w switch
            {
                '-' => 0,
                'A' => 1,
                'G' => 2,
                'C' => 3,
                'T' => 4,
            };
            return matrix[i, j];
        }
    }
}