using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public int health;
    [HideInInspector] public float gold;

    private Transform sprite;
    private Transform target;
    private GameObject goldText;
    private List<Transform> waypoints;
    private Vector3 dir;
    private Health healthSlider;
    private int waypointIndex = 0;
    private Rigidbody2D rb;

    public GameObject hitParticles;
    public GameObject particlePrefab;
    public GameObject goldTextPrefab;
    public float speed;
    public int lifeCost;
    public Transform lastHitBy;

    void Start()
    {
        waypoints = GameMaster.waypoints;
        target = waypoints[0];

        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Health>();
        healthSlider.SetMaxHealth(health);

        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("AddSpeed", 0, 0.5f);
        dir = (target.position - transform.position).normalized;
        rb.velocity = dir * speed;

        sprite = transform.GetChild(1);
    }

    void Update()
    {
        #region MOVEMENT
        target = waypoints[waypointIndex];
        dir = (target.position - transform.position).normalized;

        if(Vector3.Distance(transform.position, target.position) < 0.1f) {
            waypointIndex++;
            AddSpeed();
            if(waypointIndex == waypoints.Count) {
                GameMaster.TakeLife(lifeCost);
                Destroy(gameObject);
            }
        }
        #endregion

        #region ROTATION
        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        sprite.rotation = targetRotation;
        #endregion

        Physics2D.IgnoreLayerCollision(8, 8);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        healthSlider.SetHealth(health);
        GameObject particles = Instantiate(hitParticles, transform.position, transform.rotation);
        Destroy(particles, 1f);
        if(health <= 0)
            DestroyEnemy();
    }

    private void AddSpeed() {
        rb.velocity = dir * speed;
        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        sprite.rotation = targetRotation;
    }

    private void DestroyEnemy() {
        GameObject particles = Instantiate(particlePrefab, transform.position, transform.rotation);
        goldText = Instantiate(goldTextPrefab, transform.position, new Quaternion(0f, 0f, 0f, 0f));
        goldText.GetComponent<TextMesh>().text = "+" + gold.ToString();
        GameMaster.gold += gold;
        gameObject.SetActive(false);
        Destroy(particles, 1f);
        Destroy(goldText, 0.6f);
        Destroy(gameObject, 3f);
        lastHitBy.SendMessage("GiveKillXp");
    }
}
