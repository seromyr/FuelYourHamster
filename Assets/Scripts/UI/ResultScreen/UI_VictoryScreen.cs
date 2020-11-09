using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VictoryScreen : MonoBehaviour
{
    public static UI_VictoryScreen main;

    private Canvas canvas;
    private Text summaryText;

    private void Awake()
    { 
        // Make this UI a singleton
        Singletonize();

        TryGetComponent(out canvas);

        transform.Find("Panel").transform.Find("Message").TryGetComponent(out summaryText);
    }

    private void Singletonize()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
        }
        else if (main != null)
        {
            Destroy(gameObject);
        }
    }

    public void SetCanvasAtive(bool value)
    {
        canvas.enabled = value;
    }

    public void SetSummaryText(string text)
    {
        summaryText.text = text;
    }
}