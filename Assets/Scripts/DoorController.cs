using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance=10f;
    [NonSerialized] public bool canOpen;
    [NonSerialized] public Animator animator;
    private GameManager gameManager;
    private bool opened = false;
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
                    if (!opened)
                    {
                        if (canOpen)
                        {
                            animator.SetTrigger("doorSwitch");
                            gameManager.soundController.OpenDoor();
                            opened = true;
                        }
                        else if (gameObject.name != "Door_Middle_Exit")
                        {
                            gameManager.error.text = LocalizationManager.Instance.GetText("DoorClosedInfo");
                            StartCoroutine(gameManager.ClearError());
                        }
                    }
                }
            }
        }
    }
}
