using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gameplay_Mechanic : MonoBehaviour
{
    public static UI_Gameplay_Mechanic main;

    private Canvas canvas;
    private void Awake()
    {
        // Make UI_Gameplay_Mechanic become a Singleton
        Singletonizer();

        TryGetComponent(out canvas);
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
}
