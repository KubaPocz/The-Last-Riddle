using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public enum Language
    {
        Polish,
        English       
    }

    [NonSerialized]public Language CurrentLanguage;
    private Dictionary<string, string> localizationDictionary = new Dictionary<string, string>();

    private void Awake()
    {
        Enum.TryParse<Language>(PlayerPrefs.GetString("Language"), out CurrentLanguage);
        if (Instance == null)
        {
            Instance = this;
            LoadLocalizationData();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadLocalizationData()
    {
        string fileName;
        string folderName;
        switch (CurrentLanguage)
        {
            case Language.Polish:
                fileName = "pl";
                folderName = "Polish";
                break;
            case Language.English:
                fileName = "en";
                folderName = "English";
                break;
            default:
                Debug.LogWarning("Selected language is not supported. Defaulting to English.");
                fileName = "en";
                folderName = "English";
                break;
        }

        TextAsset textAsset = Resources.Load<TextAsset>($"Language/{folderName}/{fileName}");

        if (textAsset == null)
        {
            Debug.LogError($"Localization file {fileName}.txt not found in Resources/Language/");
            return;
        }

        localizationDictionary.Clear();
        string[] lines = textAsset.text.Split('\n');
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line) && line.Contains("="))
            {
                string[] keyValue = line.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    localizationDictionary[key] = value;
                }
            }
        }

        Debug.Log($"Loaded {localizationDictionary.Count} localization entries for {folderName}/{fileName}");
    }

    public string GetText(string key)
    {
        if (localizationDictionary.TryGetValue(key, out string value))
        {
            return value;
        }
        return $"[MISSING:{key}]";
    }
    public string GetKey(string value)
    {
        if(localizationDictionary.TryGetValue(value, out string key))
        {
            return key;
        }
        return $"[MISSING:{value}]";
    }

    public void SetLanguage(Language language)
    {
        CurrentLanguage = language;
        LoadLocalizationData();
    }
}
