using UnityEngine;

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
                    gameManager.ShowBook(gameManager.przepisyImage[hit.collider.gameObject.GetComponent<BookController>().page1index], gameManager.przepisyImage[hit.collider.gameObject.GetComponent<BookController>().page2index]);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    StartCoroutine(gameManager.HideBook());
                }
            }
        }
    }
}
