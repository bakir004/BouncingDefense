using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private bool clickable = false;
    void Start()
    {
        Invoke("LoadScene", 7f);
        Invoke("Clickable", 2.5f);
    }

    void LoadScene() {
        SceneManager.LoadScene("TitleScreen");
    }

    public void Click() {
        if(clickable)
            SceneManager.LoadScene("TitleScreen");
    }

    void Clickable() {
        clickable = true;
    }
}
