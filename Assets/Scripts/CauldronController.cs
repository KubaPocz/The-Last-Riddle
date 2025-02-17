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
                    bool canCook = true;
                    foreach(var skladnik in gameManager.przepis.Skladniki)
                    {
                        if(!gameManager.playerInventory.ContainsKey(skladnik.Key) || gameManager.playerInventory[skladnik.Key] < skladnik.Value)
                        {
                            canCook = false;
                            break;
                        }
                        else
                        {
                            canCook = true;
                        }
                    }
                    if (canCook)
                    {
                        gameManager.Cauldron_water.gameObject.SetActive(false);
                        gameManager.Cauldron_soup.gameObject.SetActive(true);
                        foreach (var skladnik in gameManager.przepis.Skladniki)
                        {
                            gameManager.playerInventory[skladnik.Key] -= skladnik.Value;
                        }
                    }
                    else
                    {
                        gameManager.error.text = "Nie masz wszystkich potrzebnych sk³adników";
                        StartCoroutine(gameManager.ClearError());
                    }
                }
            }

        }
    }
}
