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

    private void Start()
    {
        Time.timeScale = 1.0f;
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
        options.SetActive(false);
        audioSource = FindAnyObjectByType<AudioSource>();
        canvas.gameObject.SetActive(false);
        StartCoroutine(ShowText());
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
        options.SetActive(!options.activeInHierarchy);
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
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(2f);
        canvas.gameObject.SetActive(true);
    }
}
