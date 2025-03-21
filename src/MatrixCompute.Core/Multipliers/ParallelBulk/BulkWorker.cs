using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Multipliers.ParallelBulk;

internal class BulkWorker
{
    private readonly int _workerIndex;
    private readonly double[] _row;           
    private readonly Matrix _matrixB;         
    private readonly int _iterations;         
    private readonly Action<int, double[]> _onCompleted;

    internal BulkWorker(int workerIndex, double[] row, Matrix matrixB, int iterations, Action<int, double[]> onCompleted)
    {
        _workerIndex = workerIndex;
        _row = row;
        _matrixB = matrixB;
        _iterations = iterations;
        _onCompleted = onCompleted;
    }

    internal void Execute()
    {
        double[] resultRow = new double[_iterations];
        int currentColIndex = _workerIndex;

        for (int iter = 0; iter < _iterations; iter++)
        {
            double dot = _row
                .Select((rowValue, idx) => rowValue * _matrixB[idx, currentColIndex])
                .Sum();
            
            resultRow[currentColIndex] = dot;

            currentColIndex = (currentColIndex - 1 + _iterations) % _iterations;
        }

        _onCompleted(_workerIndex, resultRow);
    }
}