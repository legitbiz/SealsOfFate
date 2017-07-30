

/******************************************************************************
 * File     : StateAlert.cs
 * Purpose  : The state an enemy is in when it is within activation range but it
 * hasn't noticed the player yet.
 * Notes    : For this prototype implementation, the state immediately
 * transitions to the playerSeek state.
 ******************************************************************************/
//Copyright 2017 Andrew Waugh, Licensed under the terms of the MIT license.
public class StateAlert : State<Enemy> {
    private static StateAlert instance;

    public override void Enter(Enemy owner) {
        owner.StateMachine.ChangeState(StateSeekPlayer.getInstance());
    }

    public override void Execute(Enemy owner) { }

    public override void Exit(Enemy owner) { }

    public static StateAlert getInstance() {
        return instance ?? (instance = new StateAlert());
    }
}