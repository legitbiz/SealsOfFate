using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Level Manager for the Seals of Fate Handles creating the level and
// delegates the room creation 
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private LevelManager levelScript;
    private int currentLevel = 1;
    private int enemyTurn = 0;
    private bool isMoving = false;
    public int maxLevel;

    public bool playersTurn = true;

    private List<MovingObject> entitiesToMove;

    public short playerHealth;

    public void RegisterEnemy(MovingObject enemyToRegister)
    {
        entitiesToMove.Add(enemyToRegister);
    }

    // Use this for initialization
    void Awake()
    {
        // Singleton Code
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // Grab the currently attached levelManager script
        levelScript = GetComponent<LevelManager>();
        // Setup the level.
        entitiesToMove = new List<MovingObject>();
        InitLevel();

    }

    // Handles returning the game control to player and running other entities
    private void Update()
    {
        // Wait until the player has finished deciding their turn
        if (!playersTurn)
        {
            //If there are entities (enemies) to take their turns, and everything has finished movement.
            if (entitiesToMove != null && !IsMoving)
            {
                //While we have enemies who haven't taken their turn yet
                //Note that not all turns involve movement, so we should continue
                //looping until something moves.
                while (enemyTurn < entitiesToMove.Count)
                {
                    if (IsMoving == false)
                    {
                        //Defensive check to make sure it's not a null pointer.
                        //This shouldn't ever actually happen
                        if (entitiesToMove[enemyTurn] != null)
                        {
                            //Cast to an enemy, and update it's state machine
                            //Candidate for refactoring to avoid the cast
                            Enemy enemy = (Enemy)entitiesToMove[enemyTurn];
                            enemy.StateMachine.Update();
                            
                        }
                        //Increment to the next enemy
                        ++enemyTurn;
                    }
                    //If something started moving, break from the while loop.
                    //finish the remaining enemies on the next update step(s).
                    else break;
                }
                //If all enemies have taken their turn, reset the index and
                //let the player take their turn.
                if (enemyTurn >= entitiesToMove.Count)
                {
                    playersTurn = true;
                    enemyTurn = 0;
                }
            }
        }
    }

    public static GameManager getInstance() { return instance; }

    void InitLevel()
    {
        levelScript.SetupScene(currentLevel);
    }

    public void GameOver()
    {

    }

    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
}
