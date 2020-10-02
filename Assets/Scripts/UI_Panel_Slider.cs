using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel_Slider : MonoBehaviour
{
    private Vector3 defaultOffset;

    private RectTransform rectTransform;

    private Vector3 inPosition, outPosition;

    private bool called;

    [SerializeField, Header("UI Roll Speed")]
    private float speed;

    void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        defaultOffset.y = rectTransform.position.magnitude;
    }

    private void Start()
    {
        inPosition = rectTransform.position;
        outPosition = inPosition + defaultOffset;

        // Initialize default position
        rectTransform.position = outPosition;

        called = false;
    }

    void Update()
    {
        if (called)
        {
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, inPosition, Time.deltaTime * speed);
        }
        else
        {
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, outPosition, Time.deltaTime * speed);
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
