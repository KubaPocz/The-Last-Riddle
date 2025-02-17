using TMPro;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public TextAsset scrollTextFile;
    private Camera playerCamera;
    public GameManager gameManager;
    public GameObject scroll;
    public TextMeshProUGUI scrollTextTMP;
    public GameObject elderScroll;

    private void Start()
    {
        playerCamera = Camera.main;
        scroll.gameObject.SetActive(false);
        elderScroll.SetActive(false);
    }
    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E) && gameManager.firstPersonController.enabled)
                {
                    gameManager.ShowScroll(scroll, scrollTextTMP,scrollTextFile.text);
                    elderScroll.SetActive(true);
                }
                if(Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled)
                {
                    StartCoroutine(gameManager.HideScroll(scroll));
                }
            }

        }
        
    }
}
