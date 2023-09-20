using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
    {
        public int enemyIndex;
        public int enemyCount;
        public int enemyHP;
        public float timeBetweenSpawns;
        public int timeBetweenWaves;
        public int waveGold;

        public Wave(int _enemyIndex, int _enemyCount, int _enemyHP, float _timeBetweenSpawns, int _timeBetweenWaves, int _waveGold)
        {
            enemyIndex = _enemyIndex;
            enemyCount = _enemyCount;
            enemyHP = _enemyHP;
            timeBetweenSpawns = _timeBetweenSpawns;
            timeBetweenWaves = _timeBetweenWaves;
            waveGold = _waveGold;
        }
        public Wave()
        {
            enemyCount = 0;
            enemyHP = 0;
            timeBetweenWaves = 0;
            timeBetweenSpawns = 0;
            waveGold = 0;
        }
    }
    // public class Waves
    // {
    //     public Wave wave1;
    //     public Wave wave2;
    //     public Wave wave3;
    //     public Wave wave4;
    //     public Wave[] allWaves;

    //     public Waves()
    //     {
    //         // enemy type, count, health, time between spawns, time between waves
    //         wave1 = new Wave(0, 5, 10, 1f, 15, 20);
    //         wave2 = new Wave(0, 10, 15, 0.7f, 15, 30);
    //         wave3 = new Wave(0, 15, 15, 0.75f, 20, 40);
    //         wave4 = new Wave(1, 5, 10, 0.5f, 0, 30);

    //         allWaves = new Wave[] { wave1, wave2, wave3, wave4 };
    //     }
    // }
public class Spawner : MonoBehaviour
{
    
    // public Waves waves = new Waves();
    public Wave[] waves;
    public GameObject[] enemyPrefabs;
    public static float timer = 20f;
    public bool skip = false;
    private bool endOfWave = false;
    
    [Header("Waves")]
    [HideInInspector] public int stopIndex;
    [HideInInspector] public int waveNumber = -1;

    void Start() {
        timer = 20f;
    }

    void Update()
    {
        if(waveNumber >= stopIndex)
            skip = false;
        if(timer <= 0f) {
            if(waveNumber != waves.Length) {
                waveNumber++;
                if(waveNumber != waves.Length) {
                    StartCoroutine(SpawnWave(waves[waveNumber]));
                    if(!skip)
                        timer = waves[waveNumber].timeBetweenWaves;
                    else 
                        timer = 0.01f;
                } else {
                    GameObject.Find("Waves").GetComponent<WaveUIGenerator>().RenderWaves();
                }
            } else {
                timer = 0f;
            }
        }
        timer -= Time.deltaTime;

        if(timer <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && waveNumber == waves.Length && endOfWave) {
            GameObject.Find("UI").GetComponent<GameMaster>().EndLevel();
        }
    }

    public IEnumerator SpawnWave(Wave wave) {
        GameObject.Find("Waves").GetComponent<WaveUIGenerator>().RenderWaves();
        float goldPerEnemy = wave.waveGold / wave.enemyCount;
        endOfWave = false;
        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(enemyPrefabs[wave.enemyIndex], wave.enemyHP, goldPerEnemy);
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }
        endOfWave = true;
    }

    private void SpawnEnemy(GameObject enemyPrefab, int hp, float gold) {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemy.GetComponent<Enemy>().health = hp;
        enemy.GetComponent<Enemy>().gold = gold;
    }
}
