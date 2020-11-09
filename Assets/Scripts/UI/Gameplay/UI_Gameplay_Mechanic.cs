using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gameplay_Mechanic : MonoBehaviour
{
    public static UI_Gameplay_Mechanic main;

    private Canvas canvas;

    private GameObject preRunNotice, endRunNotice, speedUpNotice;

    private void Awake()
    {
        // Make UI_Gameplay_Mechanic become a Singleton
        Singletonizer();

        TryGetComponent(out canvas);

        preRunNotice = transform.Find("CountDown").gameObject;
        endRunNotice = transform.Find("EndRun").gameObject;
        speedUpNotice = transform.Find("SpeedUp").gameObject;
    }

    private void Start()
    {
        preRunNotice.SetActive(false);
        endRunNotice.SetActive(false);
        speedUpNotice.SetActive(false);
    }
    private void Singletonizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Gameplay UI created created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetCanvasActive(bool value)
    {
        canvas.enabled = value;
    }

    public void StartCountDown()
    {
        preRunNotice.SetActive(true);
    }

    public void ShowEndRunNotice()
    {
        endRunNotice.SetActive(true);
    }

    public void ShowSpeedUpNotice()
    {
        speedUpNotice.SetActive(true);
    }
}
