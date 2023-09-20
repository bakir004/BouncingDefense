using UnityEngine;

public class Splash : Turret
{
    [HideInInspector] public Vector3 bulletDirection;
    public Transform bulletPrefab;
    private Transform firepoint;
    private float fireCountdown = 0f;
    private Vector2 ultimateCastDir;
    private bool isUlting = false;

    [Header("Special Attributes")]
    public int splashDamage;
    public float splashRadius;

    public string[] attributeNames;
    public Sprite[] attributeSprites;
    public Color[] attributeColors;

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
        SplashBullet newBullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation).GetComponent<SplashBullet>();
        newBullet.dir = bulletDirection;
        newBullet.damage = base.damage;
        newBullet.splashDamage = splashDamage;
        newBullet.splashRadius = splashRadius;
        newBullet.speed = base.bulletSpeed;
        newBullet.turretShotFrom = transform;
        if(isUlting) {
            isUlting = false;
            newBullet.isUltimateBullet = true;
        } else {
            newBullet.lifeBounces = base.bulletBounces;
        }
    }

    public void GetAttributes() {
        
        float[] attributeValues = {splashDamage, splashRadius};
        UIController.values = attributeValues;
        UIController.additionalAttributeColors = attributeColors;
        UIController.additionalAttributeSprites = attributeSprites;
        UIController.additionalAttributeNames = attributeNames;

        Transform nextTurret = GameObject.Find("UI").GetComponent<TurretManager>().GetTurretPrefab(GameMaster.RemoveLevel(gameObject.name), GameMaster.GetLevel(gameObject.name) + 1);
        if(nextTurret != null) {
            float[] nextLevelValues = {nextTurret.GetComponent<Splash>().splashDamage, nextTurret.GetComponent<Splash>().splashRadius};
            UIController.nextLevelValues = nextLevelValues;
        }
        else {
            float[] nextLevelValues = null;
            UIController.nextLevelValues = nextLevelValues;
        }
    }

    public void Ultimate(Vector2 dir) {
        ultimateCastDir = dir;
        isUlting = true;
        GetComponent<TurretSelect>().SetDir(ultimateCastDir);
        Shoot();
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
