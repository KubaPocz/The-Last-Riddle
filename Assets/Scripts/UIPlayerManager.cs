using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using static OutlineControler;

public class UIPlayerManager : MonoBehaviour
{
    public Camera playerCamera;
    private float maxDistance = 3f;
    private OutlineControler outlineControler;
    public TextMeshProUGUI interactionText;
    private string ingredientName;
    public GameObject characterHead;
    private FirstPersonController firstPersonController;
    public bool setTarget = false;
    public GameObject dialog;
    public TextMeshProUGUI dialogGracz;
    private string[] dialogGraczText;
    public TextMeshProUGUI dialogCharacter;
    private string[] dialogCharacterText;
    private int dialogGraczIndex = 0;
    private int dialogCharacterIndex = 0;
    private Coroutine dialogCoroutine;
    public Animator playeranimator;
    public GameManager gameManager;


    void Start()
    {
        playerCamera = Camera.main;
        firstPersonController = GetComponent<FirstPersonController>();
        string dialogPlayerFilePath = Path.Combine(Application.streamingAssetsPath, "Texts", "Dialogi", "Player.txt");
        dialogGraczText = File.ReadAllLines(dialogPlayerFilePath);
        string dialogCharacterFilePath = Path.Combine(Application.streamingAssetsPath, "Texts", "Dialogi", "Character.txt");
        dialogCharacterText = File.ReadAllLines(dialogCharacterFilePath);
        dialog.SetActive(false);
    }
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Interactable")
            {
                outlineControler = hit.collider.gameObject.GetComponent<OutlineControler>();
                switch (outlineControler.Interakcja)
                {
                    case interaction.wez:
                        try
                        {
                            ingredientName = hit.collider.gameObject.GetComponent<IngredientController>().ingredientName;
                        }
                        catch { }
                        interactionText.text = $"WeŸ {ingredientName ?? "Zupe"}\n(E)";
                        ingredientName = null;
                        break;
                    case interaction.daj:
                        interactionText.text = $"Daj Zupê {gameManager.przepis}(E)";
                        break;
                    case interaction.porozmawiaj:
                        interactionText.text = "Porozmawiaj (E)";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            firstPersonController.enabled = false;
                            setTarget = true;
                            playeranimator.enabled = false;
                            dialog.SetActive(true);
                        }
                        break;
                    case interaction.otworz:
                        interactionText.text = "Otworz (E)";
                        break;
                    case interaction.gotuj:
                        if (gameManager.knownRecipe)
                            interactionText.text = $"Gotuj {gameManager.przepis.Nazwa} (E)";
                        else
                            interactionText.text = $"Gotuj ??? (E)";
                        break;
                    case interaction.czytaj:
                        interactionText.text = "Czytaj (E)";
                        break;
                    case interaction.uzyj:
                        interactionText.text = "U¿yj (E)";
                        break;
                }
            }
            else
                interactionText.text = "";
        }
        if (setTarget)
        {
            Vector3 direction = characterHead.transform.position - playerCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Space) && dialogCoroutine == null)
            {
                dialogCoroutine = StartCoroutine(WriteDialog());
            }
        }
        IEnumerator WriteDialog()
        {
            if (dialogGraczIndex < dialogGraczText.Length || dialogCharacterIndex < dialogCharacterText.Length)
            {
                if (dialogGraczIndex <= dialogCharacterIndex && dialogGraczIndex < dialogGraczText.Length)
                {
                    if (dialogCharacterIndex == 8)
                    {
                        if (!gameManager.playerInventory.ContainsKey(gameManager.przepis.Nazwa))
                            yield break;
                        else
                            gameManager.playerInventory.Remove(gameManager.przepis.Nazwa);
                    }

                    dialogGracz.text = "-";
                    for (int i = 0; i < dialogGraczText[dialogGraczIndex].Length; i++)
                    {
                        dialogGracz.text += dialogGraczText[dialogGraczIndex][i];
                    }
                    dialogCharacter.text = "";
                    dialogGraczIndex++;
                }
                else if (dialogCharacterIndex < dialogCharacterText.Length)
                {
                    dialogCharacter.text = "-";
                    for (int i = 0; i < dialogCharacterText[dialogCharacterIndex].Length; i++)
                    {
                        dialogCharacter.text += dialogCharacterText[dialogCharacterIndex][i];
                        yield return new WaitForSeconds(0.02f);
                    }
                    if (dialogCharacterIndex == 5)
                    {
                        dialogCharacter.text += $" {gameManager.przepis.Nazwa}";
                        gameManager.knownRecipe = true;
                    }
                    if (dialogCharacterIndex == 9)
                    {
                        dialogCharacter.text += gameManager.code[1].ToUpper();
                        gameManager.UpdateCodeText(1);
                    }
                    dialogCharacterIndex++;
                }
            }
            dialogCoroutine = null;
        }
    }
}
