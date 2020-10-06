using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PurchaseMonitoring : MonoBehaviour
{
    private GameManager gameManager;
    private int statID;
    private Button button;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        GetUpgradeStatID();

        button.onClick.AddListener(PurchaseAction);
    }

    void Update()
    {
        button.interactable = gameManager.UpgradeData.Stats[statID].available;
    }

    private void GetUpgradeStatID()
    {
        switch (transform.name)
        {
            case "Stat_00":
                statID = 0;
                break;
            case "Stat_01":
                statID = 1;
                break;
            case "Stat_02":
                statID = 2;
                break;
            case "Stat_03":
                statID = 3;
                break;
            case "Stat_04":
                statID = 4;
                break;
        }
    }

    private void PurchaseAction()
    {
        gameManager.PurchaseUpgrade(statID);
    }
}
