using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMeterMechanic : MonoBehaviour
{
    // Player reference
    private Player player;

    // For debugging
    [SerializeField]
    private float currentCaffeineLevel;

    // For adjusting color based on current caffeine level
    [SerializeField]
    private Gradient gradient;

    // Fill image
    [SerializeField]
    private Image fill;

    // Fill amount slider
    private Slider slider;

    // Coffee pouring animation
    private Image kettle, pouringCoffee;

    // Coffee serving check
    private bool isPouringCoffee;

    private void Start()
    {
        // Setups
        slider = GetComponent<Slider>();
        fill = GameObject.Find("Fill").GetComponent<Image>();
        kettle = GameObject.Find("Kettle").GetComponent<Image>();
        pouringCoffee = kettle.gameObject.transform.GetChild(0).GetComponent<Image>();
        HideKettle();
        isPouringCoffee = false;

        player = GameObject.Find("Player").GetComponent<Player>();
        currentCaffeineLevel = player.CaffeineLevel;
        SetCaffeineLevel(currentCaffeineLevel);
    }

    private void Update()
    {
        // Monitor and display player's current amount of caffeine in the blood stream
        currentCaffeineLevel = player.CaffeineLevel;
        SetCaffeineLevel(currentCaffeineLevel);

        if (player.RequestCoffee) PourCoffee();

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

    public void SetCaffeineLevel(float _health)
    {
        slider.value = _health;
        fill.color = gradient.Evaluate(_health / 100);
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
}
