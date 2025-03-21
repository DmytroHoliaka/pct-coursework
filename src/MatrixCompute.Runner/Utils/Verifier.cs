using MathNet.Numerics.LinearAlgebra.Double;
using MatrixCompute.Core.Multipliers.Classical;
using MatrixCompute.Core.Multipliers.ParallelBulk;
using MatrixCompute.Core.Multipliers.ParallelStripe;
using MatrixCompute.Core.Multipliers.Stripe;
using Matrix = MatrixCompute.Core.Models.Matrix;

namespace MatrixCompute.Runner.Utils;

internal static class Verifier
{
    private static readonly ClassicalMultiplier ClassicalMultiplier = new();
    private static readonly StripeMultiplier StripeMultiplier = new();
    private static readonly ParallelStripeMultiplier ParallelStripeMultiplier = new();
    private static readonly ParallelBulkMultiplier ParallelBulkMultiplier = new();

    internal static void VerifyAll(int dimension)
    {
        Matrix matrixA = Matrix.GenerateRandomMatrix(dimension, dimension);
        Matrix matrixB = Matrix.GenerateRandomMatrix(dimension, dimension);
        Matrix verifiedResult = MultiplyWithMathNet(matrixA, matrixB);

        Matrix classicalResult = ClassicalMultiplier.Multiply(matrixA, matrixB);
        VerifyEquality(classicalResult, verifiedResult, "Classical multiplier");
        
        Matrix stripeMultiplier = StripeMultiplier.Multiply(matrixA, matrixB);
        VerifyEquality(stripeMultiplier, verifiedResult, "Stripe multiplier");
        
        Matrix parallelStripeMultiplier = ParallelStripeMultiplier.Multiply(matrixA, matrixB);
        VerifyEquality(parallelStripeMultiplier, verifiedResult, "Parallel stripe multiplier multiplier");
        
        Matrix parallelBulkMultiplier = ParallelBulkMultiplier.Multiply(matrixA, matrixB);
        VerifyEquality(parallelBulkMultiplier, verifiedResult, "Parallel bulk multiplier multiplier");
    }

    private static void VerifyEquality(Matrix calculatedResult, Matrix verifiedResult, string multiplier)
    {
        Console.WriteLine(AreMatricesEqual(calculatedResult, verifiedResult)
            ? $"+ | {multiplier} has successfully been verified"
            : $"- | {multiplier} hasn't been verified");
    }

    private static Matrix MultiplyWithMathNet(Matrix a, Matrix b)
    {
        DenseMatrix matrixA = DenseMatrix.OfArray(a.Data);
        DenseMatrix matrixB = DenseMatrix.OfArray(b.Data);
        DenseMatrix result = matrixA * matrixB;
        return new Matrix(result.ToArray());
    }

    private static bool AreMatricesEqual(Matrix a, Matrix b, double tolerance = 1e-9)
    {
        if (a.Rows != b.Rows || a.Cols != b.Cols)
        {
            return false;
        }

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Cols; j++)
            {
                if (Math.Abs(a[i, j] - b[i, j]) > tolerance)
                {
                    return false;
                }
            }
        }

        return true;
    }
}