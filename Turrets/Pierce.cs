using UnityEngine;

public class Pierce : Turret
{
    [HideInInspector] public Vector3 bulletDirection;
    public Transform bulletPrefab;
    private Transform firepoint;
    private float fireCountdown = 0f;
    
    void Start()
    {
        Transform partToRotate = GetComponent<TurretSelect>().partToRotate;
        for (int i = 0; i < partToRotate.childCount; i++)
        {
            if(partToRotate.GetChild(i).gameObject.name == "Firepoint")
                firepoint = partToRotate.GetChild(i);
        }
    }

    void Update()
    {
        if(fireCountdown <= 0f) {
            Shoot();
            fireCountdown = 1 / base.firerate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot() {
        PierceBullet newBullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation).GetComponent<PierceBullet>();
        newBullet.dir = bulletDirection;
        newBullet.damage = base.damage;
        newBullet.lifeBounces = base.bulletBounces;
        newBullet.speed = base.bulletSpeed;
        newBullet.turretShotFrom = transform;
    }

    public void GetAttributes() {
        UIController.values = null;
        UIController.additionalAttributeColors = null;
        UIController.additionalAttributeSprites = null;
        UIController.additionalAttributeNames = null;
    }

    public void GiveKillXp() {
        GetComponent<Turret>().xp += base.killXp;
    }

    public void SetBulletDirection(Vector3 dir) {
        bulletDirection = dir;
    }
}