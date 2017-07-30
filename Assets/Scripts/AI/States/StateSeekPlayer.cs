

/******************************************************************************
 * File     : StateSeekPlayer.cs
 * Purpose  : The state for an enemy seeking the player.
 * Notes    : 
 ******************************************************************************/
//Copyright 2017 Andrew Waugh, Licensed under the terms of the MIT license.
public class StateSeekPlayer : State<Enemy> {
    private static StateSeekPlayer instance;

    public override void Enter(Enemy owner) { }

    public override void Execute(Enemy owner) {
        owner.SeekPlayer();
    }

    public override void Exit(Enemy owner) { }

    public static StateSeekPlayer getInstance() {
        if (instance == null) {
            instance = new StateSeekPlayer();
        }

        return instance;
    }
}