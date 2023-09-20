using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private UIController ui;
    private Transform inner;
    private GameObject nodeSelectSquare;

    [HideInInspector] public Transform turret;

    void Start()
    {
        ui = GameObject.Find("UI").GetComponent<UIController>();
        inner = transform.GetChild(0);
        nodeSelectSquare = transform.GetChild(1).gameObject;
        ActivateSquares(false);
    }

    void Update()
    {
        if(GameMaster.selectedNode == transform || (turret != null && GameMaster.selectedTurret == turret))
            ActivateSquares(true);
        else 
            ActivateSquares(false);

        inner.gameObject.SetActive(turret == null);
    }
    
    void OnMouseDown() {
        if(turret == null) {
            if(GameMaster.selectedNode == transform)
                GameMaster.CloseUI();
            else
                OpenShopAndSelectNode();
        } else {
            if(GameMaster.selectedTurret == turret)
                GameMaster.CloseUI();
            else
                OpenInfoAndSelectTurret();
        }
    }

    private void OpenShopAndSelectNode() {
        ui.DisableGreenSquares();
        ui.clickedIndex = -1;
        GameMaster.shopOpen = true;
        GameMaster.infoOpen = false;
        GameMaster.selectedNode = transform;
        GameMaster.selectedTurret = null;
    }

    private void OpenInfoAndSelectTurret() {
        ui.SetTurretInfo(turret);
        GameMaster.shopOpen = false;
        GameMaster.infoOpen = true;
        GameMaster.selectedTurret = turret;
        GameMaster.selectedNode = null;
    }

    public void UpgradeTurret(GameObject turretPrefab) {
        Vector3 dir = turret.GetComponent<TurretSelect>().dir.normalized;
        Vector3 scopePos = ui.scopePos;
        int xpLevel = turret.GetComponent<Turret>().xpLevel;
        int xp = turret.GetComponent<Turret>().xp;
        List<Transform> bullets = new List<Transform>();
        // foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
        //     Debug.Log("what");
        // }
        Destroy(turret.gameObject);
        
        BuildTurret(turretPrefab.gameObject);
        turret.GetComponent<TurretSelect>().SetDir(dir);
        turret.GetComponent<Turret>().xp = xp;
        turret.GetComponent<Turret>().xpLevel = xpLevel;
        turret.GetComponent<TurretSelect>().scopePos = scopePos;
        OpenInfoAndSelectTurret();
    }

    public void BuildTurret(GameObject turretPrefab) {
        Turret script = turretPrefab.GetComponent<Turret>();
        script.nodeBuiltOn = transform;

        Vector3 position = new Vector3(transform.position.x, transform.position.y, -2);
        turret = Instantiate(turretPrefab, position, transform.rotation).transform;
        turret.name = GameMaster.RemoveClone(turret.name);

        OpenInfoAndSelectTurret();
        GameMaster.Pay(script.cost);

        turret.GetComponent<TurretSelect>().AssignDirection(Vector3.right);
        GameMaster.joystick.SetHandlePosition(Vector3.right);
    }

    void ActivateSquares(bool activate) {
        for (int i = 0; i < nodeSelectSquare.transform.childCount; i++)
        {
            Transform child = nodeSelectSquare.transform.GetChild(i).transform;
            Color color = child.GetComponent<SpriteRenderer>().color;
            if(activate)
                color.a = 1f;
            else
                color.a = 0f;
            child.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
