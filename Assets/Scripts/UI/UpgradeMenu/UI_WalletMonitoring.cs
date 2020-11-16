using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WalletMonitoring : MonoBehaviour
{
    private Text textField;

    void Start()
    {
        TryGetComponent(out textField);
    }

    void Update()
    {
        //Always display the current number of money that player has in the text field
        textField.text = AddSeperatorInLargeNumber(Player.main.Wallet);
    }

    private string AddSeperatorInLargeNumber(int number)
    {
        //Use string format "N" to add comma in large number
        return string.Format("{0:N0}", number); ;
    }
}
