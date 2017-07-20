using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Transform _boardHolder;
    public int Columns;
    public GameObject[] Doors;
    public GameObject[] Enemies;
    public GameObject[] Floors;
    private readonly List<Vector3> _gridPositions = new List<Vector3>();
    public GameObject[] Loot;
    public GameObject[] Obstacles;
    public int Rows;
    public GameObject[] Walls;

    /// <summary>
    /// Initializes the level manager.
    /// </summary>
    private void Start()
    {
    }

    /// <summary>Called once per frame. Updates stuff.</summary>
    private void Update()
    {
    }

    /// <summary>
    /// Clears our list gridPositions and prepares it to generate a new board.
    /// </summary>
    private void InitialiseList()
    {
        // Clear our list gridPositions.
        _gridPositions.Clear();

        // Loop through x axis (columns).
        for (var x = 1; x < Columns - 1; x++)
        {
            // Within each column, loop through y axis (rows).
            for (var y = 1; y < Rows - 1; y++)
            {
                // At each index add a new Vector3 to our list with the x and y coordinates of that position.
                _gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    /// <summary>
    /// Sets up the outer walls and floor (background) of the game board.
    /// </summary>
    private void BoardSetup()
    {
        // Instantiate Board and set boardHolder to its transform.
        _boardHolder = new GameObject("Board").transform;

        // Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (var x = -1; x < Columns + 1; x++)
        {
            // Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (var y = -1; y < Rows + 1; y++)
            {
                // Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                var toInstantiate = Floors[Random.Range(0, Floors.Length)];

                // Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 || x == Columns || y == -1 || y == Rows)
                {
                    toInstantiate = Walls[Random.Range(0, Walls.Length)];
                }

                // Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                var instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                // Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(_boardHolder);
            }
        }
    }

    /// <summary>
    /// RandomPosition returns a random position from our list gridPositions.
    /// </summary>
    /// <returns>A 3D Vector</returns>
    private Vector3 RandomPosition()
    {
        // Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        var randomIndex = Random.Range(0, _gridPositions.Count);

        // Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        var randomPosition = _gridPositions[randomIndex];

        // Remove the entry at randomIndex from the list so that it can't be re-used.
        _gridPositions.RemoveAt(randomIndex);

        // Return the randomly selected Vector3 position.
        return randomPosition;
    }

    /// <summary>
    /// LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    /// </summary>
    /// <param name="tileArray"></param>
    /// <param name="minimum"></param>
    /// <param name="maximum"></param>
    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // Choose a random number of objects to instantiate within the minimum and maximum limits
        var objectCount = Random.Range(minimum, maximum + 1);

        // Instantiate objects until the randomly chosen limit objectCount is reached
        for (var i = 0; i < objectCount; i++)
        {
            // Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            var randomPosition = RandomPosition();

            // Choose a random tile from tileArray and assign it to tileChoice
            var tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            // Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// SetupScene initializes our level and calls the previous functions to lay out the game board
    /// </summary>
    /// <param name="level"></param>
    public void SetupScene(int level)
    {
        // Creates the outer walls and floor.
        BoardSetup();

        // Reset our list of gridpositions.
        InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        //int enemyCount = (int)Mathf.Log(level, 2f);
        var enemyCount = 2;
        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(Enemies, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        //Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    /// <summary>
    /// A range of integral values
    /// </summary>
    public class Range
    {
        public int Max;
        public int Min;

        private Range(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}