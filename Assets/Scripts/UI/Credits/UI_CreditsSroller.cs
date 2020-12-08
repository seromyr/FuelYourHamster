using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreditsSroller : MonoBehaviour
{
    [SerializeField, Header("Start Position")]
    private float startY;

    [SerializeField, Header("Scroll Speed")]
    private float speed;

    private RectTransform rectTransform;
    private Vector3 startPosition;

    private bool scrolling;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y + startY, rectTransform.position.z);
        startPosition = rectTransform.position;

        scrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling)
        {
            //rectTransform.Translate(Vector3.up * Time.deltaTime);
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y + Time.deltaTime * speed, rectTransform.position.z);

            if (rectTransform.position.y > 3000)
            {
                UI_MainMenu.main.FadeIn(1);
                UI_Credits.main.FadeOut();
                ScrollReset();
            }
        }
    }

    public void Scroll()
    {
        scrolling = true;
    }

    public void ScrollReset()
    {
        rectTransform.position = startPosition;
        scrolling = false;
    }
}
