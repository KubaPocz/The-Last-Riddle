using UnityEngine;

public class SoupController : MonoBehaviour
{
    private Camera playerCamera;
    public GameManager gameManager;
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
                    gameManager.playerInventory.Add(gameManager.przepis.Nazwa, 1);
                    gameObject.SetActive(false);
                }
            }

        }
    }
}
