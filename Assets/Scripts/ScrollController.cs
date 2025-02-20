using System.IO;
using TMPro;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    private string scrollTextFile;
    private Camera playerCamera;
    private string text;
    public int hintID;
    public GameManager gameManager;
    public GameObject scroll;
    public TextMeshProUGUI scrollTextTMP;
    public GameObject elderScroll;

    private void Start()
    {
        playerCamera = Camera.main;
        scroll.gameObject.SetActive(false);
        elderScroll.SetActive(false);
        scrollTextFile = Path.Combine(Application.streamingAssetsPath, "Texts","Hints", $"Hint{hintID}.txt");
        text = File.ReadAllText(scrollTextFile);
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
                    gameManager.ShowScroll(scroll, scrollTextTMP,text);
                    elderScroll.gameObject.SetActive(true);
                }
                if(Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled)
                {
                    StartCoroutine(gameManager.HideScroll(scroll));
                }
            }

        }
        
    }
}
