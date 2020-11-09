using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScreen : MonoBehaviour
{
    public static UI_LoadingScreen main;
    private Canvas canvas;

    private Text currentHint;
    private string[] loadingHints;

    private void Awake()
    {
        transform.Find("Panel").transform.Find("Loading Hint").TryGetComponent(out currentHint);
    }

    private void Start()
    {
        // Make Loading Screen become singleton
        Singletonize();

        TryGetComponent(out canvas);

        loadingHints = new string[]
        {
            " ",
            "Press A D to change lane\n Hold Space Bar to jump higher",
            "Green outlined objects restore lost HP to Nibbles",
            "Red outlined objects deal damage to Nibbles ",
            "It's best to jump over large red obstacles",
            "The longer you hold Jump button, the higher Nibbles jumps",
            "Upgrade is the key to success",
            "End game is when you maxed all available upgrades",
            "Our beloved hamster name is Nibbles",
        };
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

    public void SetHint(int hintID)
    {
        if (hintID >= 0)
        {
            currentHint.text = loadingHints[hintID];
        }
        else if (hintID == -1)
        {
            currentHint.text = loadingHints[Random.Range(0, loadingHints.Length)];
        }
    }
}
