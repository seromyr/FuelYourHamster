using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeCostMonitoring : MonoBehaviour
{
    private GameManager gameManager;
    private int statID;
    private Text textField;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        textField = GetComponent<Text>();
        GetUpgradeStatID();
    }

    void Update()
    {
        textField.text = AddSeperatorInLargeNumber(gameManager.UpgradeData.Stats[statID].cost);
    }

    private void GetUpgradeStatID()
    {
        switch (transform.name)
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

    private string AddSeperatorInLargeNumber(int number)
    {
        //Use string format "N" to add comma in large number
        return string.Format("{0:N0}", number); ;
    }
}
