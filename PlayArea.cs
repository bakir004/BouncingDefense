using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    void OnMouseDown() {
        GameMaster.CloseUI();
    }
}
