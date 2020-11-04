using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Constants;


public class UI_MainMenu : MonoBehaviour
{
    public static UI_MainMenu main;

    private CanvasGroup mainMenuGroup;
    private float time;

    [SerializeField, Header("Time to wait until ready to fade")]
    private float duration;

    [SerializeField, Header("Fade velocity")]
    private float speed;

    [SerializeField, Header("Type of fade")]
    private FadeType fadeType;

    private void Awake()
    {
        Singletionizer();
        mainMenuGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        if (fadeType == FadeType.Out)
        {
            mainMenuGroup.alpha = 1;
        }
        else
        {
            // Main Menu is transparent by default
            mainMenuGroup.alpha = 0;

            // Record time
            time = Time.time;
        }
    }

    private void Singletionizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Main Menu created");
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
            mainMenuGroup.alpha += Time.deltaTime * speed;
        }
    }

    private void SelfFadeOut()
    {
        mainMenuGroup.alpha -= Time.deltaTime * speed;
    }

    public void FadeOut(float _speed)
    {
        speed = _speed;
        fadeType = FadeType.Out;
        GetComponent<Canvas>().sortingOrder = 0;
    }

    public void FadeIn(float _speed)
    {
        speed = _speed;
        fadeType = FadeType.In;
        GetComponent<Canvas>().sortingOrder = 1;
        // Rest record time
        time = Time.time;
    }
}
