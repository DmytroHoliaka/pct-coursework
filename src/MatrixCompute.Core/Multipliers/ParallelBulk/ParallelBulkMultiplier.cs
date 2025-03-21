using MatrixCompute.Core.Abstractions;
using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Multipliers.ParallelBulk;

public class ParallelBulkMultiplier : IMultiplier
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
        Task[] tasks = new Task[n];

        for (int i = 0; i < n; i++)
        {
            double[] rowA = a.GetRow(i);

            BulkWorker worker = new(i, rowA, b, m, (index, computedRow) =>
            {
                for (int j = 0; j < m; j++)
                {
                    resultData[index, j] = computedRow[j];
                }
            });
            
            tasks[i] = Task.Factory.StartNew(worker.Execute, TaskCreationOptions.LongRunning);
        }

        Task.WaitAll(tasks);
        return new Matrix(resultData);
    }
}