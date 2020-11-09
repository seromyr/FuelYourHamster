using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class WheelMechanic : MonoBehaviour
{
    [SerializeField, Header("Rotation Maximum Speed")]
    private float maxSpeed;
    public float MaxSpeed { get { return maxSpeed; } }

    [SerializeField, Header("Rotation Acceleration Speed")]
    private float accelerationSpeed;

    [SerializeField, Header("Rotation Speed")]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField, Header("Duration")]
    private float duration;

    public float FeedCoffee { set { duration += value; } }

    private void Start()
    {
        maxSpeed = CONST.DEFAULT_MAX_SPEED;
        accelerationSpeed = 20f;
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
            // Accelerate running speed
            speed += Time.deltaTime * accelerationSpeed;
            if (speed >= maxSpeed) speed = maxSpeed;
        }
        else
        {
            // Deaccelerate running speed
            speed -= Time.deltaTime * accelerationSpeed * 3;
            if (speed <= 0) speed = 0;
        }
        
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }

    public void SpeedUp(float value)
    {
        maxSpeed += value;
    }
}
