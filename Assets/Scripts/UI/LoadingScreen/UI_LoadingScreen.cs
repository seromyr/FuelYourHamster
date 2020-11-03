using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingScreen : MonoBehaviour
{
    public static UI_LoadingScreen main;
    private Canvas canvas;

    private void Start()
    {
        // Make Loading Screen become singleton
        Singletonize();

        TryGetComponent(out canvas);
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

    public void SetCanvasActive(bool value)
    {
        canvas.enabled = value;
    }

    public void SetCanvasActiveWithDelay(bool value, float delay)
    {
        StartCoroutine(SetCanvasActive(value, delay));
    }

    private IEnumerator SetCanvasActive(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.enabled = value;
    }
}
