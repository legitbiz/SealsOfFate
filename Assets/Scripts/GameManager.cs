using System.Collections.Generic;
using Assets.Scripts.LevelGeneration;
using UnityEngine;


// Level Manager for the Seals of Fate Handles creating the level and
// delegates the room creation 
namespace Assets.Scripts {
    public class GameManager : MonoBehaviour {
        private static readonly float MinimumDistanceFromPlayer = 200f;
        public static GameManager Instance;
        private readonly int currentLevel = 1;
        private int _enemyTurn;

        private List<MovingObject> _entitiesToMove;
        public int MaxLevel;

        public int PlayerHealth;

        public bool PlayersTurn = true;

        public GameManager() {
            IsMoving = false;
        }

        public bool IsMoving { get; set; }

        public LevelManager LevelScript { get; set; }

        /// <summary>Get the CurrentLevelFeatureMap</summary>
        internal static int[,] CurrentLevelFeatureMap {
            get { return Instance.LevelScript.CurrentLevel.FeatureMap; }
        }

        /// <summary>
        ///     Registers an enemy so we can make it move
        /// </summary>
        /// <param name="enemyToRegister">The enemy to move</param>
        public void RegisterEnemy(MovingObject enemyToRegister) {
            _entitiesToMove.Add(enemyToRegister);
        }

        /// <summary>
        ///     Unregisters an enemy so we don't try to make dead people move.
        /// </summary>
        /// <param name="enemyToRemove">The enemy to unregister</param>
        public void UnregisterEnemy(MovingObject enemyToRemove) {
            _entitiesToMove.Remove(enemyToRemove);
        }

        /// <summary>
        ///     Initializes the GameManager
        /// </summary>
        private void Awake() {
            // Singleton Code
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            // Grab the currently attached levelManager script
            LevelScript = GetComponent<LevelManager>();

            // Setup the level.
            _entitiesToMove = new List<MovingObject>();
            InitLevel();
        }

        /// <summary>
        ///     Handles returning the game control to player and running other entities
        /// </summary>
        private void Update() {
            // Bail if this is the player's turn
            if (PlayersTurn) {
                return;
            }

            // If there are no entities left to move, or someone is moving, bail out.
            if (_entitiesToMove == null || IsMoving) {
                return;
            }

            // While we have enemies who haven't taken their turn yet
            // Note that not all turns involve movement, so we should continue
            // looping until something moves.
            while (_enemyTurn < _entitiesToMove.Count) {
                if (IsMoving) {
                    // If something started moving, break from the while loop.
                    // finish the remaining enemies on the next update step(s).
                    break;
                }

                // Defensive check to make sure it's not a null pointer.
                // This shouldn't ever actually happen
                if (_entitiesToMove[_enemyTurn] != null) {
                    // Cast to an enemy, and update its state machine
                    // Candidate for refactoring to avoid the cast
                    var enemy = (Enemy) _entitiesToMove[_enemyTurn];
                    var distanceFromPlayer = DistanceFromPlayer(enemy);

                    if (distanceFromPlayer >= MinimumDistanceFromPlayer) {
                        enemy.StateMachine.ChangeState(StateAsleep.getInstance());
                    } else if (distanceFromPlayer < MinimumDistanceFromPlayer && enemy.IsAsleep()) {
                        enemy.StateMachine.RevertState();
                    }
                    enemy.StateMachine.Update();
                }

                //Increment to the next enemy
                ++_enemyTurn;
            }

            // If all enemies have taken their turn, reset the index and
            // let the player take their turn.
            if (_enemyTurn >= _entitiesToMove.Count) {
                PlayersTurn = true;
                _enemyTurn = 0;
            }
        }

        /// <summary>
        ///     Determines the distance between the player and an enemy
        /// </summary>
        /// <param name="enemy">The enemy</param>
        /// <returns>A float representation of the distance from the player</returns>
        private static float DistanceFromPlayer(Enemy enemy) {
            return (GameObject.FindGameObjectWithTag("Player").transform.position
                    - enemy.transform.position).sqrMagnitude;
        }

        /// <summary>
        ///     Returns an instance of the GameManager
        /// </summary>
        /// <returns></returns>
        public static GameManager GetInstance() {
            return Instance;
        }

        /// <summary>
        ///     Sets up the level
        /// </summary>
        private void InitLevel() {
            LevelScript.SetupScene(currentLevel);
        }

        /// <summary>
        ///     Something, something, end of the game.
        /// </summary>
        public void GameOver() { }
    }
}