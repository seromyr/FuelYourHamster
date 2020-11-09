using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input Configurations
    private KeyCode jump, dodgeLeft, dodgeRight;

    // Jump mechanic
    [SerializeField, Header("Jump Mechanics"), Range(1, 20)]
    private float jumpVelocity;
    [SerializeField]
    private float fallMultiplier;
    [SerializeField]
    private float lowJumpMultiplier;
    private Rigidbody rb;
    public float rb_speed;

    [SerializeField]
    private Side dodgeSide;
    private Vector3 initLocalPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jump = KeyCode.Space;
        dodgeLeft = KeyCode.A;
        dodgeRight = KeyCode.D;
    }

    private void Start()
    {
        // Initialize jump mechanic parameters
        jumpVelocity = 9f;
        fallMultiplier = 7f;
        lowJumpMultiplier = 4f;

        dodgeSide = (Side)UnityEngine.Random.Range(1, 3);
        initLocalPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        // Jump!
        Jump();

        // Dodge!
        Dodge();

        rb_speed = rb.velocity.magnitude;
    }

    private bool RequestJump()
    {
        // Only allow jumping when player is grounded
        // This could cause double jump
        if (transform.position.y >= 1.4f && transform.position.y <= 1.7f) return Input.GetKey(jump);
        // Otherwise the key pressed has no effect
        else return false;
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
                transform.localPosition = new Vector3(initLocalPos.x + 4f, transform.localPosition.y, transform.localPosition.z);
                Player.main.IsInLaneNumber(3);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 25);
                transform.localPosition = new Vector3(initLocalPos.x - 4f, transform.localPosition.y, transform.localPosition.z);
                Player.main.IsInLaneNumber(1);
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.localPosition = new Vector3(initLocalPos.x, transform.localPosition.y, transform.localPosition.z);
            Player.main.IsInLaneNumber(2);
        }
    }
}
