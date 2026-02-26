namespace SudokuSolver;

public class SudokuCell(int initialNumber = 0)
{
    /// <summary>
    /// The actual number placed in the cell.
    /// 0 means "empty / not yet solved".
    /// </summary>
    public int ActualNumber { get; set; } = initialNumber;

    /// <summary>
    /// Potential numbers that this cell could take.
    /// </summary>
    public HashSet<int> PotentialNumbers { get; } = [];

    public bool IsSolved => ActualNumber != 0;

    public override string ToString()
    {
        return ActualNumber == 0 ? "." : ActualNumber.ToString();
    }
}