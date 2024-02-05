using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public void Interact(InteractController controller) {
        Debug.Log("Interacted with " + this.gameObject.name);
        if (this.gameObject.GetComponent<GunScript>() != null) {
            
        }
    }

}
