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
    public float rb_speed;

    [SerializeField]
    private Lane currentLane;
    private Vector3 defaultPosition;

    private void Start()
    {
        // Initialize jump mechanic parameters
        jumpVelocity = 9f;
        fallMultiplier = 7f;
        lowJumpMultiplier = 4f;

        currentLane = Lane.Middle;
        defaultPosition = transform.localPosition;

        OnChangeLaneKeypressed += ChangeLane;
        OnJumpKeyPressed += Jump;
    }

    private void Update()
    {
        // Fixed inresponsive input
        GameplayInputListener();
    }

    private void FixedUpdate()
    {
        JumpMechanic();
        SwitchLaneMechanic();

        rb_speed = Player.main.RigidBody.velocity.magnitude;
    }

    private void GameplayInputListener()
    {
        if (IsGrounded())
        {
            JumpRequest();
        }

        LaneChangeRequest();
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
        if (Input.GetKey(CONST.JUMP_KEY) && Player.main.AllowPlayerControl)
        {
            OnJumpKeyPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void LaneChangeRequest()
    {
        if ((Input.GetKey(CONST.LEFT_KEY) || Input.GetKey(CONST.RIGHT_KEY)) & Player.main.AllowPlayerControl)
        {
            // Pass along the pressed key info
            OnChangeLaneKeypressed?.Invoke(this, new KeyInfo { keyPressed = Input.inputString.ToLower() /*Caplock input fixed*/ });
        }
    }

    private void Jump(object sender, EventArgs e)
    {
        // Basic jump
        Player.main.RigidBody.velocity = Vector3.up * jumpVelocity;
        Player.main.PlaySound(SoundController.main.SoundLibrary[3]);
    }

    private void JumpMechanic()
    {
        // The longer the jump button is hold the higher jump player makes
        if (Player.main.RigidBody.velocity.y < 0)
        {
            Player.main.RigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (Player.main.RigidBody.velocity.y > 0 && !Input.GetKey(CONST.JUMP_KEY))
        {
            Player.main.RigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ChangeLane(object sender, KeyInfo e)
    {
        switch (e.keyPressed)
        {
            case CONST.LEFT_KEY_STRING:
                if (currentLane != Lane.Left)
                {
                    currentLane--;
                    Player.main.PlaySound(SoundController.main.SoundLibrary[8]);
                }
                break;

            case CONST.RIGHT_KEY_STRING:
                if (currentLane != Lane.Right)
                {
                    currentLane++;
                    Player.main.PlaySound(SoundController.main.SoundLibrary[8]);
                }
                break;
        }
    }

    private void SwitchLaneMechanic()
    {
        switch (currentLane)
        {
            case Lane.Left:
                Strafe(defaultPosition.x + 4.5f);
                Player.main.IsInLaneNumber(3);
                break;

            case Lane.Middle:
                Strafe(defaultPosition.x);
                Player.main.IsInLaneNumber(2);
                break;

            case Lane.Right:
                Strafe(defaultPosition.x - 4.5f);
                Player.main.IsInLaneNumber(1);
                break;
        }
    }

    private void Strafe(float distance)
    {
        if (transform.localPosition.x != distance)
        {
            float speed = Time.deltaTime * Mathf.Pow(4, 2 + Mathf.Abs(Input.GetAxis("Horizontal")));
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(distance, transform.localPosition.y, transform.localPosition.z), speed);
        }
    }
}
