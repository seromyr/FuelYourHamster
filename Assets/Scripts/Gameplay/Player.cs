using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Player : MonoBehaviour
{
    // Gameplay Variables
    private int health;
    public int Health { get { return health; } }
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }

    // Input Configurations
    [SerializeField, Header("Input Configurations")]
    private KeyCode jump;
    [SerializeField]
    private KeyCode dodgeLeft;
    [SerializeField]
    private KeyCode dodgeRight;
    [SerializeField]
    private KeyCode serveCoffee;
    [SerializeField]
    private KeyCode vomitCoffee;

    // Jump mechanic
    [SerializeField, Header("Jump Mechanics"), Range(1, 20)]
    private float jumpVelocity;
    [SerializeField]
    private float fallMultiplier;
    [SerializeField]
    private float lowJumpMultiplier;
    private Rigidbody rb;

    [SerializeField]
    private Side dodgeSide;
    private Vector3 initLocalPos;

    // Hit mechanic
    [SerializeField, Header("Collision Color Effects")]
    private Color hitColor;
    [SerializeField]
    private Color collectColor;
    [SerializeField]
    private SpriteRenderer playerSprite;

    [SerializeField, Header("Current Coffee inside body")]
    private float caffeineLevel;
    public float CaffeineLevel { get { return caffeineLevel; } }

    public bool IsRunning { get { return caffeineLevel > 0; } }

    public bool RequestCoffee { get { return AskForCoffee(); } }

    private float caffeineMaxLevel, coffeePerServing;

    [SerializeField, Header("How fast could I digest all this?")]
    private float consumingSpeed;

    // Player get access to vault
    private ObjectReserve.Vault vault01, vault02, vault03;
    private string currentVaultName;

    // Player event
    public event EventHandler OnCollectCoin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float rb_speed;

    private void Start()
    {
        // Health info
        maxHealth = 2;
        health = maxHealth;

        // Initialize jump mechanic parameters
        //fallMultiplier = 2.5f;
        //lowJumpMultiplier = 2f;
        fallMultiplier = 7f;
        lowJumpMultiplier = 4f;

        // Initialize color effects
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerSprite.color = Color.white;

        dodgeSide = (Side)UnityEngine.Random.Range(1, 3);
        initLocalPos = transform.localPosition;

        caffeineMaxLevel = 100;
        coffeePerServing = 20;

        // Player start with some caffeine in its blood
        caffeineLevel = caffeineMaxLevel;

        consumingSpeed = 5;

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
        DrinkCoffee(- (Time.deltaTime * consumingSpeed));

        // Test 
        if (VomitCoffee()) DrinkCoffee(-coffeePerServing);
        else if (AskForCoffee()) DrinkCoffee(coffeePerServing);

        // Prevent cafein level going outside permited range
        CaffeineLevelLimiter();
    }

    private void FixedUpdate()
    {
        // Jump!
        Jump();

        // Dodge!
        Dodge();

        rb_speed = rb.velocity.magnitude;
    }

    private void DrinkCoffee(float amount)
    {
        caffeineLevel += amount;
    }

    private bool AskForCoffee()
    {
        return Input.GetKeyDown(serveCoffee);
    }

    private bool VomitCoffee()
    {
        return Input.GetKeyDown(vomitCoffee);
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

    private bool RequestJump()
    {
        // Only allow jumping when player is grounded
        // This could cause double jump
        //if (rb.velocity.magnitude >= 1f && rb.velocity.magnitude <= 1.2f)
        if (transform.position.y >= 1.5f && transform.position.y <= 1.7f)
            return Input.GetKeyDown(jump);
        // Otherwise the key pressed has no effect
        else return false;
        //else return Input.GetKeyDown(KeyCode.None);
    }

    private void Jump()
    {
        // Basic jump
        if (RequestJump())
        {
            rb.velocity = Vector3.up * jumpVelocity;
        }

        // The longer the jump button is hold the higher jump player makes
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.velocity.y > 0 && !Input.GetKey(jump))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool RequestDodge()
    {
        if (Input.GetKey(dodgeLeft))
        {
            dodgeSide = Side.Left;
            return Input.GetKey(dodgeLeft);
        }
        else if (Input.GetKey(dodgeRight))
        {
            dodgeSide = Side.Right;
            return Input.GetKey(dodgeRight);
        }
        else
        {
            return false;
        }
    }

    private void Dodge()
    {
        if (RequestDodge())
        {
            if (dodgeSide == Side.Left)
            {
                transform.rotation = Quaternion.Euler(0, 0, -25);
                transform.localPosition = new Vector3(initLocalPos.x + 3f, transform.localPosition.y, transform.localPosition.z);
                currentVaultName = vault03.name;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 25);
                transform.localPosition = new Vector3(initLocalPos.x - 3f, transform.localPosition.y, transform.localPosition.z);
                currentVaultName = vault01.name;
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.localPosition = new Vector3(initLocalPos.x, transform.localPosition.y, transform.localPosition.z);
            currentVaultName = vault02.name;
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
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
            OnCollectCoin?.Invoke(this, EventArgs.Empty);
        }
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

    public void UpgradeConsumingSpeed()
    {
        consumingSpeed -= 0.75f;
    }

    public void UpgradeMaxHealth()
    {
        maxHealth += 1;
    }
}
