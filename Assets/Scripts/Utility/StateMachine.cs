using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * File     : StateMachine.cs
 * Purpose  : StateMachine is used in the AI system to handle the switching of
 * states. Actual AI Behavior itself is NOT handled by this class, it merely 
 * facilitates the swapping in and out of states.
 * Notes    : Based off of the FSM implementation described in
 * "Programming Game AI By Example" by Mat Buckland. Modified for C# and the
 * needs of SealsOfFate.
 ******************************************************************************/
 //Copyright 2017 Andrew Waugh, Licensed under the terms of the MIT license.

public class StateMachine<EntityT> {

    public StateMachine(EntityT owner)
    {
        this.owner = owner;
        this.currentState = null;
        this.previousState = null;
        this.globalState = null;
    }

    
    /*
	// Use this for initialization
	void Start () {
		
	}
    */
	
	// If we decide to extend StateMachine from MonoBehavior, than Update will be called once per frame. Currently, it is called manually.
	public void Update () {
		//If we have a defined global state, execute it first
        if (globalState != null) { globalState.Execute(owner); }
        //Now run the current state
        if (currentState != null) { currentState.Execute(owner); }
	}

    public void ChangeState(State<EntityT> newState)
    {
        //save our current state as the previous
        previousState = currentState;
        //Call our current state's exit behavior
        currentState.Exit(owner);
        //Update our current state to the new state
        currentState = newState;
        //Call the new state's entrance behavior
        currentState.Enter(owner);
    }

    //Reverts to the previous state
    public void RevertState() { ChangeState(previousState); }

    //Checks to see if the state machine is in a particular state
    public bool IsInState(State<EntityT> check)
    {
        //Use RTTI to check if they're the same type.
        if (check.GetType() == currentState.GetType()) { return true; }
        else { return false; }
    }

    private EntityT owner;                  //The agent that is using this state machine
    private State<EntityT> currentState;    //The current state of the agent
    private State<EntityT> previousState;   //A record of the previous state of the agent
    private State<EntityT> globalState;     //A global state that is checked every update.

    //Properties for states. Note these are only intended for the initial setup.
    public State<EntityT> CurrentState { get { return this.currentState; } set { currentState = value; currentState.Enter(owner); } }
    public State<EntityT> GlobalState { get { return this.globalState; } set { globalState = value; } }
}
