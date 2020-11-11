using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler<KeyInfo> OnChangeLaneKeypressed;
    public event EventHandler OnJumpKeyPressed;

    public class KeyInfo : EventArgs
    {
        public string keyPressed;
    }

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
    private Lane currentLane;
    private Vector3 defaultPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Initialize jump mechanic parameters
        jumpVelocity = 9f;
        fallMultiplier = 7f;
        lowJumpMultiplier = 4f;

        //currentLane = (Lane)UnityEngine.Random.Range(1, 3);
        currentLane = Lane.Middle;
        defaultPosition = transform.localPosition;

        OnChangeLaneKeypressed += ChangeLane;
        OnJumpKeyPressed += Jump;
    }

    private void FixedUpdate()
    {
        GameplayInputListener();
        JumpMechanic();
        rb_speed = rb.velocity.magnitude;
    }

    private void GameplayInputListener()
    {
        if (IsGrounded())
        {
            LaneChangeRequest();
            JumpRequest();
        }
    }

    private bool IsGrounded()
    {
        // Define the condition that player is grounded
        return transform.position.y >= 1.4f && transform.position.y <= 1.7f;
    }

    private void JumpRequest()
    {
        // Only allow jumping when player is grounded
        // This could invoke double jump
        if (Input.GetKey(CONST.JUMP_KEY))
        {
            OnJumpKeyPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void LaneChangeRequest()
    {
        if (Input.GetKeyDown(CONST.LEFT_KEY) || Input.GetKeyDown(CONST.RIGHT_KEY))
        {
            // Pass along the pressed key info
            OnChangeLaneKeypressed?.Invoke(this, new KeyInfo { keyPressed = Input.inputString });
        }
    }

    private void Jump(object sender, EventArgs e)
    {
        // Basic jump
        rb.velocity = Vector3.up * jumpVelocity;
    }

    private void JumpMechanic()
    {
        // The longer the jump button is hold the higher jump player makes
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.velocity.y > 0 && !Input.GetKey(CONST.JUMP_KEY))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ChangeLane(object sender, KeyInfo e)
    {
        switch (e.keyPressed)
        {
            case CONST.LEFT_KEY_STRING:
                if (currentLane != Lane.Left) currentLane--;
                break;

            case CONST.RIGHT_KEY_STRING:
                if (currentLane != Lane.Right) currentLane++;
                break;
        }

        switch (currentLane)
        {
            case Lane.Left:
                transform.localPosition = new Vector3(defaultPosition.x + 4.5f, transform.localPosition.y, transform.localPosition.z);
                Player.main.IsInLaneNumber(3);
                break;

            case Lane.Middle:
                transform.localPosition = new Vector3(defaultPosition.x, transform.localPosition.y, transform.localPosition.z);
                Player.main.IsInLaneNumber(2);
                break;

            case Lane.Right:
                transform.localPosition = new Vector3(defaultPosition.x - 4.5f, transform.localPosition.y, transform.localPosition.z);
                Player.main.IsInLaneNumber(1);
                break;
        }
    }
}
