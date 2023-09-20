using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public static AudioClip fart;
    public static AudioSource audioSource;
    void Start() {
        fart = Resources.Load("fart") as AudioClip;
        audioSource = GetComponent<AudioSource>();
    }
    public void Quit() {
        Application.Quit();
    }

    public void Play() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Fart() {
        PlaySound("fart");
    }

    public static void PlaySound(string name) {
        switch(name) {
            case "fart": {
                audioSource.PlayOneShot(fart, 1f);
                break;
            }
        }
    }
}
