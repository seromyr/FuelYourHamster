using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeCostMonitoring : MonoBehaviour
{
    private int statID;
    private Text textField;
    private Color enableColor, disableColor;

    void Start()
    {
        textField = GetComponent<Text>();
        GetUpgradeStatID();

        ColorUtility.TryParseHtmlString("#926143FF", out enableColor);
        ColorUtility.TryParseHtmlString("#9AB9D1FF", out disableColor);
    }

    void Update()
    {
        if (UpgradeData.main.CheckUpgradeIsAlreadyMax(statID))
        {
            textField.text = "MAX";
            textField.color = disableColor;
        }
        else if (!GameManager.main.AllowUpgradePurchasing(statID))
        {
            textField.color = disableColor;
            textField.text = AddSeperatorInLargeNumber(UpgradeData.main.Stats[statID].cost);
        }
        else
        {
            textField.text = AddSeperatorInLargeNumber(UpgradeData.main.Stats[statID].cost);
            textField.color = enableColor;
        }
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
