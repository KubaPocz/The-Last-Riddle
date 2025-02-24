using System;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public GameManager gameManager;
    private Camera playerCamera;
    [NonSerialized] public bool knowRecipe = false;
    private void Start()
    {
        playerCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
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
                    foreach(var skladnik in gameManager.przepis.Ingredients)
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
                    if (canCook && gameManager.knownRecipe)
                    {
                        gameManager.Cauldron_water.gameObject.SetActive(false);
                        gameManager.Cauldron_soup.gameObject.SetActive(true);
                        foreach (var skladnik in gameManager.przepis.Ingredients)
                        {
                            gameManager.playerInventory[skladnik.Key] -= skladnik.Value;
                        }
                    }
                    else if (!canCook && gameManager.knownRecipe)
                    {
                        gameManager.error.text = LocalizationManager.Instance.GetText("NoIngredients");
                        StartCoroutine(gameManager.ClearError());
                    }
                    else
                    {
                        gameManager.error.text = LocalizationManager.Instance.GetText("NoRecipe");
                        StartCoroutine(gameManager.ClearError());
                    }
                }
            }

        }
    }
}
