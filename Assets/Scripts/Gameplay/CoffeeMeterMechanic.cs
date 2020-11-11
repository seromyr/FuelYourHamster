using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMeterMechanic : MonoBehaviour
{
    // For debugging
    [SerializeField]
    private float currentCaffeineLevel;

    // Fill image
    [SerializeField]
    private Image barFill, barBkg, barOutline;

    // Coffee pouring animation
    private Image kettle, pouringCoffee;

    // Coffee serving check
    private bool isPouringCoffee;

    [SerializeField]
    private List<Sprite> barBkgs, barOutlines;

    private void Awake()
    {
        barBkgs.AddRange(Resources.LoadAll<Sprite>("UIComponents/Coffee-O-Meter/Backgrounds"));
        barOutlines.AddRange(Resources.LoadAll<Sprite>("UIComponents/Coffee-O-Meter/Outlines"));

        transform.Find("BarFill").TryGetComponent(out barFill);
        transform.Find("BarBackground").TryGetComponent(out barBkg);
        transform.Find("BarOutline").TryGetComponent(out barOutline);

        transform.Find("Coffee").transform.Find("Kettle").TryGetComponent(out kettle);
        pouringCoffee = kettle.transform.GetComponentInChildren<Image>();
    }

    private void Start()
    {
        HideKettle();
        isPouringCoffee = false;

        currentCaffeineLevel = Player.main.CaffeineCurrentLevel;
        SetCaffeineLevel(currentCaffeineLevel);
        SetBarLevel(0);
    }

    private void Update()
    {
        // Monitor and display player's current amount of caffeine in the blood stream
        currentCaffeineLevel = Player.main.CaffeineCurrentLevel;
        SetCaffeineLevel(currentCaffeineLevel);

        if (Player.main.Mechanic.RequestCoffee) PourCoffee();

        if (isPouringCoffee) CoffeeFlow(200);

        if (pouringCoffee.fillAmount >= 1) isPouringCoffee = false;

        // Coffee flow animation
        if (!isPouringCoffee && pouringCoffee.fillAmount <= 0) HideKettle();

        // Allow the flow to finish before stopping the flow
        if (!isPouringCoffee && pouringCoffee.fillAmount > 0)
        {
            pouringCoffee.fillOrigin = 0;
            CoffeeFlow(-200);
        }
    }

    public void SetCaffeineLevel(float value)
    {
        barFill.fillAmount = value / Player.main.CaffeineMaxLevel;
    }

    private void HideKettle()
    {
        // Hide the kettle and its pouring water
        kettle.enabled = false;
        pouringCoffee.enabled = false;

        // Reset pouring direction
        ResetFillType();
    }

    private void PourCoffee()
    {
        kettle.enabled = true;
        pouringCoffee.enabled = true;

        // Reset pouring direction
        ResetFillType();

        if (!isPouringCoffee) isPouringCoffee = true;
    }

    private void CoffeeFlow(float speed)
    {
        pouringCoffee.fillAmount += Time.deltaTime * speed / 100;
    }

    private void ResetFillType()
    {
        pouringCoffee.fillOrigin = 1;
        pouringCoffee.fillAmount = 0;
    }

    public void SetBarLevel(int value)
    {
        barFill.sprite = barBkg.sprite = barBkgs[value];
        barOutline.sprite = barOutlines[value];
    }
}
