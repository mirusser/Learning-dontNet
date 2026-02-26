namespace SudokuSolver;

public class BoardBase
{
    protected const int Size = 9; // classic sudoku size
    protected SudokuCell[,] Cells { get; } = new SudokuCell[Size, Size];

    protected BoardBase(int[,] initialGrid)
    {
        if (initialGrid.GetLength(0) != Size || initialGrid.GetLength(1) != Size)
            throw new ArgumentException("Grid must be 9x9");

        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                Cells[row, col] = new SudokuCell(initialGrid[row, col]);
            }
        }
    }

    /// <summary>
    /// Prints the board to the console in a friendly way.
    /// </summary>
    public void Print()
    {
        for (var row = 0; row < Size; row++)
        {
            if (row % 3 == 0 && row != 0)
            {
                Console.WriteLine("------+-------+------");
            }

            for (var col = 0; col < Size; col++)
            {
                if (col % 3 == 0 && col != 0)
                {
                    Console.Write("| ");
                }

                Console.Write(Cells[row, col] + " ");
            }

            Console.WriteLine();
        }
    }
}