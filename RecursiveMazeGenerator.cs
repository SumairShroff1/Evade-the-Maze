using UnityEngine;
using System.Collections;

//<summary>
//Pure recursive maze generation.
//Use carefully for large mazes to avoid stack overflow.
//</summary>
public class RecursiveMazeGenerator : BasicMazeGenerator {

    // Constructor to initialize the base class with maze dimensions
    public RecursiveMazeGenerator(int rows, int columns):base(rows, columns){

    }

    // Starts the maze generation process
    public override void GenerateMaze ()
    {
        // Begin visiting cells starting from the top-left corner of the maze
        VisitCell(0, 0, Direction.Start);
    }

    // Recursively visits cells to generate the maze
    private void VisitCell(int row, int column, Direction moveMade){
        // Array to store available moves from the current cell
        Direction[] movesAvailable = new Direction[4];
        int movesAvailableCount = 0;

        do {
            movesAvailableCount = 0; // Reset the count of available moves

            // Check if moving right is possible
            if (column + 1 < ColumnCount && !GetMazeCell(row, column + 1).IsVisited) {
                movesAvailable[movesAvailableCount] = Direction.Right;
                movesAvailableCount++;
            } else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Left) {
                GetMazeCell(row, column).WallRight = true; // Add a wall to the right
            }

            // Check if moving forward is possible
            if (row + 1 < RowCount && !GetMazeCell(row + 1, column).IsVisited) {
                movesAvailable[movesAvailableCount] = Direction.Front;
                movesAvailableCount++;
            } else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Back) {
                GetMazeCell(row, column).WallFront = true; // Add a wall to the front
            }

            // Check if moving left is possible
            if (column > 0 && column - 1 >= 0 && !GetMazeCell(row, column - 1).IsVisited) {
                movesAvailable[movesAvailableCount] = Direction.Left;
                movesAvailableCount++;
            } else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Right) {
                GetMazeCell(row, column).WallLeft = true; // Add a wall to the left
            }

            // Check if moving backward is possible
            if (row > 0 && row - 1 >= 0 && !GetMazeCell(row - 1, column).IsVisited) {
                movesAvailable[movesAvailableCount] = Direction.Back;
                movesAvailableCount++;
            } else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Front) {
                GetMazeCell(row, column).WallBack = true; // Add a wall to the back
            }

            // If no moves are available and the cell is unvisited, mark it as a goal
            if (movesAvailableCount == 0 && !GetMazeCell(row, column).IsVisited) {
                GetMazeCell(row, column).IsGoal = true;
            }

            // Mark the current cell as visited
            GetMazeCell(row, column).IsVisited = true;

            // If there are available moves, choose one at random and visit the next cell
            if (movesAvailableCount > 0) {
                switch (movesAvailable[Random.Range(0, movesAvailableCount)]) {
                    case Direction.Start:
                        break;
                    case Direction.Right:
                        VisitCell(row, column + 1, Direction.Right);
                        break;
                    case Direction.Front:
                        VisitCell(row + 1, column, Direction.Front);
                        break;
                    case Direction.Left:
                        VisitCell(row, column - 1, Direction.Left);
                        break;
                    case Direction.Back:
                        VisitCell(row - 1, column, Direction.Back);
                        break;
                }
            }

        } while (movesAvailableCount > 0); // Repeat until no moves are available
    }
}
