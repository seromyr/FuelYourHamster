using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeButton : MonoBehaviour
{
    private Button button;

    //private UI_EndRunNotice endRunNotice;

    private void Awake()
    {
        //GameObject.Find("EndRun").TryGetComponent(out endRunNotice);
    }
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowUpgrade);
    }
    public void ShowUpgrade()
    {
        GameManager.main.PauseGame();
        //endRunNotice.SetActive(false);
    }
}
