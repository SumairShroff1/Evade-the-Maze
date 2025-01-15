using UnityEngine;
using System.Collections;

//<summary>
//Maze generation by dividing area in two, adding spaces in walls, and repeating recursively.
//Non-recursive realization of the algorithm.
//</summary>
public class DivisionMazeGenerator : BasicMazeGenerator {

    // Constructor to initialize the base class with maze dimensions
    public DivisionMazeGenerator(int row, int column):base(row, column){
    }

    //<summary>
    //Class representing a rectangular area of the maze to be divided
    //</summary>
    private struct IntRect{
        public int left;
        public int right;
        public int top;
        public int bottom;

        // Override ToString for better debugging visualization of the rectangle
        public override string ToString ()
        {
            return string.Format ("[IntRect {0}-{1} {2}-{3}]", left, right, bottom, top);
        }
    }

    // Queue to hold areas of the maze that need to be divided
    private System.Collections.Generic.Queue<IntRect> rectsToDivide = new System.Collections.Generic.Queue<IntRect>();

    // Method to generate the maze by dividing it recursively
    public override void GenerateMaze(){
        // Create outer walls of the maze
        for (int row = 0; row < RowCount; row++) {
            GetMazeCell(row, 0).WallLeft = true; // Left wall
            GetMazeCell(row, ColumnCount - 1).WallRight = true; // Right wall
        }
        for (int column = 0; column < ColumnCount; column++) {
            GetMazeCell(0, column).WallBack = true; // Top wall
            GetMazeCell(RowCount - 1, column).WallFront = true; // Bottom wall
        }

        // Enqueue the entire maze area for division
        rectsToDivide.Enqueue(new IntRect() { left = 0, right = ColumnCount, bottom = 0, top = RowCount });

        // Process each rectangular area in the queue
        while (rectsToDivide.Count > 0) {
            IntRect currentRect = rectsToDivide.Dequeue();
            int width = currentRect.right - currentRect.left;
            int height = currentRect.top - currentRect.bottom;

            // Determine how to divide the area based on its dimensions
            if (width > 1 && height > 1) {
                if (width > height) {
                    divideVertical(currentRect);
                } else if (height > width) {
                    divideHorizontal(currentRect);
                } else { // Width equals height, choose randomly
                    if (Random.Range(0, 100) > 42) {
                        divideVertical(currentRect);
                    } else {
                        divideHorizontal(currentRect);
                    }
                }
            } else if (width > 1 && height <= 1) {
                divideVertical(currentRect);
            } else if (width <= 1 && height > 1) {
                divideHorizontal(currentRect);
            }
        }
    }

    //<summary>
    //Divides the selected rectangular area vertically
    //</summary>
    private void divideVertical(IntRect rect){
        int divCol = Random.Range(rect.left, rect.right - 1); // Select a column to divide
        for (int row = rect.bottom; row < rect.top; row++) {
            GetMazeCell(row, divCol).WallRight = true; // Add right wall
            GetMazeCell(row, divCol + 1).WallLeft = true; // Add left wall to adjacent cell
        }
        int space = Random.Range(rect.bottom, rect.top); // Create a passage in the wall
        GetMazeCell(space, divCol).WallRight = false;
        if (divCol + 1 < rect.right) {
            GetMazeCell(space, divCol + 1).WallLeft = false;
        }
        // Enqueue the two new areas created by the division
        rectsToDivide.Enqueue(new IntRect() { left = rect.left, right = divCol + 1, bottom = rect.bottom, top = rect.top });
        rectsToDivide.Enqueue(new IntRect() { left = divCol + 1, right = rect.right, bottom = rect.bottom, top = rect.top });
    }

    //<summary>
    //Divides the selected rectangular area horizontally
    //</summary>
    private void divideHorizontal(IntRect rect){
        int divRow = Random.Range(rect.bottom, rect.top - 1); // Select a row to divide
        for (int col = rect.left; col < rect.right; col++) {
            GetMazeCell(divRow, col).WallFront = true; // Add front wall
            GetMazeCell(divRow + 1, col).WallBack = true; // Add back wall to adjacent cell
        }
        int space = Random.Range(rect.left, rect.right); // Create a passage in the wall
        GetMazeCell(divRow, space).WallFront = false;
        if (divRow + 1 < rect.top) {
            GetMazeCell(divRow + 1, space).WallBack = false;
        }
        // Enqueue the two new areas created by the division
        rectsToDivide.Enqueue(new IntRect() { left = rect.left, right = rect.right, bottom = rect.bottom, top = divRow + 1 });
        rectsToDivide.Enqueue(new IntRect() { left = rect.left, right = rect.right, bottom = divRow + 1, top = rect.top });
    }
}
