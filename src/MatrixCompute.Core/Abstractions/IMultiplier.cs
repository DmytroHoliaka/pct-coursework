using MatrixCompute.Core.Models;

namespace MatrixCompute.Core.Abstractions;

public interface IMultiplier
{
    Matrix Multiply(Matrix matrixA, Matrix matrixB);
}