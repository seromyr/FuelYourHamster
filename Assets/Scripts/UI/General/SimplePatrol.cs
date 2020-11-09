using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    private enum Direction
    {
        Right,
        Up,
        Forward
    }

    [SerializeField]
    private Direction direction;

    private Vector3 sineWave;
    private float amplitude;

    [SerializeField, Header("Distance"), Range(0.0f, 10f)]
    private float distanceModifier;

    [SerializeField, Header("Speed"), Range(0, 1)]
    private float frequency;

    [SerializeField]
    private float offset;

    private float orginalPosition;

    private bool isMoving;

    void Start()
    {
        //frequency = 0.5f;
        //distanceModifier = 2f;

        GameManager.main.OnGamePlay += ActivateTheMovement;

        isMoving = false;

        switch (direction)
        {
            case Direction.Right:
                orginalPosition = transform.position.x;
                break;
            case Direction.Up:
                orginalPosition = transform.position.y;
                break;
            case Direction.Forward:
                orginalPosition = transform.position.z;
                break;
        }
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (isMoving)
        {
            amplitude = Mathf.Sin(Time.time / frequency + offset) * distanceModifier;
            switch (direction)
            {
                case Direction.Right:
                    sineWave = new Vector3(amplitude + orginalPosition, transform.position.y, transform.position.z);
                    break;
                case Direction.Up:
                    sineWave = new Vector3(transform.position.x, amplitude + orginalPosition, transform.position.z);
                    break;
                case Direction.Forward:
                    sineWave = new Vector3(transform.position.x, transform.position.y, amplitude + orginalPosition);
                    break;
            }

            transform.position = sineWave;
        }
    }

    private void ActivateTheMovement(object sender, EventArgs e)
    {
        StartCoroutine(SetPatrolActiveWithDelay(5, true));
    }
    private IEnumerator SetPatrolActiveWithDelay(float delay, bool state)
    {
        yield return new WaitForSeconds(delay);
        isMoving = state;
    }

    private void OnDestroy()
    {
        GameManager.main.OnGamePlay -= ActivateTheMovement;
    }
}
