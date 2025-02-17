using System;
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
                if (Input.GetKeyDown(KeyCode.E) && haveKey)
                {
                    doorController.canOpen = true;
                    Destroy(gameObject);
                }
                else
                {
                    gameManager.error.text = "Drzwi zamkniête na k³ódkê";
                    StartCoroutine(gameManager.ClearError());
                }
            }
        }
    }
}
