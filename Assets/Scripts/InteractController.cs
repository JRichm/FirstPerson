using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] private float reachDistance;

    private FirstPersonController movementController;
    private Camera playerCam;

    private void Awake() {
        if (this.gameObject.layer != 3) {
            Debug.Log("InteractController located on non player object");
            Destroy(this);
            return;
        }
        movementController = GetComponent<FirstPersonController>();
        playerCam = movementController.playerCamera;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {

            RaycastHit lookPoint;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out lookPoint)) {
                if (lookPoint.distance <= reachDistance) {
                    Interactable objectInteractable = lookPoint.collider.gameObject.GetComponent<Interactable>();
                    if (objectInteractable != null) {
                        objectInteractable.Interact(this);
                    }
                }
            }

            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * reachDistance, Color.green, 3);
            Debug.DrawRay(playerCam.transform.position + (playerCam.transform.forward * reachDistance), playerCam.transform.forward * 1, Color.red, 3);
        }
    }
}
