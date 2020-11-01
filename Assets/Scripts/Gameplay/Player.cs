using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Player : MonoBehaviour
{
    public static Player main;

    // Gameplay Variables
    private int health;
    public int CurrentHealth { get { return health; } }
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }

    [SerializeField, Header("Current Caffeine inside body")]
    private float caffeineLevel;
    public float CaffeineCurrentLevel { get { return caffeineLevel; } }

    public bool IsRunning { get { return caffeineLevel > 0; } }

    public bool RequestCoffee { get { return AskForCoffee(); } }

    private float caffeineMaxLevel, coffeePerServing;
    public float CaffeineMaxLevel { get { return caffeineMaxLevel; } }

    [SerializeField, Header("Caffeine Digestion Speed")]
    private float caffeineConsumingSpeed;

    // Hit mechanic
    [SerializeField, Header("Collision Color Effects")]
    private Color hitColor;
    [SerializeField]
    private Color collectColor;
    [SerializeField]
    private SpriteRenderer playerSprite;

    // Player get access to vault
    private ObjectReserve.Vault vault01, vault02, vault03;
    private string currentVaultName;

    // Token Collection event
    public event EventHandler OnCollectToken;

    private void Awake()
    {
        // Make player become a Singleton
        Singletonizer();
    }

    private void Start()
    {
        // Health info
        maxHealth = 2;
        health = maxHealth;

        caffeineMaxLevel = CONST.DEFAULT_MAX_CAFFEINE_LEVEL;
        coffeePerServing = 20; // For cheating, remember to remove before gold

        // Player start with some caffeine in its blood
        caffeineLevel = caffeineMaxLevel;

        caffeineConsumingSpeed = CONST.DEFAULT_CAFFEINE_COSUMING_SPEED;

        // Initialize color effects
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerSprite.color = Color.white;
    }

    private void Singletonizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    public void AssignVault()
    {
        vault01 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault01;
        vault02 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault02;
        vault03 = GameObject.Find(PrimeObj.OBJPOOL).GetComponent<ObjectReserve>().Vault03;
    }

    private void Update()
    {
        // Player cafein level is continously decreasing
        DrinkCoffee(- (Time.deltaTime * caffeineConsumingSpeed));

        // For cheat/debugging purpose, remember to remove before gold
        if (VomitCoffee()) DrinkCoffee(-coffeePerServing);
        else if (AskForCoffee()) DrinkCoffee(coffeePerServing);

        // Prevent cafein level going outside permited range
        CaffeineLevelLimiter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAG.OBJBAD))
        {
            if (health > 0) health -= 1;
            StartCoroutine(ColorSwitch(Color.white, hitColor));
            Destroy(other.gameObject);
            CollectiblePooling();
        }

        else if (other.CompareTag(TAG.OBJGOOD))
        {
            if (health < maxHealth) health += 1;
            StartCoroutine(ColorSwitch(Color.white, collectColor));
            Destroy(other.gameObject);
            CollectiblePooling();
        }

        else if (other.CompareTag(TAG.COIN))
        {
            Destroy(other.gameObject);
            CollectiblePooling();

            // Send event notification that player has collected a coin
            OnCollectToken?.Invoke(this, EventArgs.Empty);
        }
    }

    private void DrinkCoffee(float amount)
    {
        caffeineLevel += amount;
    }

    private bool AskForCoffee()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    private bool VomitCoffee()
    {
        return Input.GetKeyDown(KeyCode.Backspace);
    }

    private void CaffeineLevelLimiter()
    {
        if (caffeineLevel > 100)
        {
            caffeineLevel = 100;
        }
        else if (caffeineLevel < 0)
        {
            caffeineLevel = 0;
        }
    }

    public void ResetCaffeineLevel()
    {
        caffeineLevel = caffeineMaxLevel;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    private void CollectiblePooling()
    {
        // Coin pooling on player side
        switch (currentVaultName)
        {
            case PrimeObj.VAULT01:
                for (int i = 0; i < vault01.capacity; i++)
                {
                    if (!vault01.isInVault[i])
                    {
                        vault01.isInVault[i] = true;
                        break;
                    }
                }
                break;
            case PrimeObj.VAULT02:
                for (int i = 0; i < vault02.capacity; i++)
                {
                    if (!vault02.isInVault[i])
                    {
                        vault02.isInVault[i] = true;
                        break;
                    }
                }
                break;
            case PrimeObj.VAULT03:
                for (int i = 0; i < vault03.capacity; i++)
                {
                    if (!vault03.isInVault[i])
                    {
                        vault03.isInVault[i] = true;
                        break;
                    }
                }
                break;
        }
    }
    private IEnumerator ColorSwitch(Color color1, Color color2)
    {
        for (int i = 0; i < 5; i++)
        {
            playerSprite.color = color1;
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = color2;
            yield return new WaitForSeconds(0.1f);
        }

        playerSprite.color = color1;
    }

    public void UpgradeFuelEfficiency()
    {
        caffeineConsumingSpeed -= 0.75f;
    }

    public void UpgradeMaxHealth()
    {
        maxHealth += 1;
    }

    public void UpgradeMaxFuel()
    {
        caffeineMaxLevel += 20;
    }

    public void IsInLaneNumber(int value)
    {
        switch (value)
        {
            case 1: currentVaultName = vault01.name; break;
            case 2: currentVaultName = vault02.name; break;
            case 3: currentVaultName = vault03.name; break;
            default: break;
        }
    }
}
