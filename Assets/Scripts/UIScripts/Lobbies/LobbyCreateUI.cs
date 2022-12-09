using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    // Simple sets a singular UI active
    public static LobbyCreateUI Instance { get; private set; }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
