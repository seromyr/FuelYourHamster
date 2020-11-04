using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class WheelMechanic : MonoBehaviour
{
    [SerializeField, Header("Rotation Maximum Speed")]
    private float maxSpeed;

    [SerializeField, Header("Rotation Speed")]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField, Header("Duration")]
    private float duration;

    public float FeedCoffee { set { duration += value; } }

    [SerializeField]
    private GameObject speedometer;

    void Start()
    {
        speed = maxSpeed;
    }

    void Update()
    {
        CheckDifficulty(Player.main.IsRunning);
        // Wheel only runs if player is running
        RotateWheel(Player.main.IsRunning);
    }

    private void CheckDifficulty(bool playerIsRunning)
    {
        if (playerIsRunning)
        {
            switch (GameManager.main.Difficulty)
            {
                case Difficulty.Kindergarten: maxSpeed = 50; break;
                case Difficulty.Decent: maxSpeed = 70; break;
                case Difficulty.Engaged: maxSpeed = 110; break;
                case Difficulty.Difficult: maxSpeed = 150; break;
                case Difficulty.Lightspeed: maxSpeed = 170; break;
                case Difficulty.Victory: maxSpeed = 0; break;
            }
        }
    }

    private void RotateWheel(bool playerIsRunning)
    {
        if (playerIsRunning) speed = maxSpeed;
        else speed = 0;
        
        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
