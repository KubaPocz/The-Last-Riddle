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
        string instrictionsFilePath = Path.Combine(Application.streamingAssetsPath, "Texts", "Scrolls", "Notes", "Instructions.txt");
        instructions = File.ReadAllText(instrictionsFilePath);
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
                    gameManager.ShowBeginningScroll(scroll, scrollText, instructions);
                    gameManager.codeText.text = "Szyfr: _ _ _";
                }
                if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled)
                {
                    StartCoroutine(gameManager.HideBeginningScroll(scroll));
                }
            }

        }

    }
}
