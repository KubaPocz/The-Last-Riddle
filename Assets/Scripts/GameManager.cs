using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, int> playerInventory = new Dictionary<string, int>();
    [Header("Inventory")]
    public TextMeshProUGUI inventoryName;
    public TextMeshProUGUI inventoryContent;
    public Animator inventoryAnimator;
    public FirstPersonController firstPersonController;
    public new ParticleSystem particleSystem;
    [Header("Menu")]
    public GameObject menu;
    public TextMeshProUGUI menuText;
    public TextMeshProUGUI optionsText;
    public TextMeshProUGUI exitText;
    public TextMeshProUGUI infoText;
    [Header("Options")]
    public GameObject opcje;
    public TextMeshProUGUI optionsMainText;
    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI brightnessText;
    public TextMeshProUGUI saveText;
    public TextMeshProUGUI exitOptionsText;
    public GameObject book;
    [Header ("Book-Page1")]
    public TextMeshProUGUI name1;
    public TextMeshProUGUI description1;
    public TextMeshProUGUI ingredients1;
    [Header("Book-Page2")]
    public TextMeshProUGUI name2;
    public TextMeshProUGUI description2;
    public TextMeshProUGUI ingredients2;
    [Header("EndCodeWindow")]
    public TextMeshProUGUI enterCodeText;
    public TextMeshProUGUI sumbitText;
    public TextMeshProUGUI placeholderText;
    public bool bookOn = false;
    public bool scrollOn = false;
    public bool codeWindowOn = false;
    public bool ending = false;
    public Slider volume;
    public Slider brightness;
    public Slider fov;
    public UIPlayerManager uIPlayerManager;
    public List<Przepis> przepisy;
    public GameObject Cauldron_water;
    public GameObject Cauldron_soup;
    public TextMeshProUGUI error;
    public TextMeshProUGUI meDialogText;
    public TextMeshProUGUI ulricDialogText;
    public GameObject staminaBar;
    public GameObject endingPanel;
    public GameObject endingPanelSliding;
    public GameObject endingPanelBackground;
    public TextMeshProUGUI codeText;
    public TextMeshProUGUI taskText;

    [NonSerialized] public List<string> codes = new List<string>();
    [NonSerialized] public string[] code;
    [NonSerialized] public Przepis przepis;
    [NonSerialized] public Coroutine coroutineError;
    [NonSerialized] public bool knownRecipe = false;
    [NonSerialized] public List<int> knownCodeID = new List<int>();
    [NonSerialized] public GameSoundController soundController;

    public class Przepis
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, int> Ingredients { get; set; }
    }
    private void Awake()
    {
        soundController = FindAnyObjectByType<GameSoundController>();
    }
    void Start()
    {
        UpdateMenuLanguage();
        menu.SetActive(false);
        opcje.SetActive(false);
        book.SetActive(false);
        endingPanel.SetActive(false);
        SaveOptions();
        uIPlayerManager.dialog.SetActive(false);
        Cauldron_water.SetActive(false);
        Cauldron_soup.SetActive(false);
        TextAsset recipesFilePath = Resources.Load<TextAsset>($"Language/{LocalizationManager.Instance.CurrentLanguage.ToString()}/Texts/Recipes");
        string json = recipesFilePath.text;
        przepisy = JsonConvert.DeserializeObject<List<Przepis>>(json);
        przepis = przepisy[UnityEngine.Random.Range(0,przepisy.Count)];
        TextAsset codesFilePath = Resources.Load<TextAsset>($"Language/{LocalizationManager.Instance.CurrentLanguage.ToString()}/Texts/Codes");
        string[] codesText = codesFilePath.text.Split("\n");
        foreach (string codeLine in codesText)
        {
            foreach(string codePart in codeLine.Split(" "))
            {
                codes.Add(codePart);
            }
        }
        Shuffle(codes);
        code = new string[] { codes[0], codes[1], codes[2] };
        codeText.text = ""; 
        taskText.text = LocalizationManager.Instance.GetText("FindInstructions");
        meDialogText.text = LocalizationManager.Instance.GetText("Me");
        ulricDialogText.text = LocalizationManager.Instance.GetText("Ulric");
        enterCodeText.text = LocalizationManager.Instance.GetText("EnterCode");
        sumbitText.text = LocalizationManager.Instance.GetText("Submit");
        placeholderText.text = LocalizationManager.Instance.GetText("Placeholder");
        volume.value = PlayerPrefs.GetFloat("Volume");
        brightness.value = PlayerPrefs.GetFloat("Brightness");
        soundController.UpdateVolume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !uIPlayerManager.dialog.activeInHierarchy)
        {
            UpdateInventory();
            inventoryAnimator.SetBool("Inventory", !inventoryAnimator.GetBool("Inventory"));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!ending)
            {
                if (Time.timeScale == 0f && menu.activeInHierarchy)
                {
                    Time.timeScale = 1f;
                    firstPersonController.enabled = true;
                    particleSystem.Play();
                    soundController.music.Play();
                    menu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else if (Time.timeScale == 1f && !menu.activeInHierarchy && !uIPlayerManager.setTarget && !bookOn && !scrollOn && !codeWindowOn)
                {
                    Time.timeScale = 0f;
                    firstPersonController.enabled = false;
                    particleSystem.Pause();
                    UpdateMenuLanguage();
                    menu.SetActive(true);
                    soundController.music.Pause();
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                inventoryAnimator.SetBool("Inventory", false);
                if (uIPlayerManager.setTarget == true)
                {
                    uIPlayerManager.setTarget = false;
                    firstPersonController.enabled = true;
                    uIPlayerManager.dialog.SetActive(false);
                    uIPlayerManager.playeranimator.enabled = true;
                }
                uIPlayerManager.setTarget = false;
                if (opcje.activeInHierarchy)
                {
                    UpdateMenuLanguage();
                    opcje.SetActive(false);
                    menu.SetActive(true);
                    soundController.music.Pause();
                }
            }
            else
            {
                soundController.music.Play();
                menu.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadSceneAsync(0);
            }
        }
    }
    public void UpdateInventory()
    {
        string eqText = "";
        inventoryName.text = LocalizationManager.Instance.GetText("Inventory");

        List<string> keysToRemove = new List<string>();

        foreach (var kvp in playerInventory)
        {
            if (kvp.Value == 0)
            {
                keysToRemove.Add(kvp.Key);
            }
            else
            {
                eqText += $"{kvp.Key} - {kvp.Value}\n";
            }
        }
        foreach (var key in keysToRemove)
        {
            playerInventory.Remove(key);
        }

        inventoryContent.text = eqText;
    }

    public void UpdateMenuLanguage()
    {
        optionsText.text = LocalizationManager.Instance.GetText("Options");
        exitText.text = LocalizationManager.Instance.GetText("Exit");
        infoText.text = LocalizationManager.Instance.GetText("Info");
        optionsMainText.text = LocalizationManager.Instance.GetText("Options");
        volumeText.text = LocalizationManager.Instance.GetText("Volume");
        brightnessText.text = LocalizationManager.Instance.GetText("Brightness");
        saveText.text = LocalizationManager.Instance.GetText("Save");
        exitOptionsText.text = LocalizationManager.Instance.GetText("Exit");
}
    public void ShowOptions()
    {
        menu.SetActive(false);
        opcje.SetActive(true);
    }
    public void HideOptions()
    {
        opcje.SetActive(false);
        menu.SetActive(true);
        soundController.UpdateVolume();
    }
    public void ShowScroll(GameObject scroll, TextMeshProUGUI scrollText, string text)
    {
        soundController.OpenScroll();
        scrollOn = true;
        scrollText.text = text;
        scroll.gameObject.SetActive(true);
        firstPersonController.enabled = false;
        Time.timeScale = 0f;
    }
    public IEnumerator HideScroll(GameObject scroll)
    {
        scroll.gameObject.SetActive(false);
        firstPersonController.enabled = true;
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.1f);
        scrollOn = false;
    }
    public void ShowBeginningScroll(GameObject scroll, TextMeshProUGUI scrollText, string text)
    {
        soundController.OpenScroll();
        scrollOn = true;
        scrollText.text = text;
        scroll.gameObject.SetActive(true);
        firstPersonController.enabled = false;
        Time.timeScale = 0f;
    }
    public IEnumerator HideBeginningScroll(GameObject scroll)
    {
        scroll.gameObject.SetActive(false);
        firstPersonController.enabled = true;
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.1f);
        scrollOn = false;
    }
    public void ShowScrollElder(GameObject scroll, TextMeshProUGUI scrollText, string text)
    {
        soundController.OpenScroll();
        scrollOn = true;
        scrollText.text = text;
        scroll.gameObject.SetActive(true);
        firstPersonController.enabled = false;
        Time.timeScale = 0f;
    }
    public IEnumerator HideScrollElder(GameObject scroll)
    {
        scroll.gameObject.SetActive(false);
        firstPersonController.enabled = true;
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.1f);
        scrollOn = false;
    }
    public void ShowEndCodeWindow(GameObject window)
    {
        codeWindowOn = true;
        window.SetActive(true);
        firstPersonController.enabled = false;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    public IEnumerator HideEndCodeWindow(GameObject window)
    {
        window.SetActive(false);
        firstPersonController.enabled = true;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.1f);
        codeWindowOn = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void SaveOptions()
    {
        firstPersonController.fov = fov.value+30;
        RenderSettings.ambientLight = Color.black + new Color(brightness.value, brightness.value, brightness.value) * 0.3f;
        PlayerPrefs.SetFloat("Volume", volume.value);
        soundController.UpdateVolume();
    }
    public void Exit()
    {
        menu.SetActive(false);
        Destroy(this);
        SceneManager.LoadSceneAsync(0);
    }
    public void ShowBook(int page1, int page2)
    {
        soundController.OpenBook();
        name1.text = przepisy[page1].Name;
        description1.text = przepisy[page1].Description;
        ingredients1.text = "";
        foreach (var ingredient in przepisy[page1].Ingredients)
        {
            ingredients1.text += $"{ingredient.Key} - {ingredient.Value}\n";
        }
        name2.text = przepisy[page2].Name;
        description2.text = przepisy[page2].Description;
        ingredients2.text = "";
        foreach (var ingredient in przepisy[page2].Ingredients)
        {
            ingredients2.text += $"{ingredient.Key} - {ingredient.Value}\n";
        }
        book.SetActive(true);
        bookOn = true;
        firstPersonController.enabled = false;
        uIPlayerManager.playeranimator.enabled = false;
        Time.timeScale = 0f;
    }
    public IEnumerator HideBook()
    {
        book.gameObject.SetActive(false);
        firstPersonController.enabled = true;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.1f);
        bookOn = false;
        firstPersonController.enabled = true;
        uIPlayerManager.playeranimator.enabled = true;
    }
    public void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public IEnumerator ClearError()
    {
        yield return new WaitForSeconds(1.5f);
        error.text = "";
    }
    public void Ending()
    {
        ending = true;
        firstPersonController.enabled = false;
        endingPanel.SetActive(true);
        taskText.text = string.Empty;
        codeText.text = string.Empty;
        soundController.music.loop = false;
        Animator anim1 = endingPanelBackground.GetComponent<Animator>();
        Animator anim2 = endingPanel.GetComponent<Animator>();
        Animator anim3 = endingPanelSliding.GetComponent<Animator>();

        anim1.SetTrigger("Ending");
        anim2.SetTrigger("Ending");
        anim3.SetTrigger("Ending");
    }
    public void UpdateCodeText(int codeID)
    {
        knownCodeID.Add(codeID);
        string text = $"{LocalizationManager.Instance.GetText("Cipher")}: ";
        for (int i = 0;i<3;i++)
        {
            if (knownCodeID.Contains(i))
            {
                text += code[i]+" ";
            }
            else
            {
                text += "_ ";
            }
        }
        codeText.text = text.Replace("\u200B", "");
    }
}
