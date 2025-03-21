using MatrixCompute.Core.Abstractions;
using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Multipliers.Stripe;

public class StripeMultiplier : IMultiplier
{
    public Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Cols != b.Rows)
        {
            throw new ArgumentException("Invalid matrix dimensions. A.Cols must equal B.Rows.");
        }

        int n = a.Rows; 
        int s = a.Cols; 
        int m = b.Cols; 

        if (m != n)
        {
            throw new NotSupportedException("Stripe algorithm requires the number of columns in B " +
                                            "to equal the number of rows in A.");
        }

        double[,] resultData = new double[n, m];

        for (int i = 0; i < n; i++)
        {
            double[] rowA = a.GetRow(i);
            int currentColIndex = i;
            
            for (int iter = 0; iter < m; iter++)
            {
                double dot = 0;
                for (int k = 0; k < s; k++)
                {
                    dot += rowA[k] * b[k, currentColIndex];
                }

                resultData[i, currentColIndex] = dot;
                currentColIndex = (currentColIndex - 1 + m) % m;
            }
        }

        return new Matrix(resultData);
    }
}