using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    [SerializeField] int maxInventorySize;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject weaponHolderAnchor;
    [SerializeField] private Camera playerCam;

    private List<GameObject> weaponInventory = new List<GameObject>();
    private int currentWeaponIndex = 0;

    private void Update() {

        weaponHolderAnchor.transform.rotation = playerCam.transform.rotation;
        Vector2 scrollDelta = Input.mouseScrollDelta;
        if (scrollDelta[1] != 0) {
            SwitchWeapon((int)scrollDelta[1]);
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            DropWeapon(currentWeaponIndex);
        }

        if (Input.GetKey(KeyCode.Mouse0)) {
            if (weaponInventory[currentWeaponIndex] != null) {
                weaponInventory[currentWeaponIndex].GetComponent<GunScript>().HandleShoot();
            }
        }
    }

    public void EquipWeapon(GameObject weaponObj) {
        if (weaponObj == null) {
            Debug.LogError("Attempted to equip a null weapon object.");
            return;
        }

        // Check if the weapon object has the required components
        BoxCollider weaponCollider = weaponObj.GetComponent<BoxCollider>();
        Rigidbody weaponRB = weaponObj.GetComponent<Rigidbody>();
        GunScript gunScript = weaponObj.GetComponent<GunScript>();

        if (weaponCollider == null || weaponRB == null || gunScript == null) {
            Debug.LogError("The weapon object is missing required components.");
            return;
        }

        Debug.Log(weaponInventory.Count);
        Debug.Log(maxInventorySize);

        if (weaponInventory.Count < maxInventorySize) {
            // add weapon to player inventory
            weaponInventory.Add(weaponObj);

            // disable weapon collider while being held 
            weaponCollider.enabled = false;

            // remove weapon physics
            weaponRB.isKinematic = true;

            // tell gun script it is equipped
            gunScript.EquipWeapon(playerCam);

            // set weapons parent to player's weapon holder and set position/rotation
            weaponObj.transform.parent = weaponHolder.transform;
            weaponObj.transform.position = weaponHolder.transform.position;
            weaponObj.transform.rotation = weaponHolder.transform.rotation;

            // switch to the new weapon
            SwitchWeapon(1);

        } else {
            Debug.Log("Not enough space in inventory");
        }
    }

    private void SwitchWeapon(int direction) {
        int newIndex = currentWeaponIndex + direction;

        if (newIndex >= maxInventorySize) {
            newIndex = 0;
        } else if (newIndex < 0) {
            newIndex = maxInventorySize - 1;
        }

        if (weaponInventory.Count > newIndex) {
            foreach (var weapon in weaponInventory) {
                weapon.GetComponentInChildren<MeshRenderer>().enabled = false;
            }

            weaponInventory[newIndex].GetComponentInChildren<MeshRenderer>().enabled = true;
            currentWeaponIndex = newIndex;
        }
    }

    private void DropWeapon(int weaponIndex) {

        // get gameobject of current weapon
        GameObject weaponObject = weaponInventory[weaponIndex];

        // get rigidbody of current weapon
        Rigidbody weaponRB = weaponObject.GetComponent<Rigidbody>();

        // remove weapon parent
        weaponObject.transform.parent = null;

        weaponObject.GetComponent<GunScript>().DropWeapon();

        // re-enable weapon collider
        weaponObject.GetComponent<BoxCollider>().enabled = true;

        // make gun react to physics again and add 'throw' force
        weaponRB.isKinematic = false;
        weaponRB.AddForce(playerCam.transform.forward * 5, ForceMode.Impulse);
        weaponRB.AddRelativeTorque(new Vector3(Random.Range(-7, 7), Random.Range(-7, 7), Random.Range(-7, 7)));

        // remove weapon from player inventory
        weaponInventory.RemoveAt(weaponIndex);

        // reduce current weapon index
        SwitchWeapon(-1);
    }
}
