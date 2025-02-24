using System.Collections;
using UnityEngine;
using TMPro;

public class IngredientController : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance = 3f;
    public GameManager gameManager;
    public string ingredientName;
    public bool znika;
    public TextMeshProUGUI inventoryNotifications;
    public Animator notificationsAnimator;
    private Coroutine coroutineNotifications;

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
                    if (gameManager.playerInventory.ContainsKey(LocalizationManager.Instance.GetText(ingredientName)))
                    {
                        if (gameManager.playerInventory[LocalizationManager.Instance.GetText(ingredientName)] >= 5)
                        {
                            if(gameManager.coroutineError != null)
                                StopCoroutine(gameManager.coroutineError);
                            gameManager.error.text = $"{LocalizationManager.Instance.GetText("NoMore")} {LocalizationManager.Instance.GetText(ingredientName)}";
                            gameManager.coroutineError = StartCoroutine(gameManager.ClearError());
                        }
                        else
                        {
                            if (coroutineNotifications != null)
                                StopCoroutine(coroutineNotifications);
                            notificationsAnimator.SetBool("Powiadomienia", true);
                            gameManager.soundController.PickUp();
                            coroutineNotifications = StartCoroutine(gameManager.HideNotifications(coroutineNotifications));
                            gameManager.playerInventory[LocalizationManager.Instance.GetText(ingredientName)] += 1;
                            inventoryNotifications.text += $"+1 {LocalizationManager.Instance.GetText(ingredientName)}\n";
                        }
                    }
                    else
                    {
                        if (coroutineNotifications != null)
                            StopCoroutine(coroutineNotifications);
                        notificationsAnimator.SetBool("Powiadomienia", true);
                        gameManager.soundController.PickUp();
                        coroutineNotifications = StartCoroutine(gameManager.HideNotifications(coroutineNotifications));
                        gameManager.playerInventory.Add(LocalizationManager.Instance.GetText(ingredientName), 1);
                        inventoryNotifications.text += $"+1 {LocalizationManager.Instance.GetText(ingredientName)}\n";
                    }
                    if (znika)
                    {
                        gameObject.transform.position = Vector3.zero;
                        DestroyObject();
                    }
                    gameManager.UpdateInventory();
                }
            }
        }
    }
    
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
