using System.Collections.Generic;
using Assets.Scripts.LevelGeneration;
using UnityEngine;


// Level Manager for the Seals of Fate Handles creating the level and
// delegates the room creation 
namespace Assets.Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        private readonly int currentLevel = 1;
        private int _enemyTurn;

        private List<MovingObject> _entitiesToMove;
        private LevelManager _levelScript;
        public int MaxLevel;

        public int PlayerHealth;

        public bool PlayersTurn = true;

        public GameManager() {
            IsMoving = false;
        }

        public bool IsMoving { get; set; }
        public LevelManager LevelScript {
            get { return _levelScript; }
            set { _levelScript = value; }
        }

        public void RegisterEnemy(MovingObject enemyToRegister) {
            _entitiesToMove.Add(enemyToRegister);
        }

        public void UnregisterEnemy(MovingObject enemyToRemove) {
            _entitiesToMove.Remove(enemyToRemove);
        }

        // Use this for initialization
        private void Awake() {
            // Singleton Code
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            // Grab the currently attached levelManager script
            LevelScript = GetComponent<LevelManager>();
            // Setup the level.
            _entitiesToMove = new List<MovingObject>();
            InitLevel();
        }

        // Handles returning the game control to player and running other entities
        private void Update() {
            // Wait until the player has finished deciding their turn
            if (!PlayersTurn) {
                //If there are entities (enemies) to take their turns, and everything has finished movement.
                if (_entitiesToMove != null && !IsMoving) {
                    //While we have enemies who haven't taken their turn yet
                    //Note that not all turns involve movement, so we should continue
                    //looping until something moves.
                    while (_enemyTurn < _entitiesToMove.Count) {
                        if (IsMoving == false) {
                            //Defensive check to make sure it's not a null pointer.
                            //This shouldn't ever actually happen
                            if (_entitiesToMove[_enemyTurn] != null) {
                                //Cast to an enemy, and update it's state machine
                                //Candidate for refactoring to avoid the cast
                                var enemy = (Enemy) _entitiesToMove[_enemyTurn];
                                var distanceFromPlayer = (GameObject.FindGameObjectWithTag("Player").transform.position
                                    - enemy.transform.position).sqrMagnitude;
                                if (distanceFromPlayer >= 200f) {
                                    enemy.StateMachine.ChangeState(StateAsleep.getInstance());
                                }
                                else if (distanceFromPlayer < 200f && enemy.StateMachine.IsInState(StateAsleep.getInstance())) {
                                    enemy.StateMachine.RevertState();
                                }
                                enemy.StateMachine.Update();
                            }
                            //Increment to the next enemy
                            ++_enemyTurn;
                        }
                        //If something started moving, break from the while loop.
                        //finish the remaining enemies on the next update step(s).
                        else {
                            break;
                        }
                    }
                    //If all enemies have taken their turn, reset the index and
                    //let the player take their turn.
                    if (_enemyTurn >= _entitiesToMove.Count) {
                        PlayersTurn = true;
                        _enemyTurn = 0;
                    }
                }
            }
        }

        public static GameManager GetInstance() {
            return Instance;
        }

        private void InitLevel() {
            LevelScript.SetupScene(currentLevel);
        }

        public void GameOver() { }
    }
}