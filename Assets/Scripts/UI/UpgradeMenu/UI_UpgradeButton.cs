using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowUpgrade);
    }
    public void ShowUpgrade()
    {
        GameManager.main.PauseGame();
    }
}
