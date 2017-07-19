using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

    /// <summary>
    ///   This class contains logic for processing player input and interacting with other relevant
    ///   GameObjects in the generated scene.
    /// </summary>
    public class Player : MovingObject
    {
        /// <summary>Delay duration in seconds to restart level.</summary>
        public float restartLevelDelay = 1f;
        /// <summary>Number of points to add to player food resource when picking up a food object.</summary>
        public int pointsPerFood = 10;
        /// <summary>Number of points to add to player food resource when picking up a soda object.</summary>
        public int pointsPerSoda = 20;
        /// <summary>How much damage the Player inflicts to the Wall object when it attacks.</summary>
        public int wallDamage = 1;
        /// <summary>Stores a reference to the Player's animator component.</summary>
        private Animator animator;
        /// <summary>Stores the Player's current food points during the level.</summary>
        private int food;
        
        /// <summary>
        ///   Configures the Player state on entry to the scene.
        /// </summary>
        protected override void Start ()
        {
            // Get a component reference to the Player's animator component
            animator = GetComponent<Animator>();
            
            // Get the current food point total stored in GameManager.instance between levels.
            food = GameManager.instance.playerHealth;
            
            // Call the Start function of the MovingObject base class.
            base.Start ();
        }
        
        /// <summary>
        ///   Stores the Player state in the GameManager when the Player GameObject is disabled.
        ///   This is done to carry over the Player state from the current level to the next level.
        /// </summary>
        private void OnDisable ()
        {
        // Currently only storing the Player's food
            GameManager.instance.playerHealth = food;
        }
        
        /// <summary>
        ///   If it's the Player's turn, this method handles updating the Player game logic
        ///   (such as translating player input into movement) on each engine tick. 
        /// </summary>
        private void Update ()
        {
            //If it's not the player's turn, exit the function.
            if(!GameManager.instance.playersTurn || GameManager.getInstance().IsMoving) return;
            
            // Receive horizontal (arrow key left/right) input from the Input manager.
            int horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
            
            // Receive vertical (arrow key up/down) input from the Input manager.
            int vertical = (int) (Input.GetAxisRaw ("Vertical"));
            // NOTE: horizontal and vertical are cast to ints to round to a whole number.

            // NOTE: Player movement is limited to exclusively up, down, left, right.
            //       Diagonal/omnidirectional movement is restricted.
            // If horizontal movement detected, vertical is set to 0 to avoid movement
            // along vertical axis.
            if(horizontal != 0)
            {
                vertical = 0;
            }
            
            if(horizontal != 0 || vertical != 0)
            {
                AttemptMove<Enemy>(horizontal, vertical);
            }
        }

        /// <summary>
        ///   Attempts to move the Player in the direction specified by xDir and yDir. This method will 
        /// </summary>
        /// <param name="xDir">The horizontal direction (left or right).</param>
        /// <param name="yDir">The vertical direction (up or down).</param>
        /// <param name="T">
        ///   An obstruction, such as an enemy or a wall, that could possibly
        ///   prohibit movement.
        /// </param>
        protected override void AttemptMove <T> (int xDir, int yDir)
        {
            // Every time player moves, subtract from food points total.
            food--;
            
            // Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove <T> (xDir, yDir);
            
            // Hit allows us to reference the result of the Linecast done in Move.
            RaycastHit2D hit;
            
            // If Move returns true, meaning Player was able to move into an empty space.
            if (Move (xDir, yDir, out hit)) 
            {
                // Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
            }
            
            // Since the player has moved and lost food points, check if the game has ended.
            CheckIfGameOver ();
            
            // Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.instance.playersTurn = false;
        }
        
        /// <summary>
        ///   This method is called when the Player failed to move in the specified direction.
        ///   In the event of a destructible wall or enemy, the player can attack to try to
        ///   remove the obstruction.
        /// </summary>
        /// <param name="component">An interactable entity, such as a wall or an enemy.</param>
        protected override void OnCantMove <T> (T component)
        {

        }
    
        /// <summary>
        ///   Handles collision logic and determines how the Player interaction with
        /// other with other GameObjects on collision are invoked.
        /// </summary>
        /// <param name="other">
        ///   A reference to the GameObject's collider that the 
        ///   Player collided into.
        /// </param>
        private void OnTriggerEnter2D (Collider2D other)
        {
            // Check if the tag of the trigger collided with is Exit.
            if(other.tag == "Exit")
            {
                // Invoke the Restart function to start the next level with a delay of 
                // restartLevelDelay (default 1 second).
                Invoke ("Restart", restartLevelDelay);
                
                // Disable the player object since level is over.
                enabled = false;
            }
            
            // Check if the tag of the trigger collided with is Food.
            else if(other.tag == "Food")
            {
                // Add pointsPerFood to the players current food total.
                food += pointsPerFood;
                
                // Disable the food object the player collided with.
                other.gameObject.SetActive (false);
            }
            
            // Check if the tag of the trigger collided with is Soda.
            else if(other.tag == "Soda")
            {
                // Add pointsPerSoda to players food points total
                food += pointsPerSoda;
                
                
                // Disable the soda object the player collided with.
                other.gameObject.SetActive (false);
            }
        }
        
        /// <summary>
        ///   Reloads the scene.
        /// </summary>
        private void Restart ()
        {
            // Load the last scene loaded, in this case Main, the only scene in the game.
            SceneManager.LoadScene (0);
        }
        
        /// <summary>
        ///   Reduces the Player's food resource.
        /// </summary>
        /// <param name="loss">The amount of food to deduct from Player's food count.</param>
        public void LoseFood (int loss)
        {
            // Set the trigger for the player animator to transition to the playerHit animation.
            animator.SetTrigger ("playerHit");
            
            // Subtract lost food points from the players total.
            food -= loss;
            
            // Check to see if game has ended.
            CheckIfGameOver ();
        }
        
        /// <summary>
        ///   If a defeat condition has been reached (such as running out of food), end the game.
        ///</summary>
        private void CheckIfGameOver ()
        {
            // Check if food point total is less than or equal to zero.
            if (food <= 0) 
            {
                GameManager.instance.GameOver ();
            }
        }
    }
