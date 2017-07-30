// Copyright 2017 Andrew Waugh, Licensed under the terms of the MIT license.

/// <summary>
///     State is an abstract base class for a state.
/// </summary>
/// <remarks>
///     Based off of the FSM implementation described in "Programming
///     Game AI By Example" by Mat Buckland.Modified for C# and the
/// </remarks>
/// <typeparam name="EntityT"></typeparam>
public abstract class State<EntityT> {
    public abstract void Enter(EntityT owner);

    public abstract void Execute(EntityT owner);

    public abstract void Exit(EntityT owner);
}