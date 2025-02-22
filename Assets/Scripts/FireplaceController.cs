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
                    if (gameManager.playerInventory.ContainsKey(LocalizationManager.Instance.GetText("Cauldron")))
                    {
                        GetComponent<GlowEffect>().StopGlowing();
                        this.tag = "Untagged";
                        gameManager.Cauldron_water.SetActive(true);
                        gameManager.playerInventory.Remove(LocalizationManager.Instance.GetText("Cauldron"));
                    }
                    else
                    {
                        if (gameManager.coroutineError != null)
                            StopCoroutine(gameManager.coroutineError);
                        gameManager.error.text = LocalizationManager.Instance.GetText("NoCauldron");
                        gameManager.coroutineError = StartCoroutine(gameManager.ClearError());
                    }
                }
            }

        }
    }
}
