using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIGenerator : MonoBehaviour
{
    public GameObject waveSquare;
    public Transform[] sprites;
    private Wave[] waves;
    private Spawner spawner;
    void Start()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        waves = spawner.waves;
        RenderWaves();
    }

    public void RenderWaves() {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        if(spawner.waveNumber != waves.Length) {
            for (int i = spawner.waveNumber + 1; i < waves.Length; i++)
            {
                Wave wave = waves[i];
                Transform square = Instantiate(waveSquare).transform;
                square.SetParent(transform, false);
                Transform sprite = Instantiate(sprites[wave.enemyIndex]);
                sprite.SetParent(square.GetChild(2).transform, false);
                square.GetChild(3).GetChild(0).GetComponent<Text>().text = wave.enemyCount.ToString();
                if(wave.enemyCount >= 100) 
                    square.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 60;
                else 
                    square.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 80;
                square.GetComponent<WaveSquare>().waveIndex = i;
            }
        }
    }
}
