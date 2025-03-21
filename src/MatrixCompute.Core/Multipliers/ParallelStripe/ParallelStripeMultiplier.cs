using System.Collections.Concurrent;
using MatrixCompute.Core.Abstractions;
using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Multipliers.ParallelStripe;

public class ParallelStripeMultiplier : IMultiplier
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

        BlockingCollection<Column>[] mailboxes = new BlockingCollection<Column>[n];
        for (int i = 0; i < n; i++)
        {
            mailboxes[i] = new BlockingCollection<Column>(1);
        }

        Task[] tasks = new Task[n];

        for (int i = 0; i < n; i++)
        {
            double[] rowA = a.GetRow(i);
            double[] colData = b.GetColumn(i);
            Column initialColumn = new(i, colData);

            StripeWorker stripeWorker = new(i, rowA, m, mailboxes, initialColumn, (index, computedRow) =>
            {
                for (int j = 0; j < m; j++)
                {
                    resultData[index, j] = computedRow[j];
                }
            });

            tasks[i] = Task.Factory.StartNew(stripeWorker.Execute, TaskCreationOptions.LongRunning);
        }

        Task.WaitAll(tasks);
        return new Matrix(resultData);
    }
}