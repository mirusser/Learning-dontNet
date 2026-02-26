namespace SudokuSolver;

public class SudokuBoard(int[,] initialGrid) : BoardBase(initialGrid)
{
    /// <summary>
    /// Try to solve the Sudoku using backtracking.
    /// </summary>
    public bool SolveBacktracking()
    {
        int row = -1;
        int col = -1;

        // Find an unassigned cell
        if (!FindUnassignedLocation(ref row, ref col))
        {
            // No unassigned cells -> solved
            return true;
        }

        var cell = Cells[row, col];
        cell.PotentialNumbers.Clear();

        // Build up potential candidates for this cell
        for (int candidate = 1; candidate <= 9; candidate++)
        {
            if (IsSafe(row, col, candidate))
            {
                cell.PotentialNumbers.Add(candidate);
            }
        }

        // Try each candidate
        foreach (int candidate in cell.PotentialNumbers.ToList())
        {
            cell.ActualNumber = candidate;

            if (SolveBacktracking())
            {
                return true; // solved!
            }

            // Backtrack
            cell.ActualNumber = 0;
        }

        // No candidate worked
        cell.PotentialNumbers.Clear();
        return false;
    }

    /// <summary>
    /// Finds the next cell with ActualNumber == 0.
    /// Returns true if found and sets row/col.
    /// </summary>
    private bool FindUnassignedLocation(ref int row, ref int col)
    {
        for (var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                if (Cells[r, c].ActualNumber == 0)
                {
                    row = r;
                    col = c;
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsSafe(int row, int col, int number)
    {
        return !UsedInRow(row, number) &&
               !UsedInColumn(col, number) &&
               !UsedInBox(row - row % 3, col - col % 3, number);
    }

    private bool UsedInRow(int row, int number)
    {
        for (var col = 0; col < Size; col++)
        {
            if (Cells[row, col].ActualNumber == number)
                return true;
        }

        return false;
    }

    private bool UsedInColumn(int col, int number)
    {
        for (var row = 0; row < Size; row++)
        {
            if (Cells[row, col].ActualNumber == number)
                return true;
        }

        return false;
    }

    private bool UsedInBox(int boxStartRow, int boxStartCol, int number)
    {
        for (var rowInBox = 0; rowInBox < 3; rowInBox++)
        {
            for (var c = 0; c < 3; c++)
            {
                if (Cells[boxStartRow + rowInBox, boxStartCol + c].ActualNumber == number)
                    return true;
            }
        }

        return false;
    }

    public void ComputePotentialNumbers()
    {
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                var cell = Cells[row, col];
                cell.PotentialNumbers.Clear();

                if (cell.IsSolved)
                    continue;

                for (var candidate = 1; candidate <= 9; candidate++)
                {
                    if (IsSafe(row, col, candidate))
                    {
                        cell.PotentialNumbers.Add(candidate);
                    }
                }
            }
        }
    }
}