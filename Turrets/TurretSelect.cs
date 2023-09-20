using UnityEngine;

public class TurretSelect : MonoBehaviour
{
    [HideInInspector] public Transform partToRotate;
    [HideInInspector] public Transform ultimateObj;
    [HideInInspector] public Vector3 dir;
    public Vector3 scopePos = Vector3.zero;

    void Awake()
    {
        dir = Vector3.right;
        for (int i = 0; i < transform.childCount; i++)
            if(transform.GetChild(i).gameObject.name == "PartToRotate")
                partToRotate = transform.GetChild(i);
            else if(transform.GetChild(i).gameObject.name == "UltimateObj")
                ultimateObj = transform.GetChild(i);
        
        SetDir(dir);
    }

    void Update()
    {
        if(GameMaster.joystick.pressed && GameMaster.selectedTurret == transform) {
            dir = GameMaster.joystick.Direction;

            GameObject.Find("Scope(Clone)").transform.parent = partToRotate;
            scopePos = GameObject.Find("Scope(Clone)").transform.position;
            GameObject.Find("UI").GetComponent<UIController>().scopePos = GameObject.Find("Scope(Clone)").transform.position;
            SetDir(dir);
        } else {
            if(GameObject.Find("Scope(Clone)") != null && GameObject.Find("Scope(Clone)").transform.parent != null)
                GameObject.Find("Scope(Clone)").transform.parent = null;
        }

        if(!GameMaster.joystick.pressed && GameMaster.selectedTurret == transform)
            GameMaster.joystick.Direction = dir;
    }

    public void AssignDirection(Vector3 direction) {
        SendMessage("SetBulletDirection", direction);
    }

    public void SetDir(Vector3 _dir) {
        dir = _dir;
        AssignDirection(dir);
        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        partToRotate.rotation = targetRotation;
        GameMaster.joystick.Direction = dir;
        GameMaster.joystick.SetHandlePosition(dir);
    }
}
