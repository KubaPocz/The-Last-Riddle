using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    
    public AudioSource background;
    public AudioSource buttonHover;

    public AudioClip backroundSound;
    public AudioClip buttonHoverSound;
    void Start()
    {
        background.clip = backroundSound;
        background.loop = true;
        background.Play();
        buttonHover.clip = buttonHoverSound;

    }

    // Update is called once per frame
    void Update()
    {
        background.volume = PlayerPrefs.GetFloat("Volume");
        buttonHover.volume = PlayerPrefs.GetFloat("Volume");
    }
    public void PlayButtonHoverSound()
    {
        buttonHover.Play();
    }
}
