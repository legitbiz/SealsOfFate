using System;
using UnityEngine;

/// <summary>
///     Collectable items thatn can be picked up
/// </summary>
public class Collectable : MonoBehaviour, IInteractable {
    /// <summary>
    ///     A default constructor
    /// </summary>
    private Collectable() { }

    /// <summary>
    ///     The interact method from IInteractable
    /// </summary>
    public void Interact() {
        Pickup();
    }

    /// <summary>
    ///     Pick up the Collectable item.
    /// </summary>
    private void Pickup() {
        throw new NotImplementedException();
    }
}