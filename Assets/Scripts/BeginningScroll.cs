using System.IO;
using TMPro;
using UnityEngine;

public class BeginningScroll : MonoBehaviour
{
    private Camera playerCamera;
    private GameManager gameManager;

    public GameObject scroll;
    public TextMeshProUGUI scrollText;
    public string instructions;
    private void Start()
    {
        playerCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
        instructions = File.ReadAllText(@"Assets\Texts\Scrolls\Notes\Instructions.txt");
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
