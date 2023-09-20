using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelClick(string lvl) {
        SceneManager.LoadScene("Level" + lvl);
    }

    public void Back() {
        SceneManager.LoadScene("TitleScreen");
    }
}
