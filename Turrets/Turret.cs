using UnityEngine;

public class Turret : MonoBehaviour
{
    public int cost;
    public string description;

    [Header("Attributes")]
    public float firerate; // n per second
    public int damage;
    public int bulletBounces;
    public float bulletSpeed;
    [HideInInspector] public int xp;
    [HideInInspector] public int killXp;
    [HideInInspector] public int xpLevel;
    [HideInInspector] public int xpMilestone;
    [HideInInspector] public Transform nodeBuiltOn;

    void Awake()
    {
        xp = 0;
        xpLevel = 1;
        killXp = 5;
    }

    void FixedUpdate() {
        xpMilestone = GameMaster.GetLevelMilestone(xpLevel);
        if(xp >= xpMilestone) {
            xpLevel++;
            xp -= xpMilestone;
        }
    }
}
