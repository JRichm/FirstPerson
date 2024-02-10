using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GunScript : MonoBehaviour {

    private readonly int clipSize = 30;
    private int currBullets;
    private int rsrvBullets;
    private float timeBetweenShots;
    private Camera playerCamera;

    private AudioSource audioSource;
    [SerializeField] private GameObject shootFromPoint;
    //[SerializeField] private Material tracerMaterial;
    private float shootTime;


    private void Start() {

        currBullets = clipSize;
        rsrvBullets = clipSize * 2;
        timeBetweenShots = 0.125f;
        shootTime = 0;

        this.AddComponent<InteractScript>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (shootTime > 0) {
            shootTime -= Time.deltaTime;
        }
    }

    public void HandleShoot() {

        // make sure player has ammo
        if (currBullets > 0) {

            // wait for next shot
            if (shootTime <= 0) {

                Vector3 bulletLandPoint;

                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit lookatPoint)) {
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
            Debug.Log("Not enough ammo ammo");
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
    }
}