using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class PlayerMechanic : MonoBehaviour
{
    public bool RequestCoffee { get { return AskForCoffee(); } }

    private float coffeePerServing;

    // Hit mechanic
    [SerializeField, Header("Collision Color Effects")]
    private Color hitColor;
    [SerializeField]
    private Color collectColor;

    // Token Collection event
    public event EventHandler OnCollectToken;

    private void Start()
    {
        coffeePerServing = 20; // For cheating, remember to remove before gold

        // Player start with some caffeine in its blood
        Player.main.FullLoadFuel();

        // Initialize color effects
        Player.main.SpriteRenderer.color = Color.white;
    }

    private void Update()
    {
        // Player caffeine level is continously decreasing
        Player.main.ConsumeFuel(- (Time.deltaTime * Player.main.FuelConsumptionSpeed));

        // For cheat/debugging purpose, remember to remove before gold
        if (VomitCoffee()) Player.main.ConsumeFuel(-coffeePerServing);
        else if (AskForCoffee()) Player.main.ConsumeFuel(coffeePerServing);

        // Prevent cafein level going outside permited range
        Player.main.FuelLimiter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.OBJBAD))
        {
            if (Player.main.HamsterBall != null && Player.main.HamsterBall.IsActive)
            {
                Player.main.HamsterBall.DegradeSphere();
            }
            else
            {
                StartCoroutine(ColorSwitch(Color.white, hitColor));
                if (Player.main.Health > 0) Player.main.TakeDamage(1);
            }
            Destroy(other.gameObject);
            Player.main.CollectiblePooling();
        }

        else if (other.CompareTag(TAG.OBJGOOD))
        {
            if (Player.main.Health < Player.main.MaxHealth) Player.main.RestoreHealth(1);
            StartCoroutine(ColorSwitch(Color.white, collectColor));
            Destroy(other.gameObject);
            Player.main.CollectiblePooling();
        }

        else if (other.CompareTag(TAG.COIN))
        {
            Destroy(other.gameObject);
            Player.main.CollectiblePooling();

            // Send event notification that player has collected a coin
            OnCollectToken?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool AskForCoffee()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    private bool VomitCoffee()
    {
        return Input.GetKeyDown(KeyCode.Backspace);
    }

    private IEnumerator ColorSwitch(Color color1, Color color2)
    {
        for (int i = 0; i < 5; i++)
        {
            Player.main.SpriteRenderer.color = color1;
            yield return new WaitForSeconds(0.1f);
            Player.main.SpriteRenderer.color = color2;
            yield return new WaitForSeconds(0.1f);
        }

        Player.main.SpriteRenderer.color = color1;
    }
}
