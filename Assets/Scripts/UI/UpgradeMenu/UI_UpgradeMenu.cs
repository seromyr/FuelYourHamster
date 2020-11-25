using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradeMenu : MonoBehaviour
{
    public static UI_UpgradeMenu main;

    private Canvas canvas;

    private Transform statUpgradeButtons;

    private void Awake()
    {
        // Make this UI a singleton
        Singletonize();

        TryGetComponent(out canvas);

        statUpgradeButtons = transform.Find("Panel").Find("Stats Upgrade Buttons");
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

    public void Reset()
    {
        for (int i = 0; i < statUpgradeButtons.childCount; i++)
        {
            statUpgradeButtons.GetChild(i).gameObject.SetActive(true);
        }
    }
}