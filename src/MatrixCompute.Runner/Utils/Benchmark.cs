using System.Diagnostics;
using MatrixCompute.Core.Abstractions;
using Matrix = MatrixCompute.Core.Models.Matrix;

namespace MatrixCompute.Runner.Utils;

internal sealed class Benchmark
{
    private readonly Stopwatch _sw = new();

    internal void Run(IMultiplier multiplier, int dimension)
    {
        Matrix matrixA = Matrix.GenerateRandomMatrix(dimension, dimension);
        Matrix matrixB = Matrix.GenerateRandomMatrix(dimension, dimension);

        _sw.Restart();
        multiplier.Multiply(matrixA, matrixB);
        _sw.Stop();
        double duration = _sw.Elapsed.TotalMilliseconds;

        Console.WriteLine(duration);
    }
}