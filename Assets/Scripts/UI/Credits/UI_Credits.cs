using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Constants;

public class UI_Credits : MonoBehaviour
{
    public static UI_Credits main;

    private CanvasGroup creditsGroup;
    private float time;

    [SerializeField, Header("Time to wait until ready to fade")]
    private float duration;

    [SerializeField, Header("Fade velocity")]
    private float speed;

    [SerializeField, Header("Type of fade")]
    private FadeType fadeType;

    private UI_CreditsSroller selfScroller;
    public UI_CreditsSroller SelfScroller { get { return selfScroller; } } 

    private void Awake()
    {
        Singletionizer();
        creditsGroup = gameObject.AddComponent<CanvasGroup>();
        selfScroller = GetComponentInChildren<UI_CreditsSroller>();
    }

    void Start()
    {
        fadeType = FadeType.Out;
        if (fadeType == FadeType.Out)
        {
            creditsGroup.alpha = 0;
        }
        else
        {
            // Credits is transparent by default
            creditsGroup.alpha = 1;

            // Record time
            time = Time.time;
        }
    }

    private void Singletionizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Credits created");
            main = this;
        }
        else if (main != null)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (fadeType == FadeType.In) SelfFadeIn();
        else if (fadeType == FadeType.Out) SelfFadeOut();
    }

    private void SelfFadeIn()
    {
        if (Time.time >= time + duration)
        {
            creditsGroup.alpha += Time.deltaTime * speed;
        }
    }

    private void SelfFadeOut()
    {
        creditsGroup.alpha -= Time.deltaTime * speed;
    }

    public void FadeOut()
    {
        fadeType = FadeType.Out;
        GetComponent<Canvas>().sortingOrder = 0;
    }

    public void FadeIn()
    {
        fadeType = FadeType.In;
        GetComponent<Canvas>().sortingOrder = 1;
        // Rest record time
        time = Time.time;
    }
}
