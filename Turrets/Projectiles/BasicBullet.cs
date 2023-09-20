using UnityEngine;

public class BasicBullet : Bullet
{
    public GameObject particlesPrefab;

    public override void Start()
    {
        base.rb = GetComponent<Rigidbody2D>();
        if(isUltimateBullet) {
            enemyHits = 3;
            lifeBounces = 99;
            timed = true;
            timer = 5f;
        }
        base.Start();
    }

    private void DestroyBullet() {
        GameObject particles = Instantiate(particlesPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(particles, 1f);
        Destroy(gameObject, 3f);
    }
}
