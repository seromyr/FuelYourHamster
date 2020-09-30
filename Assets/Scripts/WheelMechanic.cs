using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMechanic : MonoBehaviour
{
    [SerializeField, Header("Player Reference")]
    private Player player;

    [SerializeField, Header("Rotation Maximum Speed")]
    private float maxSpeed;

    [SerializeField, Header("Rotation Acceleration Speed")]
    private float accelerationSpeed;

    [SerializeField, Header("Rotation Speed")]
    private float speed;

    [SerializeField, Header("Duration")]
    private float duration;

    public float FeedCoffee { set { duration += value; } }

    void Start()
    {
        // Get player reference
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        maxSpeed = 1000;
        speed = 0;
    }

    void Update()
    {
        // Wheel only runs if player is running
        RotateWheel(player.IsRunning);
    }

    private void RotateWheel(bool playerIsRunning)
    {
        if (playerIsRunning)
        {
            speed += Time.deltaTime * accelerationSpeed;
            if (speed >= maxSpeed) speed = maxSpeed;

        }
        else
        {
            speed -= Time.deltaTime * accelerationSpeed * 3;
            if (speed <= 0) speed = 0;
        }
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
