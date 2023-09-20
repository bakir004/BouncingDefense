using System.Collections.Generic;
using UnityEngine;

public class SplashBullet : Bullet
{
    public GameObject particlesPrefab;
    public GameObject ultimateParticlesPrefab;

    [HideInInspector] public int splashDamage;
    [HideInInspector] public float splashRadius;

    public override void Start() {
        base.rb = GetComponent<Rigidbody2D>();
        if(isUltimateBullet) {
            Debug.Log("isultimate");
            lifeBounces = 99;
            enemyHits = 1;
            splashDamage *= 2;
            splashRadius *= 1.5f;
            timed = true;
            timer = 15f;
            transform.localScale = new Vector3(0.25f, 0.25f, 1);
            Debug.Log(lifeBounces);
        }
        base.Start();
    }

    private void DestroyBullet() {
        DealSplashDamage();
        GameObject particles = Instantiate(particlesPrefab, transform.position, transform.rotation);
        Destroy(particles, 1f);
        Destroy(gameObject, 3f);
    }

    private void DealSplashDamage() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesToDamage = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) <= splashRadius) {
                enemiesToDamage.Add(enemy);
            }
        }

        foreach (GameObject enemy in enemiesToDamage)
        {
            enemy.GetComponent<Enemy>().lastHitBy = transform;
            enemy.GetComponent<Enemy>().TakeDamage(splashDamage);
        }
        gameObject.SetActive(false);
    }
}
