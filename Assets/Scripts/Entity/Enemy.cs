using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

/// <summary>
/// This class is the general enemy class. It extends MovingObject and is expected to be extended by more specific
/// classes for particular enemy behavior. It defines general functions that most enemies will need.
/// </summary>
public class Enemy : MovingObject
{
    /// <summary>The normal movement speed of the enemy</summary>
    public int speed;
    /// <summary> The minimum attack range for an enemy. Enemies will try to stay above this range</summary>
    public int min_range;
    /// <summary> The maximum attack range for an enemy. Enemies will try to stay below this range </summary>
    public int max_range;

    /// <summary> The state machine that handles state transitions. </summary>
    private StateMachine<Enemy> stateMachine;

    Enemy() : base()
    {
        stateMachine = new StateMachine<Enemy>(this);
        stateMachine.CurrentState = StateAlert.getInstance();
    }

    /// <summary>
    /// Sets up the enemy on load and registers it with the game manager.
    /// </summary>
    void Awake()
    {
        GameManager.instance.RegisterEnemy(this);
    }

    /// <summary>
    /// Returns the state machine instance
    /// </summary>
    /// <returns></returns>
    public StateMachine<Enemy> getStateMachine()
    {
        return stateMachine;
    }

    /// <summary>
    /// Attempts to move this enemy towards the player. 
    /// </summary>
    public void SeekPlayer()
    {
        int horizontal, vertical;

        //Find the player
        Player playerObj = GameObject.FindObjectOfType<Player>();

        //Calculate a vector pointing from this enemy to the player
        Vector2 playerDir =  playerObj.transform.position - transform.position;

        //Normalize the vector to a magnitude of 1
        playerDir.Normalize();

        //***Decompose to pure horizontal and vertical***
        //Travel in the direction of whichever component is larger. In case of a tie, do a coin flip.
        float coinFlip;
        if (Math.Abs(Math.Abs(playerDir.x) - Math.Abs(playerDir.y)) < FloatComparer.kEpsilon) {
            coinFlip = UnityEngine.Random.value;
            if (coinFlip >=0.51) //Random.value returns a number between 0.0 and 1.0 inclusively
            {
                horizontal = 1;
                vertical = 0;
            }
            else
            {
                vertical = 1;
                horizontal = 0;
            }
        }
        else if (Math.Abs(playerDir.x) > Math.Abs(playerDir.y)) { horizontal = 1; vertical = 0; }
        else { horizontal = 0; vertical = 1; }

        //If the original vector was negative, flip our movement direction.
        if (playerDir.x <0) { horizontal *= -1;  }
        if (playerDir.y < 0 ) { vertical *= -1; }

        //***Simple and stupid obstacle avoidance***
        //Raycast in the direction of travel, if it hits a non-player blocking object, randomly generate a new location
        //to move to. Placeholder for actual pathfinding.
        RaycastHit2D hit;
        if (RaycastInDirection(horizontal,vertical,out hit))
        {

            while (RaycastInDirection(horizontal, vertical, out hit) && hit.transform != playerObj.transform)
            {
                horizontal = (int)(UnityEngine.Random.Range(0, 1.99f));
                if (horizontal == 0) { vertical = 1; }

                coinFlip = UnityEngine.Random.value;
                if (coinFlip >= 0.51) { horizontal *= -1; vertical *= -1; }
            }
        }

        //move in the direction given
        AttemptMove<Player>(horizontal,vertical);
    }
    protected override void OnCantMove<T>(T component)
    {
        return;
        //throw new NotImplementedException();
    }

}
