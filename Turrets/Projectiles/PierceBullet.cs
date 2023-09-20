using UnityEngine;

public class PierceBullet : Bullet
{
    public GameObject particlesPrefab;

    public override void Start() {
        base.rb = GetComponent<Rigidbody2D>();
        base.Start();
    }

    public override void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        } else
            base.bounces++;
    }

    public override void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<Enemy>().lastHitBy = transform;
            turretShotFrom.GetComponent<Turret>().xp += 1;
            coll.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            if(GameMaster.RemoveClone(coll.gameObject.name) == "AntiPierce") {
                DestroyBullet();
            }
        }
    }

    private void DestroyBullet() {
        GameObject particles = Instantiate(particlesPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(particles, 1f);
        Destroy(gameObject, 1f);
    }
}
