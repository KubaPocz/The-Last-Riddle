using UnityEngine;

public class FireplaceController : MonoBehaviour
{
    public GameManager gameManager;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gameManager.playerInventory.ContainsKey("Garnek"))
                    {
                        gameManager.Cauldron_water.SetActive(true);
                        gameManager.playerInventory.Remove("Cauldron");
                    }
                    else
                    {
                        if (gameManager.coroutineError != null)
                            StopCoroutine(gameManager.coroutineError);
                        gameManager.error.text = $"Nie masz garnka, najpierw go znajdü";
                        gameManager.coroutineError = StartCoroutine(gameManager.ClearError());
                    }
                }
            }

        }
    }
}
