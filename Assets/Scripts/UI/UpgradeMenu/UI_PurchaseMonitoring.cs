using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PurchaseMonitoring : MonoBehaviour
{
    private int statID;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        GetUpgradeStatID();

        button.onClick.AddListener(PurchaseAction);
    }

    void Update()
    {
        button.interactable = UpgradeData.main.Stats[statID].available;
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
        GameManager.main.PurchaseUpgrade(statID);
    }
}
