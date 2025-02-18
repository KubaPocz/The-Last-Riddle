using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance=10f;
    [NonSerialized]public bool canOpen;
    [NonSerialized] public Animator animator;
    private GameManager gameManager;
    private void Awake()
    {
        canOpen = true;
    }
    void Start()
    {
        playerCamera = Camera.main;
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
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
                        animator.SetTrigger("doorSwitch");
                    }
                    else
                    {
                        gameManager.error.text = "Drzwi zamkniête na k³ódkê";
                        gameManager.ClearError();
                    }
                }
            }
        }
    }
}
