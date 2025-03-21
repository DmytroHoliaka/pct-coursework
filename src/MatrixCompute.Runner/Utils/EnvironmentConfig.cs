namespace MatrixCompute.Runner.Utils;

internal class EnvironmentConfig
{
    internal static class Keys
    {
        internal const string Multiplier = "MULTIPLIER";
        internal const string Dimension = "DIMENSION";
        internal const string VerificationDimension = "VERIFICATION_DIMENSION";
    }

    internal static class Options
    {
        internal const string ClassicalMultiplier = "CLASSICAL_MULTIPLIER";
        internal const string StripeMultiplier = "STRIPE_MULTIPLIER";
        internal const string ParallelStripeMultiplier = "PARALLEL_STRIPE_MULTIPLIER";
        internal const string ParallelBulkMultiplier = "PARALLEL_BULK_MULTIPLIER";
        internal const string MultipliersVerification = "MULTIPLIERS_VERIFICATION";
    }

    internal static class Defaults
    {
        internal const string Multiplier = Options.ParallelStripeMultiplier;
        internal const int VerificationDimension = 512;
        internal const int Dimension = 512;
    }

    internal static string GetMultiplier()
    {
        return Environment.GetEnvironmentVariable(Keys.Multiplier) ?? Defaults.Multiplier;
    }

    internal static int GetDimension()
    {
        string matrixDimension = Environment.GetEnvironmentVariable(Keys.Dimension)
                                 ?? Defaults.Dimension.ToString();

        if (!int.TryParse(matrixDimension, out int dimension) || dimension < 0)
        {
            throw new ApplicationException("Unsupported dimension value.");
        }

        return dimension;
    }

    internal static int GetVerificationDimension()
    {
        string verificationDimension = Environment.GetEnvironmentVariable(Keys.VerificationDimension)
                                       ?? Defaults.VerificationDimension.ToString();

        if (!int.TryParse(verificationDimension, out int dimension) || dimension < 0)
        {
            throw new ApplicationException("Unsupported dimension value.");
        }

        return dimension;
    }
}