using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sound;

    [Header("Background")]
    public AudioClip backgroundClip;
    [Header("Interaction")]
    public AudioClip pickUpClip;
    [Header("Book")]
    public AudioClip bookClip;
    [Header("Scroll")]
    public AudioClip scrollClip;
    [Header("Door")]
    public AudioClip doorClip;
    [Header("Lock")]
    public AudioClip lockClip;
    void Start()
    {
        music.loop = true;
        music.clip = backgroundClip;
        music.Play();
        music.ignoreListenerPause = true;
        sound.ignoreListenerPause = true;
        music.volume = PlayerPrefs.GetFloat("Volume");
        sound.volume = PlayerPrefs.GetFloat("Volume") * 2f;
    }

    public void UpdateVolume()
    {
        music.volume = PlayerPrefs.GetFloat("Volume");
        sound.volume = PlayerPrefs.GetFloat("Volume") * 2f;
    }

    public void OpenScroll()
    {
        sound.clip = scrollClip;
        sound.Play();
    }
    public void OpenBook()
    {
        sound.clip = bookClip;
        sound.Play();
    }
    public void PickUp()
    {
        sound.clip = pickUpClip;
        sound.Play();
    }
    public void OpenDoor()
    {
        sound.clip = doorClip;
        sound.Play();
    }
    public void UnlockLock()
    {
        sound.clip = lockClip;
        sound.Play();
    }
}
