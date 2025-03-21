using MatrixCompute.Core.Multipliers.Classical;
using MatrixCompute.Core.Multipliers.ParallelBulk;
using MatrixCompute.Core.Multipliers.ParallelStripe;
using MatrixCompute.Core.Multipliers.Stripe;
using MatrixCompute.Runner.Utils;

namespace MatrixCompute.Runner;

internal abstract class Program
{
    internal static void Main()
    {
        string multiplierChoice = EnvironmentConfig.GetMultiplier();
        Benchmark benchmark = new();

        switch (multiplierChoice)
        {
            case EnvironmentConfig.Options.ClassicalMultiplier:
                ClassicalMultiplier classicalMultiplier = new();
                int classicalMatrixDimension = EnvironmentConfig.GetDimension();
                benchmark.Run(classicalMultiplier, classicalMatrixDimension);
                break;

            case EnvironmentConfig.Options.StripeMultiplier:
                StripeMultiplier stripeMultiplier = new();
                int stripeMatrixDimension = EnvironmentConfig.GetDimension();
                benchmark.Run(stripeMultiplier, stripeMatrixDimension);
                break;

            case EnvironmentConfig.Options.ParallelStripeMultiplier:
                ParallelStripeMultiplier parallelStripeMultiplier = new();
                int parallelStripeMatrixDimension = EnvironmentConfig.GetDimension();
                benchmark.Run(parallelStripeMultiplier, parallelStripeMatrixDimension);
                break;

            case EnvironmentConfig.Options.ParallelBulkMultiplier:
                ParallelBulkMultiplier parallelBulkMultiplier = new();
                int parallelBulkMatrixDimension = EnvironmentConfig.GetDimension();
                benchmark.Run(parallelBulkMultiplier, parallelBulkMatrixDimension);
                break;

            case EnvironmentConfig.Options.MultipliersVerification:
                int verificationDimension = EnvironmentConfig.GetVerificationDimension();
                Verifier.VerifyAll(verificationDimension);
                break;

            default:
                throw new ApplicationException($"Unsupported multiplier.");
        }
    }
}