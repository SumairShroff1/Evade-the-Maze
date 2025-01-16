using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// Game object responsible for creating and instantiating a maze in the scene.
/// </summary>
public class MazeSpawner : MonoBehaviour {

    // Enum defining the maze generation algorithms that can be used.
    public enum MazeGenerationAlgorithm {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    // Variables for customizing the maze generation process.
    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive; // Algorithm to use.
    public bool FullRandom = false; // Whether to use full randomization.
    public int RandomSeed = 12345; // Random seed for consistent maze generation.
    public NavMeshSurface surface; // NavMesh surface for AI pathfinding.

    // Prefabs for various maze components.
    public GameObject Floor = null; 
    public GameObject Wall = null; 
    public GameObject Pillar = null; 
    public GameObject EntrancePrefab = null; 
    public GameObject ExitPrefab = null; 
    public GameObject ghostPrefab, ghosts; // Ghost-related prefabs.

    // Maze configuration parameters.
    public int Rows = 5; // Number of rows in the maze.
    public int Columns = 5; // Number of columns in the maze.
    public float CellWidth = 5; // Width of each maze cell.
    public float CellHeight = 5; // Height of each maze cell.
    public bool AddGaps = true; // Whether to add gaps between cells.
    public GameObject[] Goodies; // Collectibles placed in the maze.
    public int GoodiesCount = 10; // Number of collectibles to spawn.

    private BasicMazeGenerator mMazeGenerator = null; // Instance of the maze generator.

    /// <summary>
    /// Spawns the maze based on input dimensions and goodies count.
    /// </summary>
    public void SpawnMaze(int X, int Y, int Bossts) {
        // Update maze dimensions and collectible count.
        Rows = X;
        Columns = Y;
        GoodiesCount = Bossts;

        // Set random seed if not using full randomization.
        if (!FullRandom) {
            Random.seed = RandomSeed;
        }

        // Select the maze generation algorithm.
        switch (Algorithm) {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }

        // Generate the maze structure.
        mMazeGenerator.GenerateMaze();

        // Create the maze objects in the scene.
        CreateMazeObjects();

        // Place the entrance and exit of the maze.
        PlaceEntranceAndExit();

        // Add collectibles to the maze.
        PlaceGoodies();

        // Spawn ghosts after a slight delay.
        Invoke("PlaceGhost", 0.5f);

        // Build the NavMesh for AI pathfinding.
        surface.BuildNavMesh();
    }

    /// <summary>
    /// Creates the visual representation of the maze using the generated data.
    /// </summary>
    private void CreateMazeObjects() {
        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {
                // Calculate the position of each cell.
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);

                // Instantiate the floor of the cell.
                GameObject tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity);
                tmp.transform.parent = transform;

                // Skip adding walls for entrance and exit.
                if ((x == 0 && z == 0) || (x == (Rows - 1) * CellHeight && z == (Columns - 1) * CellHeight)) {
                    continue;
                }

                // Instantiate walls based on the maze cell's structure.
                if (cell.WallRight) {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z), Quaternion.Euler(0, 90, 0));
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront) {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2), Quaternion.identity);
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft) {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z), Quaternion.Euler(0, 270, 0));
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack) {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2), Quaternion.Euler(0, 180, 0));
                    tmp.transform.parent = transform;
                }
            }
        }

        // Add pillars at cell intersections if applicable.
        if (Pillar != null) {
            for (int row = 0; row < Rows + 1; row++) {
                for (int column = 0; column < Columns + 1; column++) {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));

                    if ((x == 0 && z == 0) || (x == Rows * CellHeight && z == Columns * CellHeight)) {
                        continue;
                    }

                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
                    tmp.transform.parent = transform;
                }
            }
        }
    }

    /// <summary>
    /// Places the entrance and exit of the maze.
    /// </summary>
    private void PlaceEntranceAndExit() {
        if (ExitPrefab == null) return;

        // Place the exit at the bottom-right corner.
        float exitX = Columns * (CellWidth + (AddGaps ? .2f : 0));
        float exitZ = Rows * (CellHeight + (AddGaps ? .2f : 0));
        Instantiate(ExitPrefab, new Vector3(exitX, 0, exitZ), Quaternion.identity);

        // Remove walls for the exit.
        MazeCell exitCell = mMazeGenerator.GetMazeCell(Rows - 1, Columns - 1);
        exitCell.WallRight = false;
        exitCell.WallFront = false;

        if (EntrancePrefab == null) return;

        // Place the entrance at the top-left corner.
        float entranceX = 0;
        float entranceZ = 0;
        Instantiate(EntrancePrefab, new Vector3(entranceX, 1, entranceZ), Quaternion.identity);

        // Remove walls for the entrance.
        MazeCell entranceCell = mMazeGenerator.GetMazeCell(0, 0);
        entranceCell.WallLeft = false;
        entranceCell.WallBack = false;
    }

    /// <summary>
    /// Randomly places goodies (collectibles) in the maze.
    /// </summary>
    private void PlaceGoodies() {
        if (Goodies == null || Goodies.Length == 0 || GoodiesCount <= 0) return;

        System.Random rnd = new System.Random();
        for (int i = 0; i < GoodiesCount; i++) {
            int row, column;
            do {
                row = rnd.Next(0, Rows);
                column = rnd.Next(0, Columns);
            } while (mMazeGenerator.GetMazeCell(row, column).IsGoal); // Avoid entrance/exit cells.

            float x = column * (CellWidth + (AddGaps ? .2f : 0));
            float z = row * (CellHeight + (AddGaps ? .2f : 0));
            var goodie = Goodies[rnd.Next(0, Goodies.Length)];
            Instantiate(goodie, new Vector3(x, 1, z), Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns ghosts in random maze locations.
    /// </summary>
    private void PlaceGhost() {
        System.Random rnd = new System.Random();
        for (int i = 0; i < LvlController.ghost; i++) {
            int row, column;
            do {
                row = rnd.Next(0, Rows);
                column = rnd.Next(0, Columns);
            } while (mMazeGenerator.GetMazeCell(row, column).IsGoal);

            float x = column * (CellWidth + (AddGaps ? .2f : 0));
            float z = row * (CellHeight + (AddGaps ? .2f : 0));
            Instantiate(ghostPrefab, new Vector3(x, 1, z), Quaternion.identity, ghosts.transform);
        }
    }
}

