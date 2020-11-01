using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class UI_UpgradeNameMonitoring : MonoBehaviour
{
    private int statID;
    private Text textField;

    void Start()
    {
        textField = GetComponent<Text>();
        GetUpgradeStatID();
    }

    void Update()
    {
        textField.text = UpgradeData.main.Stats[statID].name;
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
