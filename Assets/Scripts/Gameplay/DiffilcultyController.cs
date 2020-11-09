using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class DiffilcultyController : MonoBehaviour
{
    public static DiffilcultyController main;
    public float MaxSpeed { get { return wheelMechanic.MaxSpeed; } }

    private bool monitorDifficulty;
    private WheelMechanic wheelMechanic;

    private float markedTime, checkPointTime, beginTime, endTime;

    //private Queue<float> speedThreshold;

    public int minute, second;

    private void Awake()
    {
        Singletonize();
    }

    private void Singletonize()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Diffilculty Controller created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        RegulateDifficulty();
    }

    public void AssignWheel()
    {
        GameObject.Find("Wheel").TryGetComponent(out wheelMechanic);
    }

    public void MarkBeginTime()
    {
        beginTime = Time.time;
        monitorDifficulty = true;
    }

    public void MarkEndTime()
    {
        endTime = Time.time;
        monitorDifficulty = false;
    }

    public void MarkTime()
    {
        markedTime = Time.time;
    }

    public void SetCheckPointTime(float value)
    {
        checkPointTime = value;
    }

    public bool IsTimeToChangeDifficulty { get { return Time.time >= markedTime + checkPointTime; } }

    public bool VictoryConditionMet { get { return 1 > 2; } }

    public string RunTime { get { return RunTimeFormatted(); } }

    private string RunTimeFormatted()
    {
        float duration = endTime - beginTime;

        // Convert float to mm:ss format
        TimeSpan ts = TimeSpan.FromSeconds(duration);
        string timeText = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);

        return timeText;
    }

    public void RegulateDifficulty()
    {
        if (monitorDifficulty)
        {
            if (Time.time >= markedTime + checkPointTime)
            {
                Debug.Log("Speed Change");
                UI_Gameplay_Mechanic.main.ShowSpeedUpNotice();
                markedTime = Time.time;
                StartCoroutine(PumpTheSpeed(2f));
            }
        }
    }

    private IEnumerator PumpTheSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);
        wheelMechanic.SpeedUp(30f);
    }
}

