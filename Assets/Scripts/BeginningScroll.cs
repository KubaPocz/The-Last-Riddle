using System.IO;
using TMPro;
using UnityEngine;

public class BeginningScroll : MonoBehaviour
{
    private Camera playerCamera;
    private GameManager gameManager;
    private string instructions;

    public GameObject scroll;
    public TextMeshProUGUI scrollText;
    private void Start()
    {
        playerCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
        TextAsset instrictionsFilePath = Resources.Load<TextAsset>($"Language/{LocalizationManager.Instance.CurrentLanguage.ToString()}/Texts/Scrolls/Notes/Instructions");
        instructions = instrictionsFilePath.text;
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
                        gameManager.ShowBeginningScroll(scroll, scrollText, instructions);
                        if (gameManager.codeText.text.Contains(LocalizationManager.Instance.GetText("FindInstructions")))
                        {
                            gameManager.codeText.text = $"{LocalizationManager.Instance.GetText("Cipher")}: _ _ _";
                        }
                    }
                    else if (!gameManager.firstPersonController.enabled)
                    {
                        StartCoroutine(gameManager.HideBeginningScroll(scroll));
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled && gameManager.scrollOn)
                {
                    StartCoroutine(gameManager.HideBeginningScroll(scroll));
                }

            }

        }
    }
}
