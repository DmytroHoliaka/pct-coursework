using System.Collections.Concurrent;
using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Multipliers.ParallelStripe;

internal class StripeWorker
{
    private readonly int _workerIndex;
    private readonly double[] _row;
    private readonly int _iterations;
    private readonly BlockingCollection<Column>[] _mailboxes;
    private readonly Column _initialColumn;
    private readonly Action<int, double[]> _onCompleted;

    internal StripeWorker(
        int workerIndex, 
        double[] row, 
        int iterations,
        BlockingCollection<Column>[] mailboxes, 
        Column initialColumn,
        Action<int, double[]> onCompleted)
    {
        _workerIndex = workerIndex;
        _row = row;
        _iterations = iterations;
        _mailboxes = mailboxes;
        _initialColumn = initialColumn;
        _onCompleted = onCompleted;
    }

    internal void Execute()
    {
        double[] resultRow = new double[_iterations];
        Column currentColumn = _initialColumn;

        for (int iteration = 0; iteration < _iterations; iteration++)
        {
            double dot = _row
                .Select((rowValue, idx) => rowValue * currentColumn.Data[idx])
                .Sum();

            resultRow[currentColumn.Index] = dot;

            if (iteration >= _iterations - 1)
            {
                continue;
            }

            int nextWorkerIndex = (_workerIndex + 1) % _mailboxes.Length;
            _mailboxes[nextWorkerIndex].Add(currentColumn);
            currentColumn = _mailboxes[_workerIndex].Take();
        }

        _onCompleted(_workerIndex, resultRow);
    }
}