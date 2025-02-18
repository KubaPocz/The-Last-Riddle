using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, int> playerInventory = new Dictionary<string, int>();
    public TextMeshProUGUI textInventory;
    public Animator inventoryAnimator;
    public FirstPersonController firstPersonController;
    public new ParticleSystem particleSystem;
    public GameObject menu;
    public GameObject opcje;
    public GameObject book;
    public RawImage bookPage1;
    public RawImage bookPage2;
    public bool bookOn = false;
    public bool scrollOn = false;
    public bool codeWindowOn = false;
    public bool ending = false;
    public Slider glosnosc;
    public Slider jasnosc;
    public Slider fov;
    public UIPlayerManager uIPlayerManager;
    public List<Przepis> przepisy;
    public List<Texture> przepisyImage;
    [NonSerialized] public Przepis przepis;
    public GameObject Cauldron_water;
    public GameObject Cauldron_soup;
    public TextMeshProUGUI error;
    [NonSerialized] public Coroutine coroutineError;
    [NonSerialized] public bool knownRecipe = false;
    public string[] code;
    public GameObject staminaBar;
    public GameObject endingPanel;
    public GameObject endingPanelSliding;
    public GameObject endingPanelBackground;



    public class Przepis
    {
        public string Nazwa { get; set; }
        public Dictionary<string, int> Skladniki { get; set; }
    }
    void Awake()
    {
        Shuffle(przepisyImage);
        menu.SetActive(false);
        opcje.SetActive(false);
        book.SetActive(false);
        endingPanel.SetActive(false);
        SaveOptions();
        uIPlayerManager.dialog.SetActive(false);
        Cauldron_water.SetActive(false);
        Cauldron_soup.SetActive(false);
        string json = File.ReadAllText(@"Library\Przepisy.txt");
        przepisy = JsonConvert.DeserializeObject<List<Przepis>>(json);
        //przepis = przepisy[UnityEngine.Random.Range(0,przepisy.Count)];
        przepis = przepisy[8];
        playerInventory.Add(przepis.Nazwa, 1);
        string[] codes = File.ReadAllLines(@"Assets\Texts\Codes.txt");
        code = codes[UnityEngine.Random.Range(0, codes.Length)].Split(" ");
        code = new string[1] { "a" };
        //Debug.Log($"{code[0]} {code[1]} {code[2]}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
                    menu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else if (Time.timeScale == 1f && !menu.activeInHierarchy && !uIPlayerManager.setTarget && !bookOn && !scrollOn && !codeWindowOn)
                {
                    Time.timeScale = 0f;
                    firstPersonController.enabled = false;
                    particleSystem.Pause();
                    menu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                inventoryAnimator.SetBool("Inventory", false);
                if (book.activeInHierarchy)
                {
                    HideBook();
                    firstPersonController.enabled = true;
                    uIPlayerManager.playeranimator.enabled = true;
                }
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
                    opcje.SetActive(false);
                    menu.SetActive(true);
                }
            }
            else
            {
                menu.SetActive(false);
                SceneManager.LoadSceneAsync(0);
            }
        }
        
    }
    public void UpdateInventory()
    {
        string eqText = "";
        foreach ( KeyValuePair<string, int> kvp in playerInventory)
        {
            if (playerInventory[(kvp.Key)] == 0)
            {
                playerInventory.Remove(kvp.Key);
            }
            eqText += $"{kvp.Key} - {kvp.Value}\n";
        }
        textInventory.text = eqText;
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
    }
    public void ShowScroll(GameObject scroll, TextMeshProUGUI scrollText, string text)
    {
        Debug.Log("Aasdas");
        scrollOn = true;
        scrollText.text = text;
        scroll.gameObject.SetActive(true);
        firstPersonController.enabled = false;
    }
    public IEnumerator HideScroll(GameObject scroll)
    {
        scroll.gameObject.SetActive(false);
        firstPersonController.enabled = true;
        yield return new WaitForSeconds(0.1f);
        scrollOn = false;
    }
    public void ShowEndCodeWindow(GameObject window)
    {
        codeWindowOn = true;
        window.SetActive(true);
        firstPersonController.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    public IEnumerator HideEndCodeWindow(GameObject window)
    {
        window.SetActive(false);
        firstPersonController.enabled = true;
        yield return new WaitForSeconds(0.1f);
        codeWindowOn = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void SaveOptions()
    {
        firstPersonController.fov = fov.value+30;
        RenderSettings.ambientLight = new Color(jasnosc.value, jasnosc.value, jasnosc.value)*2;
    }
    public void Exit()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void ShowBook(Texture page1, Texture page2)
    {
        bookPage1.texture = page1;
        bookPage2.texture = page2;
        book.SetActive(true);
        bookOn = true;
    }
    public IEnumerator HideBook()
    {
        book.gameObject.SetActive(false);
        firstPersonController.enabled = true;
        yield return new WaitForSeconds(0.1f);
        bookOn = false;
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
        yield return new WaitForSeconds(2f);
        error.text = "";
    }
    public void Ending()
    {
        ending = true;
        firstPersonController.enabled = false;
        endingPanel.SetActive(true);

        Animator anim1 = endingPanelBackground.GetComponent<Animator>();
        Animator anim2 = endingPanel.GetComponent<Animator>();
        Animator anim3 = endingPanelSliding.GetComponent<Animator>();

        anim1.SetTrigger("Ending");
        anim2.SetTrigger("Ending");
        anim3.SetTrigger("Ending");

    }

}
