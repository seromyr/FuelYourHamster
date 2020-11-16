using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndRunNotice : MonoBehaviour
{
    private Text countDownText;
    private string message;

    private void Awake()
    {
        countDownText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        if (Player.main.Health <= 0)
        {
            message = "Oops! Nibbles is out of Health";
        }

        if (Player.main.CaffeineCurrentLevel <= 0)
        {
            message = "Oops! Nibbles is out of Fuel";
        }

        StartCoroutine(CoutDownSequence(message));
    }

    private IEnumerator CoutDownSequence(string message)
    {
        Player.main.IsConsumingFuel(false);
        countDownText.text = message;
        yield return new WaitForSeconds(3f);
        countDownText.text = " ";
        gameObject.SetActive(false);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
