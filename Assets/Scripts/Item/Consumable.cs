using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour, IInteractable {

//    public int HealthMod;
 //   public int ManaMod;

    public void Interact()
    {
        Consume();
    }

    virtual public void Consume()
    {
        // GameManager.instance.playerHealth += HealthMod;
        //     GameManager.instance.playerMana += ManaMod;
    }

}
