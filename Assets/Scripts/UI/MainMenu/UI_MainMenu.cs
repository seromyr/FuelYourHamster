using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Constants;

public class UI_MainMenu : MonoBehaviour
{
    public static UI_MainMenu main;

    private CanvasGroup mainMenuGroup;
    private Canvas canvas;
    private float time;

    [SerializeField, Header("Time to wait until ready to fade")]
    private float duration;

    [SerializeField, Header("Fade velocity")]
    private float speed;

    [SerializeField, Header("Type of fade")]
    private FadeType fadeType;

    private GameObject playButton, creditButton, copyright, howToPlay;

    private void Awake()
    {
        Singletionizer();
        mainMenuGroup = gameObject.AddComponent<CanvasGroup>();
        TryGetComponent(out canvas);

        howToPlay = transform.Find("HowToPlay").gameObject;
        playButton = transform.Find("PlayButton").gameObject;
        creditButton = transform.Find("CreditButton").gameObject;
        copyright = transform.Find("Copyright").gameObject;
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

        howToPlay.SetActive(false);
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

    public void SetCanvasActive(bool value)
    {
        canvas.enabled = value;
    }

    public void SetHowToPlayActive(bool value)
    {
        howToPlay.SetActive(value);
        creditButton.SetActive(!value);
        playButton.SetActive(!value);
        copyright.SetActive(!value);
    }

    public void Reset()
    {
        SetHowToPlayActive(false);
    }
}
