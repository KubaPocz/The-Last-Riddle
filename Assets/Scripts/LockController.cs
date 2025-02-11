using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    private Camera playerCamera;
    public float maxDistance = 10f;
    [NonSerialized]public bool haveKey = false;
    public DoorController doorController;
    void Start()
    {
        playerCamera = Camera.main;
        doorController.canOpen = false;

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E) && haveKey)
                {
                    Debug.Log("Klódka otwarta");
                    doorController.canOpen = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
