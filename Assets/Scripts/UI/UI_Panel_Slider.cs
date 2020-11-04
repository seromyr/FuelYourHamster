using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel_Slider : MonoBehaviour
{
    private Vector3 defaultOffset;

    private RectTransform panelRectTransform;

    private Vector3 inPosition, outPosition;

    private bool called;

    [SerializeField, Header("UI Roll Speed")]
    private float speed;

    void Awake()
    {
        panelRectTransform = transform.Find("Panel").GetComponent<RectTransform>();
    }

    private void Start()
    {
        inPosition = Vector3.zero;
        outPosition = new Vector3(0, GetComponent<RectTransform>().position.y * 2, 0);

        speed = 10;

        // Initialize default position
        panelRectTransform.localPosition = outPosition;

        called = false;
    }

    void Update()
    {
        if (called)
        {
            panelRectTransform.localPosition = Vector3.Lerp(panelRectTransform.localPosition, inPosition, Time.deltaTime * speed);
        }
        else
        {
            panelRectTransform.localPosition = Vector3.Lerp(panelRectTransform.localPosition, outPosition, Time.deltaTime * speed);
        }
    }

    public void ScrollIn()
    {
        called = true;
    }

    public void ScrollOut()
    {
        called = false;
    }
}
