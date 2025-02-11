using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private Camera playerCamera;
    public float maxDistance = 10f;
    public LockController lockController;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {

            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Zdobyles klucz");
                    lockController.haveKey = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
