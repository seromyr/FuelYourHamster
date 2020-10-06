using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeLevelMonitoring : MonoBehaviour
{
    private GameManager gameManager;
    private Image image;
    private int statID;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        image = GetComponent<Image>();
        GetUpgradeStatID();
    }

    void Update()
    {
        image.fillAmount = (float)gameManager.UpgradeData.Stats[statID].level / 10;
    }

    private void GetUpgradeStatID()
    {
        switch (transform.parent.name)
        {
            case StatID._01:
                statID = 0;
                break;
            case StatID._02:
                statID = 1;
                break;
            case StatID._03:
                statID = 2;
                break;
            case StatID._04:
                statID = 3;
                break;
            case StatID._05:
                statID = 4;
                break;
        } 
    }
}
