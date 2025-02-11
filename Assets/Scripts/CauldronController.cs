using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public GameManager gameManager;
    private Camera playerCamera;
    private void Start()
    {
        playerCamera = Camera.main;
    }
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gameManager.playerInventory.ContainsKey("Cauldron"))
                    {
                        gameManager.Cauldron_water.SetActive(true);
                    }
                }
            }

        }
    }
}
