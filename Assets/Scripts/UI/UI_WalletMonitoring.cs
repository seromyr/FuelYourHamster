using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WalletMonitoring : MonoBehaviour
{
    private GameManager gameManager;
    private Text textField;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        textField = GetComponent<Text>();
    }

    void Update()
    {
        //Always display the current number of money that player has in the text field
        textField.text = AddSeperatorInLargeNumber(gameManager.CheckWallet);
    }

    private string AddSeperatorInLargeNumber(int number)
    {
        //Use string format "N" to add comma in large number
        return string.Format("{0:N0}", number); ;
    }
}
