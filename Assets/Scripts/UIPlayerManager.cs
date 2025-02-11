using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static OutlineControler;
using static UnityEngine.GraphicsBuffer;

public class UIPlayerManager : MonoBehaviour
{
    private Camera playerCamera;
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
        dialogGraczText = File.ReadAllLines(@"Library\Dialogi\dialogGracz.txt");
        dialogCharacterText = File.ReadAllLines(@"Library\Dialogi\dialogCharacter.txt");
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
                        ingredientName = hit.collider.gameObject.GetComponent<IngredientController>().ingredientName;
                        interactionText.text = $"WeŸ {ingredientName}\n(E)";
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
                        interactionText.text = "Gotuj (E)";
                        break;
                    case interaction.czytaj:
                        interactionText.text = "Czytaj (E)";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            firstPersonController.enabled = false;
                            playeranimator.enabled = false;
                            gameManager.ShowBook(gameManager.przepisyImage[hit.collider.gameObject.GetComponent<BookController>().page1index], gameManager.przepisyImage[hit.collider.gameObject.GetComponent<BookController>().page2index]);
                        }
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
            if (Input.GetKeyDown(KeyCode.Space) && dialogCoroutine== null)
            {
                dialogCoroutine = StartCoroutine(WriteDialog());
            }
        }
        IEnumerator WriteDialog()
        {
            if (dialogGraczIndex < dialogGraczText.Length)
            {
                if (dialogGraczIndex <= dialogCharacterIndex)
                {
                    dialogGracz.text = "-";
                    for (int i = 0; i < dialogGraczText[dialogGraczIndex].Length; i++)
                    {
                        dialogGracz.text += dialogGraczText[dialogGraczIndex][i];
                    }
                    dialogCharacter.text = "";
                    dialogGraczIndex++;
                }
                else
                {
                    dialogCharacter.text = "-";
                    for (int i = 0; i < dialogCharacterText[dialogCharacterIndex].Length; i++)
                    {
                        dialogCharacter.text += dialogCharacterText[dialogCharacterIndex][i];
                        yield return new WaitForSeconds(0.03f);
                    }
                    if (dialogCharacterIndex == 5)
                        dialogCharacter.text += gameManager.przepis;
                    dialogCharacterIndex++;
                }
            }
            dialogCoroutine = null;
        }
    }
}
