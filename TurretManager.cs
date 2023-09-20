using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public Transform[] basicTurrets;
    public Transform[] splashTurrets;
    public Transform[] pierceTurrets;
    public Transform[] basicSprites;
    public Transform[] splashSprites;
    public Transform[] pierceSprites;

    public Transform GetTurretPrefab(string name, int lvl) {
        switch(name) {
            case "Basic": {
                try {
                    return basicTurrets[lvl - 1];
                } catch {
                    return null;
                }
            }
            case "Splash": {
                try {
                    return splashTurrets[lvl - 1];
                } catch {
                    return null;
                }
            }
            case "Pierce": {
                try {
                    return pierceTurrets[lvl - 1];
                } catch {
                    return null;
                }
            }
        }
        return null;
    }
    public Transform GetTurretSprite(string name, int lvl) {
        switch(name) {
            case "Basic": {
                try {
                    return basicSprites[lvl - 1];
                } catch {
                    return null;
                }
            }
            case "Splash": {
                try {
                    return splashSprites[lvl - 1];
                } catch {
                    return null;
                }
            }
            case "Pierce": {
                try {
                    return pierceSprites[lvl - 1];
                } catch {
                    return null;
                }
            }
        }
        return null;
    }
}
