using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance=10f;
    [NonSerialized]public bool canOpen = true;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        animator = GetComponent<Animator>();
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
                    if (canOpen)
                    {
                        Debug.Log("Drzwi otwarte");
                        animator.SetTrigger("doorSwitch");
                    }
                    else
                    {
                        Debug.Log("Drzwi zablokowane");
                    }
                }
            }
        }
    }
}
