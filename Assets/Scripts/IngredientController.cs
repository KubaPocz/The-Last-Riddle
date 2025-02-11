using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientController : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance = 5f;
    public GameManager gameManager;
    public string ingredientName;
    public bool znika;
    public TextMeshProUGUI ekwipunekPowiadomenia;
    private Coroutine coroutineNotifications;
    public Animator notificationsAnimator;

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
                    if (gameManager.playerInventory.ContainsKey(ingredientName))
                    {
                        if (gameManager.playerInventory[ingredientName] >= 5)
                        {
                            if(gameManager.coroutineError != null)
                                StopCoroutine(gameManager.coroutineError);
                            gameManager.error.text = $"Nie mo¿esz wzi¹œæ wiêcej {ingredientName}";
                            gameManager.coroutineError = StartCoroutine(gameManager.ClearError());
                        }
                        else
                        {
                            if (coroutineNotifications != null)
                                StopCoroutine(coroutineNotifications);
                            notificationsAnimator.SetBool("Powiadomienia", true);
                            coroutineNotifications = StartCoroutine(HideNotifications());
                            gameManager.playerInventory[ingredientName] += 1;
                            ekwipunekPowiadomenia.text += $"+1 {ingredientName}\n";
                        }
                    }
                    else
                    {
                        if (coroutineNotifications != null)
                            StopCoroutine(coroutineNotifications);
                        notificationsAnimator.SetBool("Powiadomienia", true);
                        coroutineNotifications = StartCoroutine(HideNotifications());
                        gameManager.playerInventory.Add(ingredientName, 1);
                        ekwipunekPowiadomenia.text += $"+1 {ingredientName}\n";
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
    
    IEnumerator HideNotifications()
    {
        yield return new WaitForSeconds(3f);
        notificationsAnimator.SetBool("Powiadomienia", false);
        yield return new WaitForSeconds(1f);
        ekwipunekPowiadomenia.text = "";
        coroutineNotifications = null;
    }
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
