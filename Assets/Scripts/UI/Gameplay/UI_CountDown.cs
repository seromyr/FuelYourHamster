using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CountDown : MonoBehaviour
{
    private Text countDownText;

    private void Awake()
    {
        countDownText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(CoutDownSequence());
    }

    private IEnumerator CoutDownSequence()
    {
        countDownText.text = "3";
        yield return new WaitForSeconds(2f); 
        countDownText.text = "2";
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "1";
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "GO";
        yield return new WaitForSeconds(0.5f);
        Player.main.IsConsumingFuel = true;
        gameObject.SetActive(false);
    }
}
