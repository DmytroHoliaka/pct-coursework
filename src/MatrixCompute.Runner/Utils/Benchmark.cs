using System.Diagnostics;
using MatrixCompute.Core.Abstractions;
using Matrix = MatrixCompute.Core.Models.Matrix;

namespace MatrixCompute.Runner.Utils;

internal sealed class Benchmark
{
    private readonly Stopwatch _sw = new();

    internal void Run(IMultiplier multiplier, int dimension)
    {
        Console.WriteLine($"-------------- {multiplier.GetType().Name} --------------");
        Console.WriteLine("{0,6} | {1,6} | {2,20} ", "n", "s", "Execution time (ms)");
        Console.WriteLine("--------------------------------------------");

        Matrix matrixA = Matrix.GenerateRandomMatrix(dimension, dimension);
        Matrix matrixB = Matrix.GenerateRandomMatrix(dimension, dimension);

        _sw.Restart();
        multiplier.Multiply(matrixA, matrixB);
        _sw.Stop();
        double duration = _sw.Elapsed.TotalMilliseconds;

        Console.WriteLine("{0,6} | {1,6} | {2,20:F4}", dimension, dimension, duration);

        Console.WriteLine("--------------------------------------------");
    }
}