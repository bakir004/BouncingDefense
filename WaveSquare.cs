using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSquare : MonoBehaviour
{
    [HideInInspector] public int waveIndex;
    private Spawner spawner;
    void OnMouseDown() {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        spawner.skip = true;
        Spawner.timer = 0.01f;
        spawner.stopIndex = waveIndex - 1;
        GameObject.Find("Waves").GetComponent<WaveUIGenerator>().RenderWaves();
    }
}
