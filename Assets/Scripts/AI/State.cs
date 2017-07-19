using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/******************************************************************************
 * File     : State.cs
 * Purpose  : State is an abstract base class for a state.
 * Notes    : Based off of the FSM implementation described in
 * "Programming Game AI By Example" by Mat Buckland. Modified for C# and the
 * needs of SealsOfFate.
 ******************************************************************************/
//Copyright 2017 Andrew Waugh, Licensed under the terms of the MIT license.
public abstract class State<EntityT> {
    public abstract void Enter(EntityT owner);

    public abstract void Execute(EntityT owner);

    public abstract void Exit(EntityT owner);

}
