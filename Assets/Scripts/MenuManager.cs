using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public GameObject options;
    public Slider volume;
    public Slider brightness;
    public Slider fov;
    public AudioSource audioSource;
    public TextMeshProUGUI notification;
    public Canvas canvas;
    private Camera playerCamera;
    private Animator cameraAnimator;
    public TMP_Dropdown languageDropdown;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI optionsText;
    public TextMeshProUGUI exitText;
    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI brightnessText;
    public TextMeshProUGUI languageText;
    public TextMeshProUGUI saveText;
    public TextMeshProUGUI brightnessTipText;
    public enum Language
    {
        Polish,
        English
    }


    private void Start()
    {
        Time.timeScale = 1.0f;
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.15f);
        }
        if (!PlayerPrefs.HasKey("Brightness"))
        {
            PlayerPrefs.SetFloat("Brightness", 0.15f);
        }
        if (!PlayerPrefs.HasKey("Fov"))
        {
            PlayerPrefs.SetFloat("Fov", 0.4f);
        }
        volume.value = PlayerPrefs.GetFloat("Volume");
        brightness.value = PlayerPrefs.GetFloat("Brightness");
        fov.value = PlayerPrefs.GetFloat("Fov");
        options.SetActive(false);
        audioSource = FindAnyObjectByType<AudioSource>();
        canvas.gameObject.SetActive(false);
        StartCoroutine(ShowText());
        languageDropdown.options[0].text = Language.English.ToString();
        languageDropdown.options[1].text = Language.Polish.ToString();
        languageDropdown.value = (LocalizationManager.Instance.CurrentLanguage.ToString()=="Polish" ? 1 : 0);
        languageDropdown.RefreshShownValue();
        LoadLanguage();
    }
    private void Update()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
        RenderSettings.ambientLight = Color.black + new Color(brightness.value, brightness.value, brightness.value)*0.3f;
    }
    public void LoadLanguage()
    {
        startText.text = LocalizationManager.Instance.GetText("Play");
        optionsText.text = LocalizationManager.Instance.GetText("Options");
        exitText.text = LocalizationManager.Instance.GetText("Exit");
        volumeText.text = LocalizationManager.Instance.GetText("Volume");
        brightnessText.text = LocalizationManager.Instance.GetText("Brightness");
        languageText.text = LocalizationManager.Instance.GetText("Language");
        saveText.text = LocalizationManager.Instance.GetText("Save");
        brightnessTipText.text = LocalizationManager.Instance.GetText("BrightnessTip");
    }
    public void StartGame()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        PlayerPrefs.SetFloat("Brightness", brightness.value);
        PlayerPrefs.SetFloat("Fov", fov.value);
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenOptions()
    {
        options.SetActive(!options.activeInHierarchy);
    }
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        PlayerPrefs.SetFloat("Brightness", brightness.value);
        PlayerPrefs.SetFloat("Fov", fov.value);
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
        RenderSettings.ambientLight = Color.black + new Color(PlayerPrefs.GetFloat("Brightness"), PlayerPrefs.GetFloat("Brightness"), PlayerPrefs.GetFloat("Brightness"))*0.3f;
        PlayerPrefs.SetString("Language",languageDropdown.captionText.text);
        Enum.TryParse<LocalizationManager.Language>(languageDropdown.captionText.text, out LocalizationManager.Language lang);
        LocalizationManager.Instance.SetLanguage(lang);
        LoadLanguage();
        StartCoroutine(ShowNotification());
    }
    public IEnumerator ShowNotification()
    {
        notification.text = LocalizationManager.Instance.GetText("SaveInfo");
        yield return new WaitForSeconds(2f);
        notification.text = "";
    }
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(2f);
        canvas.gameObject.SetActive(true);
    }
}
