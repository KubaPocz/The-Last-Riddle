using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject opcje;
    public Slider volume;
    public Slider brightness;
    public Slider fov;
    public AudioSource audioSource;
    public TextMeshProUGUI notification;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("Brightness"))
        {
            PlayerPrefs.SetFloat("Brightness", 0.3f);
        }
        if (!PlayerPrefs.HasKey("Fov"))
        {
            PlayerPrefs.SetFloat("Fov", 0.4f);
        }
        volume.value = PlayerPrefs.GetFloat("Volume");
        brightness.value = PlayerPrefs.GetFloat("Brightness");
        fov.value = PlayerPrefs.GetFloat("Fov");
        opcje.SetActive(false);
        audioSource = FindAnyObjectByType<AudioSource>();
    }
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenOptions()
    {
        opcje.SetActive(!opcje.activeInHierarchy);
    }
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        PlayerPrefs.SetFloat("Brightness", brightness.value);
        PlayerPrefs.SetFloat("Fov", fov.value);
        audioSource.volume = volume.value; 
        RenderSettings.ambientLight = Color.black + new Color(PlayerPrefs.GetFloat("Brightness"), PlayerPrefs.GetFloat("Brightness"), PlayerPrefs.GetFloat("Brightness"));

        StartCoroutine(ShowNotification());
    }
    public IEnumerator ShowNotification()
    {
        notification.text = "Options saved succesfully!";
        yield return new WaitForSeconds(2f);
        notification.text = "";
    }
}
