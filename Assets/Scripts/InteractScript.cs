using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour {
    public void Interact() {
        Debug.Log("Interacted with " + gameObject.name);
        //GetComponent<GunScript>().PickUp(controller);
    }
}
