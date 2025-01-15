using UnityEngine;
using System.Collections;

//<summary>
//Basic class for Tree generation logic.
//Subclasses must override GetCellInRange to implement selecting strategy.
//</summary>
public abstract class TreeMazeGenerator : BasicMazeGenerator {

	//<summary>
	//Class representation of target cell
	//</summary>
	private struct CellToVisit{
		public int Row;
		public int Column;
		public Direction MoveMade;

		public CellToVisit(int row, int column, Direction move){
			Row = row;
			Column = column;
			MoveMade = move;
		}

		public override string ToString ()
		{
			return string.Format ("[MazeCell {0} {1}]", Row, Column);
		}
	}

	// List of cells to be visited during maze generation.
	System.Collections.Generic.List<CellToVisit> mCellsToVisit = new System.Collections.Generic.List<CellToVisit> ();

	// Constructor initializes the maze with specified rows and columns.
	public TreeMazeGenerator(int row, int column):base(row,column){

	}

	// Main method to generate the maze using a tree-based algorithm.
	public override void GenerateMaze ()
	{
		Direction[] movesAvailable = new Direction[4]; // Stores possible directions to move.
		int movesAvailableCount = 0; // Counter for available moves.
		// Add a random starting cell to the list of cells to visit.
		mCellsToVisit.Add (new CellToVisit (Random.Range (0, RowCount), Random.Range (0, ColumnCount), Direction.Start));
		
		// Loop until all cells are processed.
		while (mCellsToVisit.Count > 0) {
			movesAvailableCount = 0;
			CellToVisit ctv = mCellsToVisit[GetCellInRange(mCellsToVisit.Count - 1)]; // Select a cell based on strategy.
			
			// Check and process each possible move (right, forward, left, backward).
			// Check move right.
			if(ctv.Column+1 < ColumnCount && !GetMazeCell(ctv.Row, ctv.Column+1).IsVisited && !IsCellInList(ctv.Row, ctv.Column+1)){
				movesAvailable[movesAvailableCount] = Direction.Right;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Left){
				// Mark walls for invalid moves.
				GetMazeCell(ctv.Row, ctv.Column).WallRight = true;
				if(ctv.Column+1 < ColumnCount){
					GetMazeCell(ctv.Row, ctv.Column+1).WallLeft = true;
				}
			}
			// Check move forward.
			if(ctv.Row+1 < RowCount && !GetMazeCell(ctv.Row+1, ctv.Column).IsVisited && !IsCellInList(ctv.Row+1, ctv.Column)){
				movesAvailable[movesAvailableCount] = Direction.Front;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Back){
				GetMazeCell(ctv.Row, ctv.Column).WallFront = true;
				if(ctv.Row+1 < RowCount){
					GetMazeCell(ctv.Row+1, ctv.Column).WallBack = true;
				}
			}
			// Check move left.
			if(ctv.Column > 0 && ctv.Column-1 >= 0 && !GetMazeCell(ctv.Row, ctv.Column-1).IsVisited && !IsCellInList(ctv.Row, ctv.Column-1)){
				movesAvailable[movesAvailableCount] = Direction.Left;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Right){
				GetMazeCell(ctv.Row, ctv.Column).WallLeft = true;
				if(ctv.Column > 0 && ctv.Column-1 >= 0){
					GetMazeCell(ctv.Row, ctv.Column-1).WallRight = true;
				}
			}
			// Check move backward.
			if(ctv.Row > 0 && ctv.Row-1 >= 0 && !GetMazeCell(ctv.Row-1, ctv.Column).IsVisited && !IsCellInList(ctv.Row-1, ctv.Column)){
				movesAvailable[movesAvailableCount] = Direction.Back;
				movesAvailableCount++;
			}else if(!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Front){
				GetMazeCell(ctv.Row, ctv.Column).WallBack = true;
				if(ctv.Row > 0 && ctv.Row-1 >= 0){
					GetMazeCell(ctv.Row-1, ctv.Column).WallFront = true;
				}
			}

			// Mark the cell as the goal if all moves are exhausted.
			if(!GetMazeCell(ctv.Row, ctv.Column).IsVisited && movesAvailableCount == 0){
				GetMazeCell(ctv.Row, ctv.Column).IsGoal = true;
			}

			GetMazeCell(ctv.Row, ctv.Column).IsVisited = true; // Mark the cell as visited.
			
			if(movesAvailableCount > 0){
				switch(movesAvailable[Random.Range(0, movesAvailableCount)]){
				case Direction.Start:
					break;
				case Direction.Right:
					mCellsToVisit.Add(new CellToVisit(ctv.Row, ctv.Column+1, Direction.Right));
					break;
				case Direction.Front:
					mCellsToVisit.Add(new CellToVisit(ctv.Row+1, ctv.Column, Direction.Front));
					break;
				case Direction.Left:
					mCellsToVisit.Add(new CellToVisit(ctv.Row, ctv.Column-1, Direction.Left));
					break;
				case Direction.Back:
					mCellsToVisit.Add(new CellToVisit(ctv.Row-1, ctv.Column, Direction.Back));
					break;
				}
			}else{
				mCellsToVisit.Remove(ctv); // Remove the cell from the list if no moves are possible.
			}
		}
	}

	// Checks if a cell is already in the list of cells to visit.
	private bool IsCellInList(int row, int column){
		return mCellsToVisit.FindIndex((other) => other.Row == row && other.Column == column) >= 0;
	}

	//<summary>
	//Abstract method for cell selection strategy.
	//</summary>
	protected abstract int GetCellInRange(int max);
}
