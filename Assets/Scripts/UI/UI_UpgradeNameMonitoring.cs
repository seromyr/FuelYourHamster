﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

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
