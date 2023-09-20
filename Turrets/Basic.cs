using System.Collections;
using UnityEngine;

public class Basic : Turret
{
    [HideInInspector] public Vector3 bulletDirection;
    private float fireCountdown = 0f;
    private Transform firepoint;
    public Transform bulletPrefab;
    private Vector2 ultimateCastDir;

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
            Shoot(Vector2.zero);
            fireCountdown = 1 / base.firerate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot(Vector2 newDir) {
        BasicBullet newBullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation).GetComponent<BasicBullet>();
        
        if(newDir != Vector2.zero) {
            newBullet.isUltimateBullet = true;
            newBullet.dir = newDir;
        }
        else
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

    public void Ultimate(Vector2 dir) {
        ultimateCastDir = dir;
        StartCoroutine(SprayBullets());
    }

    private IEnumerator SprayBullets() {
        for (int i = 0; i < 10; i++)
        {
            float coeficient = Random.Range(-0.25f, 0.25f);
            Vector2 newDir = new Vector2(ultimateCastDir.x + coeficient, ultimateCastDir.y - coeficient);
            GetComponent<TurretSelect>().SetDir(newDir);
            GameMaster.joystick.SetHandlePosition(newDir);
            Shoot(newDir);
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<TurretSelect>().SetDir(ultimateCastDir);
        SetBulletDirection(ultimateCastDir);
        GameMaster.joystick.SetHandlePosition(ultimateCastDir);
        GetComponent<TurretSelect>().scopePos = transform.position + new Vector3(ultimateCastDir.x, ultimateCastDir.y, -2f) * 2;
        GameObject.Find("UI").GetComponent<UIController>().scopePos = transform.position + new Vector3(ultimateCastDir.x, ultimateCastDir.y, -2f) * 2;
    }

    public void GiveKillXp() {
        GetComponent<Turret>().xp += base.killXp;
    }

    public void SetBulletDirection(Vector3 dir) {
        bulletDirection = dir;
    }
}