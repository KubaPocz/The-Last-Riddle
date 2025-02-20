using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    private TextAsset scrollTextFile;
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
        scrollTextFile = Resources.Load<TextAsset>($"Language/{LocalizationManager.Instance.CurrentLanguage.ToString()}/Texts/Hints/Hint{hintID}");
        text = scrollTextFile.text;
    }
    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gameManager.firstPersonController.enabled)
                    {
                        gameManager.ShowScroll(scroll, scrollTextTMP, text);
                        elderScroll.gameObject.SetActive(true);
                    }
                    else if (!gameManager.firstPersonController.enabled)
                    {
                        StartCoroutine(gameManager.HideScroll(scroll));
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled && gameManager.scrollOn)
                {
                    StartCoroutine(gameManager.HideScroll(scroll));
                }
            }

        }
        
    }
}
