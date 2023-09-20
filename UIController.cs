using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameMaster gameMaster;
    private TurretManager turretManager;
    [HideInInspector] public bool readyToConfirmUpgrade;
    [HideInInspector] public int clickedIndex = -1;

    [HideInInspector] public Text goldText;
    [HideInInspector] public Text livesText;
    [Header("Shop Component Control")]
    [HideInInspector] public GameObject turretDescriptionObj;
    [HideInInspector] public GameObject turretDescriptionPlaceholder;
    [HideInInspector] public GameObject[] confirmSquares;
    [HideInInspector] public GameObject[] lockedSquares;

    [Header("Shop Attributes")]
    [HideInInspector] public Text turretNameText;
    [HideInInspector] public Text turretDescText;
    [HideInInspector] public Text turretDamageText;
    [HideInInspector] public Text turretFirerateText;
    [HideInInspector] public Text turretBouncesText;
    [HideInInspector] public Text turretSpeedText;

    [Header("Info Panel Component Control")]
    [HideInInspector] public GameObject joystickPressedPanel;

    [Header("Info Panel Attributes")]
    [HideInInspector] public Text infoPanelTurretName;
    [HideInInspector] public Text infoPanelTurretDamage;
    [HideInInspector] public Text infoPanelTurretFirerate;
    [HideInInspector] public Text infoPanelTurretBounces;
    [HideInInspector] public Text infoPanelTurretSpeed;
    [HideInInspector] public GameObject xpSliderObj;
    [HideInInspector] public Text xpSliderText;
    [HideInInspector] public Text xpLevelText;

    [Header("Info Panel Upgrade")]
    [HideInInspector] public GameObject upgradeButton;
    [HideInInspector] public Text nextLevelTurretCost;
    [HideInInspector] public Sprite blueUpgradeSprite;
    [HideInInspector] public Sprite greenUpgradeSprite;
    
    [Header("Upgrades")]
    [HideInInspector] public Transform turret;
    private Transform nextLevelTurret;
    [HideInInspector] public GameObject upgradeText;
    [HideInInspector] public GameObject upgradeCoin;
    [HideInInspector] public Transform turretSpriteParent;

    [Header("Time Control")]
    [HideInInspector] public GameObject playing;
    [HideInInspector] public GameObject fastForwarded;
    [HideInInspector] public GameObject paused;

    [HideInInspector] public GameObject[] additionalAttributes;
    [HideInInspector] public GameObject[] attributePlaceholders;
    [HideInInspector] public GameObject endScreen;
    public static float[] values;
    public static float[] nextLevelValues;
    public static string[] additionalAttributeNames;
    public static Sprite[] additionalAttributeSprites;
    public static Color[] additionalAttributeColors;

    [HideInInspector] public GameObject shop;
    [HideInInspector] public GameObject turretInfo;
    [HideInInspector] public GameObject unselected;

    [HideInInspector] public Text countdownText;
    [HideInInspector] public GameObject noMoney;
    [HideInInspector] public GameObject ultimateJoystick;

    public GameObject overlay;
    public GameObject scopePrefab;
    public GameObject scope;
    public bool isSettingDir;
    public Vector3 scopePos = new Vector3(100f, 100f, -3f);

    private string tagOpen = "<color=#47F164> + ";
    private string tagClose = "</color>  ";

    void Start()
    {
        gameMaster = GetComponent<GameMaster>();
        turretManager = GetComponent<TurretManager>();

        turretDescriptionPlaceholder.SetActive(false);

        additionalAttributes[0].SetActive(false);
        additionalAttributes[1].SetActive(false);
        attributePlaceholders[0].SetActive(true);
        attributePlaceholders[1].SetActive(true);

        endScreen.SetActive(false);

        isSettingDir = false;
        scope = Instantiate(scopePrefab, transform.position, transform.rotation);
        scope.SetActive(false);
        noMoney.SetActive(false);

        DisableGreenSquares();
    }

    void Update()
    {
        joystickPressedPanel.SetActive(GameMaster.joystick.pressed);

        goldText.text = GameMaster.gold.ToString();
        livesText.text = GameMaster.lives.ToString();

        countdownText.text = Spawner.timer.ToString("#.0");

        #region TIME-CONTROL
        if(gameMaster.timeStatus == TimeStatus.paused) {
            playing.SetActive(true);
            fastForwarded.SetActive(false);
            paused.SetActive(false);
        } else if(gameMaster.timeStatus == TimeStatus.playing) {
            playing.SetActive(false);
            fastForwarded.SetActive(true);
            paused.SetActive(false);
        } else if(gameMaster.timeStatus == TimeStatus.fastForwarded) {
            playing.SetActive(false);
            fastForwarded.SetActive(false);
            paused.SetActive(true);
        }
        #endregion

        #region UI-COMPONENT-SWITCH
        shop.SetActive(GameMaster.shopOpen);
        turretInfo.SetActive(GameMaster.infoOpen);
        unselected.SetActive(!GameMaster.shopOpen && !GameMaster.infoOpen);
        #endregion

        #region SCOPE-ASSIGN-HANDLER
        overlay.SetActive(isSettingDir);
        scope.SetActive(GameMaster.selectedTurret != null);

        if(isSettingDir && Input.GetMouseButtonDown(0)) {

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            scopePos = new Vector3(pos.x, pos.y, -3);
            Vector3 dir = scopePos - GameMaster.selectedTurret.transform.position;

            #region ROTATION
            float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            GameMaster.selectedTurret.GetComponent<TurretSelect>().partToRotate.rotation = targetRotation;
            #endregion

            #region SETTING-TURRET-STUFF
            GameMaster.selectedTurret.GetComponent<TurretSelect>().dir = dir;
            GameMaster.selectedTurret.GetComponent<TurretSelect>().AssignDirection(dir);
            GameMaster.selectedTurret.GetComponent<TurretSelect>().scopePos = scopePos;
            #endregion

            GameMaster.joystick.SetHandlePosition(GameMaster.selectedTurret.GetComponent<TurretSelect>().dir.normalized);
            isSettingDir = false;
        }
        if(!GameMaster.joystick.pressed)
            scope.transform.position = scopePos;

        scope.transform.rotation = Quaternion.identity;
        #endregion
        
        #region XP-HANDLER
        if(xpSliderObj != null && turret != null) {
            xpSliderObj.GetComponent<Slider>().value = turret.GetComponent<Turret>().xp;
            xpSliderText.text = "XP " + turret.GetComponent<Turret>().xp + "/" + turret.GetComponent<Turret>().xpMilestone;
            xpSliderObj.GetComponent<Slider>().maxValue = turret.GetComponent<Turret>().xpMilestone;
            xpLevelText.text = "LVL" + turret.GetComponent<Turret>().xpLevel;
        }
        #endregion
    
        if(turret != null) {
            if(GameMaster.ultimateJoystick.pressed) {
                Vector3 dir = GameMaster.ultimateJoystick.Direction;
                float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                turret.GetComponent<TurretSelect>().ultimateObj.gameObject.SetActive(true);
                turret.GetComponent<TurretSelect>().ultimateObj.rotation = targetRotation;
            } else 
                turret.GetComponent<TurretSelect>().ultimateObj.gameObject.SetActive(false);
        }
    }

    public void OnBuyClick(string name) {
        if(gameMaster.lockedTurrets[gameMaster.GetIndex(name)] == true) 
            return;

        Transform turret = turretManager.GetTurretPrefab(name, 1);
        if(clickedIndex != gameMaster.GetIndex(name)) {
            turretDescriptionPlaceholder.SetActive(false);
            turretDescriptionObj.SetActive(true);
            
            #region SET-TEXT
            turretNameText.text = turret.gameObject.name;
            turretDescText.text = turret.GetComponent<Turret>().description;
            turretDamageText.text = turret.GetComponent<Turret>().damage.ToString() + " damage";
            turretFirerateText.text = turret.GetComponent<Turret>().firerate.ToString() + " bullets/s";
            turretBouncesText.text = turret.GetComponent<Turret>().bulletBounces.ToString() + " bounces";
            turretSpeedText.text = turret.GetComponent<Turret>().bulletSpeed.ToString() + " speed";
            #endregion
            
            DisableGreenSquares();
            clickedIndex = gameMaster.GetIndex(name);
            confirmSquares[clickedIndex].SetActive(true);

        } else if (clickedIndex == gameMaster.GetIndex(name) && GameMaster.gold >= turret.GetComponent<Turret>().cost){
            gameMaster.OnBuyClickConfirm(name);
            clickedIndex = -1;
            turretDescriptionObj.SetActive(false);
            turretDescriptionPlaceholder.SetActive(true);
        }
    }

    public void DisableGreenSquares() {
        foreach(GameObject square in confirmSquares) {
            square.SetActive(false);
        }
    }

    public void SetDir() {
        isSettingDir = true;
    }

    public void NoMoney() {
        noMoney.SetActive(true);
        Invoke("DisableNoMoney", 3f);
    }

    public void DisableNoMoney() {
        noMoney.SetActive(false);
    }

    public void SetTurretInfo(Transform _turret) {
        turret = _turret;
        readyToConfirmUpgrade = false;
        GameMaster.joystick.SetHandlePosition(turret.GetComponent<TurretSelect>().dir == new Vector3(0f, 0f, 0f) ? Vector3.right : turret.GetComponent<TurretSelect>().dir.normalized);
        DisableGreenSquares();

        #region SCOPE-HANDLER
        if(turret.GetComponent<TurretSelect>().scopePos == Vector3.zero)
            scopePos = turret.transform.position + new Vector3(2f, 0f, 0f);
        else 
            scopePos = turret.GetComponent<TurretSelect>().scopePos;
        #endregion

        #region SET-TEXT
        Turret turretScript = turret.GetComponent<Turret>();
        infoPanelTurretName.text = GameMaster.RemoveLevel(turretScript.gameObject.name);
        infoPanelTurretDamage.text = turretScript.damage.ToString() + "  ";
        infoPanelTurretFirerate.text = turretScript.firerate.ToString() + "  ";
        infoPanelTurretBounces.text = turretScript.bulletBounces.ToString() + "  ";
        infoPanelTurretSpeed.text = turretScript.bulletSpeed.ToString() + "  ";
        #endregion

        #region EXTRA-ATTRIBUTE-HANDLERS
        turret.SendMessage("GetAttributes");
        attributePlaceholders[0].SetActive(true);
        attributePlaceholders[1].SetActive(true);
        additionalAttributes[0].SetActive(false);
        additionalAttributes[1].SetActive(false);

        if(values != null && values.Length == 1) {
            attributePlaceholders[0].SetActive(false);
            SetAttributeUI(additionalAttributes[0], 0);
        } else if(values != null && values.Length == 2) {
            attributePlaceholders[0].SetActive(false);
            attributePlaceholders[1].SetActive(false);
            SetAttributeUI(additionalAttributes[0], 0);
            SetAttributeUI(additionalAttributes[1], 1);
        }
        #endregion

        #region UPGRADE-BUTTON-HANDLER
        turretSpriteParent.GetChild(0).gameObject.SetActive(true);
        upgradeCoin.SetActive(true);
        upgradeText.SetActive(true);
        upgradeButton.SetActive(true);
        upgradeButton.GetComponent<Image>().sprite = blueUpgradeSprite;

        int lvl = GameMaster.GetLevel(turret.gameObject.name);
        nextLevelTurret = turretManager.GetTurretPrefab(GameMaster.RemoveLevel(turret.gameObject.name), lvl + 1);
        if(nextLevelTurret != null) {
            nextLevelTurretCost.text = nextLevelTurret.GetComponent<Turret>().cost.ToString() + " ";
            if(turretSpriteParent.GetChild(0).transform != null) {
                Destroy(turretSpriteParent.GetChild(0).gameObject);
            }
            GameObject turretSprite = Instantiate(turretManager.GetTurretSprite(GameMaster.RemoveLevel(turret.gameObject.name), lvl + 1).gameObject);
            turretSprite.transform.SetParent(turretSpriteParent, false);
            nextLevelTurretCost.alignment = TextAnchor.MiddleRight;
        } else {
            upgradeCoin.SetActive(false);
            upgradeText.SetActive(false);
            upgradeButton.SetActive(false);
            turretSpriteParent.GetChild(0).gameObject.SetActive(false);
            nextLevelTurretCost.text = "MAX LEVEL ";
            nextLevelTurretCost.alignment = TextAnchor.MiddleCenter;
        }
        #endregion
    
        if(turret != null && turret.GetComponent<Turret>().xpLevel >= 1)
            ultimateJoystick.SetActive(true);
        else
            ultimateJoystick.SetActive(false);
    }

    public void UpgradeClick() {
        Turret script = turret.GetComponent<Turret>();
        Turret nextScript = nextLevelTurret.GetComponent<Turret>();
        if(readyToConfirmUpgrade) {
            if(GameMaster.gold >= nextScript.cost) {
                script.nodeBuiltOn.GetComponent<Node>().UpgradeTurret(nextLevelTurret.gameObject);
                readyToConfirmUpgrade = false;
            } else {
                noMoney.SetActive(true);
                Invoke("DeactivateNoMoney", 3f);
                readyToConfirmUpgrade = false;
                upgradeButton.GetComponent<Image>().sprite = blueUpgradeSprite;
            }
            
        } else {
            #region SHOW-UPGRADE-BENEFITS
            upgradeButton.GetComponent<Image>().sprite = greenUpgradeSprite;
            infoPanelTurretDamage.text = (nextScript.damage - script.damage != 0) ? script.damage + tagOpen + (nextScript.damage - script.damage).ToString() + tagClose : script.damage + "  ";
            infoPanelTurretFirerate.text = (nextScript.firerate - script.firerate != 0) ? script.firerate + tagOpen + (nextScript.firerate - script.firerate).ToString() + tagClose : script.firerate + "  ";
            infoPanelTurretBounces.text = (nextScript.bulletBounces - script.bulletBounces != 0) ? script.bulletBounces + tagOpen + (nextScript.bulletBounces - script.bulletBounces).ToString() + tagClose : script.bulletBounces + "  ";
            infoPanelTurretSpeed.text = (nextScript.bulletSpeed - script.bulletSpeed != 0) ? script.bulletSpeed + tagOpen + (nextScript.bulletSpeed - script.bulletSpeed).ToString() + tagClose : script.bulletSpeed + "  ";
            turret.SendMessage("GetAttributes");
            if(values != null && values.Length == 1) {
                additionalAttributes[0].transform.GetChild(2).transform.GetComponent<Text>().text = (nextLevelValues[0] - values[0] != 0) ? values[0] + tagOpen + (nextLevelValues[0] - values[0]).ToString() + tagClose : values[0] + "  ";
            } else if(values != null && values.Length == 2) {
                additionalAttributes[0].transform.GetChild(2).transform.GetComponent<Text>().text = (nextLevelValues[0] - values[0] != 0) ? values[0] + tagOpen + (nextLevelValues[0] - values[0]).ToString() + tagClose : values[0] + "  ";
                additionalAttributes[1].transform.GetChild(2).transform.GetComponent<Text>().text = (nextLevelValues[1] - values[1] != 0) ? values[1] + tagOpen + (nextLevelValues[1] - values[1]).ToString() + tagClose : values[1] + "  ";
            }
            #endregion
            readyToConfirmUpgrade = true;
        }
    }

    private void DeactivateNoMoney() {
        noMoney.SetActive(false);
    }

    public void ToggleTimeControl() {
        if(gameMaster.timeStatus == TimeStatus.paused)
            gameMaster.timeStatus = TimeStatus.playing;
        else if(gameMaster.timeStatus == TimeStatus.playing)
            gameMaster.timeStatus = TimeStatus.fastForwarded;
        else if(gameMaster.timeStatus == TimeStatus.fastForwarded)
            gameMaster.timeStatus = TimeStatus.paused;
    }

    public void SetAttributeUI(GameObject attr, int index) {
        attr.SetActive(true);
        attr.transform.GetChild(0).transform.GetComponent<Image>().sprite = additionalAttributeSprites[index];
        attr.transform.GetChild(1).transform.GetComponent<Text>().text = "  " + additionalAttributeNames[index];
        attr.transform.GetChild(2).transform.GetComponent<Text>().text = values[index].ToString() + "  ";
        attr.transform.GetChild(3).transform.GetComponent<Image>().color = additionalAttributeColors[index];
    }

    public void ActivateEndScreen() {
        endScreen.SetActive(true);
    }
}

