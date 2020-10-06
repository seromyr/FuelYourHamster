using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Constants;

public class Player : MonoBehaviour
{
    // Input Configurations
    [SerializeField, Header("Input Configurations")]
    private KeyCode jump;
    [SerializeField]
    private KeyCode dodge;
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

    private float caffeineMaxLevel, caffeineMinLevel, coffeePerServing;

    [SerializeField, Header("How fast could I digest all this?")]
    private float consumingSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Initialize jump mechanic parameters
        fallMultiplier = 2.5f;
        lowJumpMultiplier = 2f;

        // Initialize dodge mechanic parameters
        //dodgeSide = Side.none;

        // Initialize color effects
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerSprite.color = Color.white;

        dodgeSide = (Side)UnityEngine.Random.Range(1, 3);
        initLocalPos = transform.localPosition;

        caffeineMaxLevel = 100;
        caffeineMinLevel = 0;
        coffeePerServing = 20;

        // Player start with little caffeine in its blood
        caffeineLevel = caffeineMinLevel + 30;
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

        // Jump!
        Jump();

        // Dodge!
        Dodge();
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

    private bool RequestJump()
    {
        // Only allow jumping when player is grounded
        // This could cause double jump
        if (rb.velocity.magnitude <= 0.2f) return Input.GetKeyDown(jump);
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
        if (Input.GetKey(dodge))
        {
            return Input.GetKey(dodge);
        }
        else if (Input.GetKeyUp(dodge))
        {
            dodgeSide = (Side)UnityEngine.Random.Range(1, 3);
            return false;
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
            if (dodgeSide == Side.left)
            {
                transform.rotation = Quaternion.Euler(0, 0, -25);
                transform.localPosition = new Vector3(initLocalPos.x + 0.8f, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 25);
                transform.localPosition = new Vector3(initLocalPos.x - 0.6f, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.localPosition = new Vector3(initLocalPos.x, transform.localPosition.y, transform.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            //Debug.Log("Hit " + other.name);
            StartCoroutine(ColorSwitch(Color.white, hitColor));
        }

        if (other.CompareTag("GoodCollectable"))
        {
            //Debug.Log("Hit " + other.name);
            StartCoroutine(ColorSwitch(Color.white, collectColor));
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
}
