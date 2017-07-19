using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int speed;
    public int min_range;
    public int max_range;

    private StateMachine<Enemy> stateMachine;

    Enemy() : base()
    {
        stateMachine = new StateMachine<Enemy>(this);
        stateMachine.CurrentState = StateAlert.getInstance();
    }

    void Awake()
    {
        GameManager.instance.RegisterEnemy(this);
    }

    public StateMachine<Enemy> getStateMachine()
    {
        return stateMachine;
    }

    //Attempts to move this enemy towards the player
    public void SeekPlayer()
    {
        int horizontal, vertical;

        //Find the player
        Player playerObj = GameObject.FindObjectOfType<Player>();

        //Calculate a vector pointing from this enemy to the player
        Vector2 playerDir =  playerObj.transform.position - transform.position;

        //Normalize the vector to a magnitude of 1
        playerDir.Normalize();

        //Travel in the direction of whichever component is larger. Note that we always go vertical in the case of a tie.
        //This could be refactored to a coin flip.
        if (Math.Abs(playerDir.x) > Math.Abs(playerDir.y)) { horizontal = 1; vertical = 0; }
        else { horizontal = 0; vertical = 1; }

        //If the original vector was negative, flip our movement direction.
        if (playerDir.x <0) { horizontal *= -1;  }
        if (playerDir.y < 0 ) { vertical *= -1; }

        //move in the direction given
        AttemptMove<Player>(horizontal,vertical);
    }
    protected override void OnCantMove<T>(T component)
    {
        return;
        //throw new NotImplementedException();
    }

}
