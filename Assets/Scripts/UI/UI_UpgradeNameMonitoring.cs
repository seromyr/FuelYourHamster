using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeNameMonitoring : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        textField.text = gameManager.UpgradeData.Stats[statID].name;
    }

    private void GetUpgradeStatID()
    {
        switch (transform.parent.name)
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
}
