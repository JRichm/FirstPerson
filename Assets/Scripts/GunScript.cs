using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GunScript : MonoBehaviour {

    private bool isEquipped;
    private int clipSize = 30;
    private int currBullets;
    private int rsrvBullets;
    private int maxAmmo;
    private float timeBetweenShots;
    private Camera playerCamera;

    private InteractScript interactScript;
    private AudioSource audioSource;
    [SerializeField] private GameObject shootFromPoint;
    //[SerializeField] private Material tracerMaterial;
    private float shootTime;


    private void Start() {

        isEquipped = false;

        currBullets = clipSize;
        maxAmmo = clipSize * 5;
        rsrvBullets = clipSize * 2;
        timeBetweenShots = 0.125f;
        shootTime = 0;

        interactScript = this.AddComponent<InteractScript>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (shootTime > 0) {
            shootTime -= Time.deltaTime;
        }
    }

    public void HandleShoot() {
        if (currBullets > 0) {
            if (shootTime <= 0) {

                RaycastHit lookatPoint;

                Vector3 bulletLandPoint;

                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out lookatPoint)) {
                    bulletLandPoint = lookatPoint.point;
                } else {
                    bulletLandPoint = playerCamera.transform.forward * 1000;
                }

                audioSource.Play();
                //ShowTracer(bulletLandPoint);
                currBullets--;

                shootTime = timeBetweenShots;
            }
        } else if (rsrvBullets < 0) {
            Debug.Log("No ammo");
            Reload();
        }
    }

    public void Reload() {

        // check if player needs to reload
        if (currBullets < clipSize) {

            // check if player has any bullets in reserve
            if (rsrvBullets > 0) {

                // if player doesnt have enough reserve bullets to top off mag
                if (rsrvBullets < clipSize - currBullets) {
                    currBullets += rsrvBullets;
                    rsrvBullets = 0;
                    Debug.Log("Partially reloaded");

                    // fill mag and subtract from reserve bullets
                } else {
                    currBullets = clipSize;
                    rsrvBullets -= clipSize - currBullets;
                    Debug.Log("Fully reloaded");
                }
            } else {
                Debug.Log("No bullets in reserve");
            }
            Debug.Log("Don't need to reload");
        } else {
        }
    }

    public void EquipWeapon(Camera _playerCamera) {
        playerCamera = _playerCamera;
        isEquipped = true;
    }

    public void DropWeapon() {
        isEquipped = false;
    }

    private void ShowTracer(Vector3 bulletLandPoint) {
        //GameObject newTracer = new GameObject("Tracer");
        //TracerScript tracerScript = newTracer.AddComponent<TracerScript>();
        //tracerScript.ShowTracer(shootFromPoint.transform.position, bulletLandPoint, tracerMaterial);
    }
}