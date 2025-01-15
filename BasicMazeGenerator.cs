using UnityEngine;
using System.Collections;

//<summary>
//Basic class for maze generation logic
//</summary>
public abstract class BasicMazeGenerator {

    // Properties to retrieve the number of rows and columns in the maze
    public int RowCount{ get{ return mMazeRows; } }
    public int ColumnCount { get { return mMazeColumns; } }

    // Private fields to store the maze dimensions and cells
    private int mMazeRows;
    private int mMazeColumns;
    private MazeCell[,] mMaze;

    // Constructor to initialize the maze with specified rows and columns
    public BasicMazeGenerator(int rows, int columns){
        // Ensure rows and columns are non-negative and set a minimum value of 1
        mMazeRows = Mathf.Abs(rows);
        mMazeColumns = Mathf.Abs(columns);
        if (mMazeRows == 0) {
            mMazeRows = 1;
        }
        if (mMazeColumns == 0) {
            mMazeColumns = 1;
        }

        // Initialize the 2D maze array with empty MazeCell objects
        mMaze = new MazeCell[rows, columns];
        for (int row = 0; row < rows; row++) {
            for(int column = 0; column < columns; column++){
                mMaze[row, column] = new MazeCell();
            }
        }
    }

    // Abstract method for generating the maze; implementation to be defined in subclasses
    public abstract void GenerateMaze();

    // Method to retrieve a specific MazeCell based on row and column indices
    public MazeCell GetMazeCell(int row, int column){
        // Ensure the indices are within bounds; otherwise, throw an exception
        if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns) {
            return mMaze[row, column];
        } else {
            Debug.Log(row + " " + column); // Log the invalid indices for debugging
            throw new System.ArgumentOutOfRangeException();
        }
    }

    // Method to set a specific MazeCell at a given row and column
    protected void SetMazeCell(int row, int column, MazeCell cell){
        // Ensure the indices are within bounds; otherwise, throw an exception
        if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns) {
            mMaze[row, column] = cell;
        } else {
            throw new System.ArgumentOutOfRangeException();
        }
    }
}

