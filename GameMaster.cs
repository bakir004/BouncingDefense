using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum TimeStatus {paused, playing, fastForwarded};

public class GameMaster : MonoBehaviour
{
    private UIController ui;

    public static Transform selectedTurret;
    public static Transform selectedNode;

    public static bool shopOpen;
    public static bool infoOpen;

    public static Joystick joystick;
    public static Joystick ultimateJoystick;
    public static List<Transform> waypoints;
    [HideInInspector] public TimeStatus timeStatus;
    [HideInInspector] public bool[] lockedTurrets;

    public static float gold;
    public static int lives;

    public float startingGold;
    public int startingLives;

    void Start()
    {
        ui = GetComponent<UIController>();
        lives = startingLives;
        gold = startingGold;
        timeStatus = TimeStatus.playing;
    
        joystick = GameObject.Find("PrimaryJoystick").GetComponent<Joystick>();
        ultimateJoystick = GameObject.Find("UltimateJoystick").GetComponent<Joystick>();
        infoOpen = false;
        shopOpen = false;

        for(int i = 0; i < lockedTurrets.Length; i++) {
            ui.lockedSquares[i].gameObject.SetActive(lockedTurrets[i]);
        }
    }

    void Update()
    {
        if(timeStatus == TimeStatus.paused)
            Time.timeScale = 0f;
        else if(timeStatus == TimeStatus.playing)
            Time.timeScale = 1f;
        else if(timeStatus == TimeStatus.fastForwarded)
            Time.timeScale = 2f;

        if(lives == 0)
            EndLevel();
    }

    public static void TakeLife(int amount) {
        lives -= amount;
    }

    public static void Pay(int amount) {
        gold -= amount;
    }

    public static string RemoveClone(string str) {
        for (int i = 0; i < str.Length; i++)
        {
            if(str[i] == '(')
                return str.Substring(0, i);
        }
        return str;
    }

    public static int GetLevel(string str) {
        for (int i = 0; i < str.Length; i++)
        {
            if((int)(str[i] - '0') >= 0 && (int)(str[i] - '0') <= 9) {

                return (int)(str[i] - '0');
            }
        }
        return 0;
    }

    public static string RemoveLevel(string str) {
        for (int i = 0; i < str.Length; i++)
        {
            if((int)(str[i] - '0') >= 0 && (int)(str[i] - '0') <= 9) {
                return str.Substring(0, i);
            }
        }
        return str;
    }

    public void OnBuyClickConfirm(string name) {
        GameMaster.selectedNode.GetComponent<Node>().BuildTurret(GetComponent<TurretManager>().GetTurretPrefab(name, 1).gameObject);
        shopOpen = false;
        ui.DisableGreenSquares();
    }

    public void Quit() => Application.Quit();

    public static void CloseUI() {
        shopOpen = false;
        infoOpen = false;
        selectedTurret = null;
        selectedNode = null;
    }

    public void Hi() {
        Debug.Log("hi");
    }

    public void EndLevel() {
        Invoke("ActivateEndScreen", 2.5f);
    }

    private void ActivateEndScreen() {
        timeStatus = TimeStatus.paused;
        ui.ActivateEndScreen();
    }

    public void BackToLevelSelect() {
        SceneManager.LoadScene("LevelSelect");
    }

    public int GetIndex(string name) {
        int index = -1;
        switch(name) {
                case "Basic": {
                    index = 0;
                    break;
                }
                case "Splash": {
                    index = 1;
                    break;
                }
                case "Pierce": {
                    index = 2;
                    break;
                }
            }
        return index;
    }

    public static int GetLevelMilestone(int lvl) {
        switch(lvl) {
            case 1:
                return 20;
            case 2:
                return 50;
            case 3:
                return 100;
            case 4: 
                return 200;
            case 5:
                return 500;
            case 6:
                return 1000;
            case 7:
                return 2500;
            case 8:
                return 5000;
            case 9: 
                return 10000;
            case 10:
                return 20000;
            case 11:
                return 50000;
            case 12:
                return 100000;
            case 13:
                return 200000;
        }
        return -1;
    }
}
