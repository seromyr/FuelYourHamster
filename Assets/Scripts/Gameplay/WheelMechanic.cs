using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMechanic : MonoBehaviour
{
    [SerializeField, Header("Rotation Maximum Speed")]
    private float maxSpeed;

    [SerializeField, Header("Rotation Acceleration Speed")]
    private float accelerationSpeed;

    [SerializeField, Header("Rotation Speed")]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField, Header("Duration")]
    private float duration;

    public float FeedCoffee { set { duration += value; } }

    void Start()
    {
        //maxSpeed = 100;
        speed = 0;
    }

    void Update()
    {
        // Wheel only runs if player is running
        RotateWheel(Player.main.IsRunning);
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
