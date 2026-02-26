using SudokuSolver;

var initialGrid = new int[,]
{
    { 0, 0, 0, 0, 9, 4, 0, 0, 0 },
    { 0, 3, 0, 0, 0, 0, 2, 0, 0 },
    { 0, 7, 0, 0, 0, 0, 0, 0, 0 },

    { 0, 0, 0, 1, 0, 0, 0, 3, 0 },
    { 6, 0, 9, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 8, 0 },

    { 2, 0, 0, 0, 0, 0, 9, 0, 4 },
    { 0, 0, 0, 0, 5, 0, 6, 0, 0 },
    { 0, 0, 0, 7, 0, 8, 0, 0, 0 }
};

SudokuBoard board = new(initialGrid);

Console.WriteLine("Initial board:");
board.Print();

Console.WriteLine("Solving...");
board.SolveBacktracking();
Console.WriteLine("Solved:");
board.Print();

Console.WriteLine("Press any key to exit...");