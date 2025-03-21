namespace MatrixCompute.Core.Models;

internal class Column
{
    internal int Index { get; }
    internal double[] Data { get; }

    internal Column(int index, double[] data)
    {
        Index = index;
        Data = data;
    }
}