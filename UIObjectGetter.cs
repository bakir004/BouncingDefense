using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIObjectGetter : MonoBehaviour
{
    void Awake()
    {
        UIController ui = GetComponent<UIController>();
        GameMaster gm = GetComponent<GameMaster>();
        ui.lockedSquares = GameObject.FindGameObjectsWithTag("LockedSquares");

        GameObject[] confirms = GameObject.FindGameObjectsWithTag("ConfirmSquares");
        GameObject[] orderedConfirms = new GameObject[confirms.Length];
        foreach (GameObject square in confirms)
        {
            int index = (int)square.name[square.name.Length - 1];
            orderedConfirms[index - 1 - '0'] = square;
        }
        
        ui.confirmSquares = orderedConfirms;
        ui.turretDescriptionObj = GameObject.Find("TurretDescription");
        ui.turretDescriptionPlaceholder = GameObject.Find("TurretDescriptionPlaceholder");
        ui.joystickPressedPanel = GameObject.Find("JoystickPressedPanel");

        ui.turretNameText = GameObject.Find("ShopTurretName").GetComponent<Text>();
        ui.turretDescText = GameObject.Find("ShopTurretDesc").GetComponent<Text>();
        ui.turretDamageText = GameObject.Find("TurretDamage").transform.GetChild(2).GetComponent<Text>();
        ui.turretFirerateText = GameObject.Find("TurretFirerate").transform.GetChild(2).GetComponent<Text>();
        ui.turretBouncesText = GameObject.Find("TurretBounces").transform.GetChild(2).GetComponent<Text>();
        ui.turretSpeedText = GameObject.Find("TurretSpeed").transform.GetChild(2).GetComponent<Text>();

        ui.infoPanelTurretName = GameObject.Find("TurretName").GetComponent<Text>();

        ui.infoPanelTurretDamage = GameObject.Find("AttributePanel1").transform.GetChild(2).GetComponent<Text>();
        ui.infoPanelTurretFirerate = GameObject.Find("AttributePanel2").transform.GetChild(2).GetComponent<Text>();
        ui.infoPanelTurretBounces = GameObject.Find("AttributePanel3").transform.GetChild(2).GetComponent<Text>();
        ui.infoPanelTurretSpeed = GameObject.Find("AttributePanel4").transform.GetChild(2).GetComponent<Text>();

        ui.upgradeButton = GameObject.Find("UpgradeButton");
        ui.nextLevelTurretCost = GameObject.Find("Cost").GetComponent<Text>();
        ui.blueUpgradeSprite = Resources.Load<Sprite>("Icons/UpgradeButton");
        ui.greenUpgradeSprite = Resources.Load<Sprite>("Icons/UpgradeButtonConfirm");

        ui.upgradeText = GameObject.Find("Upgrade");
        ui.turretSpriteParent = GameObject.Find("Turret").transform;
        ui.upgradeCoin = GameObject.Find("UpgradeCoin");
        ui.additionalAttributes = new GameObject[] {GameObject.Find("ExtraAttributes1"), GameObject.Find("ExtraAttributes2")};
        ui.attributePlaceholders = GameObject.FindGameObjectsWithTag("AttributePlaceholders");
        ui.endScreen = GameObject.Find("EndScreen");
        ui.noMoney = GameObject.Find("NoMoney");

        ui.playing = GameObject.Find("Playing");
        ui.fastForwarded = GameObject.Find("FastForwarded");
        ui.paused = GameObject.Find("Paused");

        ui.goldText = GameObject.Find("Gold").GetComponent<Text>();
        ui.livesText = GameObject.Find("Lives").GetComponent<Text>();

        ui.shop = GameObject.Find("Shop");
        ui.turretInfo = GameObject.Find("TurretInfo");
        ui.unselected = GameObject.Find("Unselected");

        ui.xpSliderObj = GameObject.Find("XPBar");
        ui.xpSliderText = GameObject.Find("XP").GetComponent<Text>();
        ui.xpLevelText = GameObject.Find("TurretLevel").GetComponent<Text>();

        ui.countdownText = GameObject.Find("Timer").GetComponent<Text>();
        ui.ultimateJoystick = GameObject.Find("UltimateJoystick");

        if(SceneManager.GetActiveScene().name == "Level1") 
            gm.lockedTurrets = new bool[] {false, true, true};
        else if(SceneManager.GetActiveScene().name == "Level2")
            gm.lockedTurrets = new bool[] {false, true, false};
        else if(SceneManager.GetActiveScene().name == "Level3")
            gm.lockedTurrets = new bool[] {false, false, false};
    }
}
