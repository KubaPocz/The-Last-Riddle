using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;

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
        SaveOptions();
        uIPlayerManager.dialog.SetActive(false);
        Cauldron_water.SetActive(false);
        Cauldron_soup.SetActive(false);
        string json = File.ReadAllText(@"Library\Przepisy.txt");
        przepisy = JsonConvert.DeserializeObject<List<Przepis>>(json);
        //przepis = przepisy[UnityEngine.Random.Range(0,przepisy.Count)];
        przepis = przepisy[8];
        playerInventory.Add(przepis.Nazwa, 1);
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
            if(Time.timeScale == 0f && menu.activeInHierarchy && particleSystem.isPaused)
            {
                Time.timeScale = 1f;
                firstPersonController.enabled = true;
                particleSystem.Play();
                menu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(Time.timeScale == 1f && !menu.activeInHierarchy && !particleSystem.isPaused && !uIPlayerManager.setTarget && !bookOn)
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
    public void hideOptions()
    {
        opcje.SetActive(false);
        menu.SetActive(true);
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
    public void HideBook()
    {
        book.SetActive(false);
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
}
