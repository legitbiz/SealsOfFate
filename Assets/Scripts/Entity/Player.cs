using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     This class contains logic for processing player input and interacting with other relevant
///     GameObjects in the generated scene.
/// </summary>
public class Player : MovingObject {
    /// <summary>Stores a reference to the Player's animator component.</summary>
    private Animator _animator;

    /// <summary>Stores the Player's current food points during the level.</summary>
    private int _food;

    /// <summary>Number of points to add to player food resource when picking up a food object.</summary>
    public int PointsPerFood = 10;

    /// <summary>Number of points to add to player food resource when picking up a soda object.</summary>
    public int PointsPerSoda = 20;

    /// <summary>Delay duration in seconds to restart level.</summary>
    public float RestartLevelDelay = 1f;

    /// <summary>How much damage the Player inflicts to the Wall object when it attacks.</summary>
    public int WallDamage = 1;

    //// Stat Block 
    /// TODO: Connect this stat block to their appropriate functionality.
    /// <summary>Stores the Player's Health.</summary>
    private ushort _healthPoints;
    
    /// <summary>Stores the Player's Mana.</summary>
    private ushort _manaPoints;
    
    /// <summary>Stores the Player's Damage Reduction (percent value).</summary>
    private byte _damageReduction;
    
    /// <summary>Stores the Player's Movement Speed.</summary>
    private ushort _movementSpeed;
    
    /// <summary>Stores the Player's evasion (percent value).</summary>
    private byte _evasion;
    
    /// <summary>Stores the Player's Physical Damage stat.</summary>
    private ushort _sealie;
    
    /// <summary>Stores the Player's Magic Damage stat.</summary>
    private ushort _unsealie;



    /// <summary>
    ///     Configures the Player state on entry to the scene.
    /// </summary>
    protected override void Start() {
        // Get a component reference to the Player's animator component
        _animator = GetComponent<Animator>();

        // Get the current food point total stored in GameManager.instance between levels.
        _food = GameManager.instance.playerHealth;

        // Call the Start function of the MovingObject base class.
        base.Start();
    }

    /// <summary>
    ///     Stores the Player state in the GameManager when the Player GameObject is disabled.
    ///     This is done to carry over the Player state from the current level to the next level.
    /// </summary>
    /// <remarks>Currently only stores the Player's food.</remarks>
    private void OnDisable() {
        GameManager.instance.playerHealth = _food;
    }

    /// <summary>
    ///     If it's the Player's turn, this method handles updating the Player game logic
    ///     (such as translating player input into movement) on each engine tick.
    /// </summary>
    private void Update() {
        //If it's not the player's turn, exit the function.
        if (!GameManager.instance.playersTurn || GameManager.getInstance().IsMoving) {
            return;
        }

        // Receive horizontal (arrow key left/right) input from the Input manager.
        var horizontal = (int) Input.GetAxisRaw("Horizontal");

        // Receive vertical (arrow key up/down) input from the Input manager.
        var vertical = (int) Input.GetAxisRaw("Vertical");
        // NOTE: horizontal and vertical are cast to ints to round to a whole number.

        // NOTE: Player movement is limited to exclusively up, down, left, right.
        //       Diagonal/omnidirectional movement is restricted.
        // If horizontal movement detected, vertical is set to 0 to avoid movement
        // along vertical axis.
        if (horizontal != 0) {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0) {
            AttemptMove<Component>(horizontal, vertical);
        }
    }

    /// <summary>
    ///     Attempts to move the Player in the direction specified by xDir and yDir. This method will
    /// </summary>
    /// <param name="xDir">The horizontal direction (left or right).</param>
    /// <param name="yDir">The vertical direction (up or down).</param>
    /// <param name="T">
    ///     An obstruction, such as an enemy or a wall, that could possibly
    ///     prohibit movement.
    /// </param>
    protected override void AttemptMove<T>(int xDir, int yDir) {
        // Every time player moves, subtract from food points total.
        _food--;

        // Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);

        // Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;

        // If Move returns true, meaning Player was able to move into an empty space.
        if (Move(xDir, yDir, out hit)) {
            // Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
        }

        // Since the player has moved and lost food points, check if the game has ended.
        CheckIfGameOver();

        // Set the playersTurn boolean of GameManager to false now that players turn is over.
        GameManager.instance.playersTurn = false;
    }

    /// <summary>
    ///     This method is called when the Player failed to move in the specified direction.
    ///     In the event of a destructible wall or enemy, the player can attack to try to
    ///     remove the obstruction.
    /// </summary>
    /// <param name="component">An interactable entity, such as a wall or an enemy.</param>
    protected override void OnCantMove<T>(T component) {
        if (component.tag == "Enemy") {
            Debug.Log("Player attacks penguin");
        }
    }

    /// <summary>
    ///     Handles collision logic and determines how the Player interaction with
    ///     other with other GameObjects on collision are invoked.
    /// </summary>
    /// <param name="other">
    ///     A reference to the GameObject's collider that the
    ///     Player collided into.
    /// </param>
    private void OnTriggerEnter2D(Collider2D other) {
        // Check if the tag of the trigger collided with is Exit.
        switch (other.tag) {
            case "Exit":
                // Invoke the Restart function to start the next level with a delay of 
                // restartLevelDelay (default 1 second).
                Invoke("Restart", RestartLevelDelay);

                // Disable the player object since level is over.
                enabled = false;
                break;
            case "Food":
                // Add pointsPerFood to the players current food total.
                _food += PointsPerFood;

                // Disable the food object the player collided with.
                other.gameObject.SetActive(false);
                break;
            case "Soda":
                // Add pointsPerSoda to players food points total
                _food += PointsPerSoda;


                // Disable the soda object the player collided with.
                other.gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    ///     Reloads the scene.
    /// </summary>
    private void Restart() {
        // Load the last scene loaded, in this case Main, the only scene in the game.
        SceneManager.LoadScene(0);
    }

    /// <summary>
    ///     Reduces the Player's food resource.
    /// </summary>
    /// <param name="loss">The amount of food to deduct from Player's food count.</param>
    public void LoseFood(int loss) {
        // Set the trigger for the player animator to transition to the playerHit animation.
        _animator.SetTrigger("playerHit");

        // Subtract lost food points from the players total.
        _food -= loss;

        // Check to see if game has ended.
        CheckIfGameOver();
    }

    /// <summary>
    ///     If a defeat condition has been reached (such as running out of food), end the game.
    /// </summary>
    private void CheckIfGameOver() {
        // Check if food point total is less than or equal to zero.
        if (_food <= 0) {
            GameManager.instance.GameOver();
        }
    }
}