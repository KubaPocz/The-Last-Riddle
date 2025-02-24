using UnityEngine;

public class SoupController : MonoBehaviour
{
    private Camera playerCamera;
    public GameManager gameManager;
    private Animator notificationsAnimator;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
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
                    notificationsAnimator.SetBool("Powiadomienia", true);
                    gameManager.soundController.PickUp();
                    StartCoroutine(gameManager.HideNotifications());
                    gameManager.playerInventory.Add(gameManager.przepis.Name, 1);
                    gameObject.SetActive(false);
                }
            }

        }
    }
}
