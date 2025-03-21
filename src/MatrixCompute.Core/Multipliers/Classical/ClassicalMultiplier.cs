using MatrixCompute.Core.Abstractions;
using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Multipliers.Classical;

public class ClassicalMultiplier : IMultiplier
{
    public Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Cols != b.Rows)
        {
            throw new ArgumentException("Invalid matrix dimensions. A.Cols must equal B.Rows.");
        }

        int n = a.Rows;
        int m = b.Cols;
        int s = a.Cols;
        double[,] resultData = new double[n, m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                double sum = 0;
                for (int k = 0; k < s; k++)
                {
                    sum += a[i, k] * b[k, j];
                }

                resultData[i, j] = sum;
            }
        }

        return new Matrix(resultData);
    }
}