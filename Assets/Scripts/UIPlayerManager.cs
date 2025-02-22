using System.Collections;
using System.IO;
using TMPro;
using Unity.VisualScripting;
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
    public TextMeshProUGUI dialogPlayer;
    private string[] dialogPlayerText;
    public TextMeshProUGUI dialogCharacter;
    private string[] dialogCharacterText;
    private int dialogPlayerIndex = 0;
    private int dialogCharacterIndex = 0;
    private Coroutine dialogCoroutine;
    public Animator playeranimator;
    private GameManager gameManager;


    void Start()
    {
        playerCamera = Camera.main;
        firstPersonController = GetComponent<FirstPersonController>();
        TextAsset dialogPlayerFilePath = Resources.Load<TextAsset>($"Language/{LocalizationManager.Instance.CurrentLanguage.ToString()}/Texts/Dialogues/Player");
        dialogPlayerText = dialogPlayerFilePath.text.Split("\n");
        TextAsset dialogCharacterFilePath = Resources.Load<TextAsset>($"Language/{LocalizationManager.Instance.CurrentLanguage.ToString()}/Texts/Dialogues/Character");
        dialogCharacterText = dialogCharacterFilePath.text.Split("\n");
        dialog.SetActive(false);
        gameManager = FindAnyObjectByType<GameManager>();

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
                        interactionText.text = $"{LocalizationManager.Instance.GetText("Take")} {LocalizationManager.Instance.GetText(ingredientName ?? "Soup")}\n(E)";
                        ingredientName = null;
                        break;
                    case interaction.daj:
                        interactionText.text = $"{LocalizationManager.Instance.GetText("GiveSoup")} {gameManager.przepis}(E)";
                        break;
                    case interaction.porozmawiaj:
                        interactionText.text = $"{LocalizationManager.Instance.GetText("Talk")} (E)";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            firstPersonController.enabled = false;
                            setTarget = true;
                            playeranimator.enabled = false;
                            dialog.SetActive(true);
                            gameManager.inventoryAnimator.SetBool("Inventory", false);
                        }
                        break;
                    case interaction.otworz:
                        interactionText.text = $"{LocalizationManager.Instance.GetText("Open")} (E)";
                        break;
                    case interaction.gotuj:
                        if (gameManager.knownRecipe)
                            interactionText.text = $"{LocalizationManager.Instance.GetText("Cook")} {gameManager.przepis.Name} (E)";
                        else
                            interactionText.text = $"{LocalizationManager.Instance.GetText("Cook")} ??? (E)";
                        break;
                    case interaction.czytaj:
                        interactionText.text = $"{LocalizationManager.Instance.GetText("Read")} (E)";
                        break;
                    case interaction.uzyj:
                        interactionText.text = $"{LocalizationManager.Instance.GetText("Use")} (E)";
                        break;
                    default:
                        interactionText.text = "";
                        break;
                }
            }
            else
                interactionText.text = "";

        }
        else
            interactionText.text = "";
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
            if (dialogPlayerIndex < dialogPlayerText.Length || dialogCharacterIndex < dialogCharacterText.Length)
            {
                if (dialogPlayerIndex <= dialogCharacterIndex && dialogPlayerIndex < dialogPlayerText.Length)
                {
                    if (dialogCharacterIndex == 8)
                    {
                        if (!gameManager.playerInventory.ContainsKey(gameManager.przepis.Name))
                            yield break;
                        else
                            gameManager.playerInventory.Remove(gameManager.przepis.Name);
                    }

                    dialogPlayer.text = "-";
                    for (int i = 0; i < dialogPlayerText[dialogPlayerIndex].Length; i++)
                    {
                        dialogPlayer.text += dialogPlayerText[dialogPlayerIndex][i];
                    }
                    dialogCharacter.text = "";
                    dialogPlayerIndex++;
                }
                else if (dialogCharacterIndex < dialogCharacterText.Length)
                {
                    dialogCharacter.text = "-";
                    for (int i = 0; i < dialogCharacterText[dialogCharacterIndex].Length; i++)
                    {
                        dialogCharacter.text += dialogCharacterText[dialogCharacterIndex][i].ToString();
                        yield return new WaitForSeconds(0.02f);
                    }
                    if (dialogCharacterIndex == 5)
                    {
                        dialogCharacter.text += $"\n{gameManager.przepis.Name}";
                        gameManager.knownRecipe = true;
                        gameManager.taskText.text = $"{LocalizationManager.Instance.GetText("MakeSoup")} {gameManager.przepis.Name}";
                    }
                    if (dialogCharacterIndex == 9)
                    {
                        dialogCharacter.text += $"\n{gameManager.code[1].ToUpper()}";
                        gameManager.UpdateCodeText(1);
                        gameManager.taskText.text = LocalizationManager.Instance.GetText("LearnCode");
                    }
                    dialogCharacterIndex++;
                }
            }
            dialogCoroutine = null;
        }
    }
}
