using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable {

    Collectable()
    {

    }

    public void Interact()
    {
        Pickup();
    }

    private void Pickup()
    {
        throw new System.NotImplementedException();
    }

}
