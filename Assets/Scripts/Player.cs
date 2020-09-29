using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Header("Current Coffee inside body")]
    private float caffeineLevel;
    public float CaffeineLevel { get { return caffeineLevel; } }

    public bool IsRunning { get { return caffeineLevel > 0; } }

    public bool RequestCoffee { get { return Input.GetKeyDown(KeyCode.Space); } }

    private float caffeineMaxLevel, caffeineMinLevel, coffeePerServing;

    [SerializeField, Header("How fast could I digest all this?")]
    private float consumingSpeed;

    private void Start()
    {
        caffeineMaxLevel = 100;
        caffeineMinLevel = 0;
        coffeePerServing = 20;

        // Player start with no caffeine in its blood
        caffeineLevel = caffeineMinLevel;
    }

    private void Update()
    {
        // Player cafein level is continously decreasing
        DrinkCoffee(- (Time.deltaTime * consumingSpeed));

        // Test 
        if (VomitCoffee()) DrinkCoffee(-coffeePerServing);
        else if (AskForCoffee()) DrinkCoffee(coffeePerServing);

        // Prevent cafein level going outside permited range
        if (caffeineLevel > 100)
        {
            caffeineLevel = 100;
        }
        else if  (caffeineLevel < 0)
        {
            caffeineLevel = 0;
        }
    }

    private void DrinkCoffee(float amount)
    {
        caffeineLevel += amount;
    }

    private bool AskForCoffee()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool VomitCoffee()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }
}
