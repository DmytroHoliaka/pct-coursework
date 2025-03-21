namespace MatrixCompute.Core.Models;

public class Matrix(double[,] data)
{
    public double[,] Data { get; } = data;
    public int Rows { get; } = data.GetLength(0);
    public int Cols { get; } = data.GetLength(1);

    private static readonly Random Rand = new();

    public static Matrix GenerateRandomMatrix(int rows, int cols)
    {
        double[,] data = new double[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                data[i, j] = Rand.NextDouble() * 100;
            }
        }

        return new Matrix(data);
    }
    
    public void Print()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write($"{Data[i, j]}\t");
            }

            Console.WriteLine();
        }
    }

    public double this[int row, int col]
    {
        get => Data[row, col];
        set => Data[row, col] = value;
    }

    public double[] GetRow(int rowIndex)
    {
        double[] row = new double[Cols];
        for (int j = 0; j < Cols; j++)
        {
            row[j] = Data[rowIndex, j];
        }

        return row;
    }

    public double[] GetColumn(int colIndex)
    {
        double[] column = new double[Rows];
        for (int i = 0; i < Rows; i++)
        {
            column[i] = Data[i, colIndex];
        }

        return column;
    }
}