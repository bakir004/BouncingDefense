using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 lastVelocity;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public int bounces;

    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Transform turretShotFrom;
    [HideInInspector] public float timer;
    [HideInInspector] public bool timed = false;

    [HideInInspector] public int damage;
    [HideInInspector] public int lifeBounces;
    [HideInInspector] public float speed;
    [HideInInspector] public bool isUltimateBullet = false;
    [HideInInspector] public int enemyHits;

    public virtual void Start()
    {
        if(dir != null)
            rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);

        bounces = lifeBounces;

        if(timed)
            Invoke("DestroyBullet", timer);
    }

    void Update()
    {
        lastVelocity = rb.velocity;
        if(bounces <= 0 || (timed && enemyHits <= 0))
            DestroyBullet();

        Physics2D.IgnoreLayerCollision(5, 6);
    }

    public virtual void OnCollisionEnter2D(Collision2D coll) {
        var speed = lastVelocity.magnitude;
        dir = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
        rb.velocity = dir * speed;

        bounces--;

        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        if(coll.gameObject.tag == "Enemy") {
            if(timed)
                enemyHits--;

            coll.gameObject.GetComponent<Enemy>().lastHitBy = transform;

            if(turretShotFrom != null)
                turretShotFrom.GetComponent<Turret>().xp += 1;

            coll.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Play Area") {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D coll) {
        if(coll.gameObject.name == "OutOfBounds") {
            Destroy(gameObject);
        }
    }

    public void GiveKillXp() {
        turretShotFrom.SendMessage("GiveKillXp");
    }

    public Transform GetTurretShotFrom() {
        return turretShotFrom.transform;
    }

    private void DestroyBullet() {
        transform.SendMessage("DestroyBullet");
    }
}
