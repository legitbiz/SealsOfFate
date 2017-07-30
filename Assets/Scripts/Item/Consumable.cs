using UnityEngine;

/// <summary>
///     Consumable object - does something to the critter picking it up
/// </summary>
public abstract class Consumable : MonoBehaviour, IInteractable {
    //    public int HealthMod;
    //   public int ManaMod;

    /// <summary>
    ///     Implementation of the IInteractable interface
    /// </summary>
    /// <remarks>
    ///     Calls Consumable.Consume()
    /// </remarks>
    public void Interact() {
        Consume();
    }

    /// <summary>
    ///     Consumes the object
    /// </summary>
    public virtual void Consume() {
        // GameManager.instance.playerHealth += HealthMod;
        //     GameManager.instance.playerMana += ManaMod;
    }
}