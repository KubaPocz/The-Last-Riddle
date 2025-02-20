using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    public int page1index;
    public int page2index;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Update()
    {
        Ray ray = new Ray(gameManager.uIPlayerManager.playerCamera.transform.position, gameManager.uIPlayerManager.playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gameManager.firstPersonController.enabled)
                    {
                        gameManager.ShowBook(page1index, page2index);
                    }
                    else if (!gameManager.firstPersonController.enabled)
                    {
                        StartCoroutine(gameManager.HideBook());
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled && gameManager.bookOn)
                {
                    StartCoroutine(gameManager.HideBook());
                }
            }
        }
    }
}
