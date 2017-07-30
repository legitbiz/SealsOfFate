// Copyright 2017 Andrew Waugh, Licensed under the terms of the MIT license.

/// <summary>
///     StateMachine is used in the AI system to handle the switching of
///     states. Actual AI Behavior itself is NOT handled by this class, it
///     merely facilitates the swapping in and out of states.
/// </summary>
/// <remarks>
///     Based off of the FSM implementation described in "Programming Game
///     AI By Example" by Mat Buckland. Modified for C# and the needs of
///     SealsOfFate.
/// </remarks>
/// <typeparam name="EntityT"></typeparam>
public class StateMachine<EntityT> {
    private readonly EntityT _owner; // The agent that is using this state machine
    private State<EntityT> _currentState; // The current state of the agent
    private State<EntityT> _previousState; // A record of the previous state of the agent

    public StateMachine(EntityT owner) {
        _owner = owner;
        _currentState = null;
        _previousState = null;
        GlobalState = null;
    }

    /// <summary>
    ///     Current agent state
    /// </summary>
    public State<EntityT> CurrentState {
        get { return _currentState; }
        set {
            _currentState = value;
            _currentState.Enter(_owner);
        }
    }

    /// <summary>
    ///     Global state that is checked every update.
    /// </summary>
    public State<EntityT> GlobalState { get; set; }

    /// <summary>
    ///     Calls update.
    /// </summary>
    /// <remarks>
    ///     This must be called manually since StateMachine does not inherit from MonoBehaviour.
    /// </remarks>
    // 
    public void Update() {
        // If we have a defined global state, execute it first
        if (GlobalState != null) {
            GlobalState.Execute(_owner);
        }

        // Now run the current state
        if (_currentState != null) {
            _currentState.Execute(_owner);
        }
    }

    public void ChangeState(State<EntityT> newState) {
        //save our current state as the previous
        _previousState = _currentState;
        //Call our current state's exit behavior
        _currentState.Exit(_owner);
        //Update our current state to the new state
        _currentState = newState;
        //Call the new state's entrance behavior
        _currentState.Enter(_owner);
    }

    /// <summary>
    ///     Reverts to the previous state.
    /// </summary>
    public void RevertState() {
        ChangeState(_previousState);
    }

    /// <summary>
    ///     Checks to see if the state machine is in a particular state
    /// </summary>
    public bool IsInState(State<EntityT> check) {
        return check == _currentState;
    }
}