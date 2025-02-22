using System;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    private Camera playerCamera;
    public float maxDistance = 10f;
    [NonSerialized]public bool haveKey = false;
    public DoorController doorController;
    private GameManager gameManager;
    void Start()
    {
        playerCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (haveKey)
                    {
                        doorController.canOpen = true;
                        gameManager.soundController.UnlockLock();
                        gameManager.playerInventory.Remove(LocalizationManager.Instance.GetText("DungeonKey"));
                        Destroy(gameObject);
                    }
                    else
                    {
                        gameManager.error.text = "Nie masz klucza!";
                        gameManager.ClearError();
                    }
                }
            }
        }
    }
}
