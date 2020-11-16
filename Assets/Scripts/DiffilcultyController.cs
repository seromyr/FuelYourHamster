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

    public int minute, second;

    public DifficultyLevel emptyScaling, coinScaling, beanScaling, goodMutationScaling, badMutationScaling, exBadMutationScaling, questScaling;

    public int totalRange;

    [SerializeField, Range(0, 1000)]
    private float emptyRange, coinRange, beanRange, goodRange, badRange, exBadRange, questRange;

    private void Awake()
    {
        Singletonize();

        // Default SpawnRate
        ResetSpawningRange();
    }

    private void Start()
    {
        emptyScaling         = new DifficultyLevel { startRange = 0                                 , endRange = (int)(emptyScaling.startRange         + emptyRange) };
        coinScaling          = new DifficultyLevel { startRange = emptyScaling.endRange         + 1 , endRange = (int)(coinScaling.startRange          + coinRange ) };
        beanScaling          = new DifficultyLevel { startRange = coinScaling.endRange          + 1 , endRange = (int)(beanScaling.startRange          + beanRange ) };
        goodMutationScaling  = new DifficultyLevel { startRange = beanScaling.endRange          + 1 , endRange = (int)(goodMutationScaling.startRange  + goodRange ) };
        badMutationScaling   = new DifficultyLevel { startRange = goodMutationScaling.endRange  + 1 , endRange = (int)(badMutationScaling.startRange   + badRange  ) };
        exBadMutationScaling = new DifficultyLevel { startRange = badMutationScaling.endRange   + 1 , endRange = (int)(exBadMutationScaling.startRange + exBadRange) };
        questScaling         = new DifficultyLevel { startRange = exBadMutationScaling.endRange + 1 , endRange = (int)(questScaling.startRange         + questRange) };
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
        RegulateSpawnScaling();
        RegulateSpawnRate();
        totalRange = questScaling.endRange;
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

    public void ResetSpawningRange()
    {
        emptyRange = 1000;
        coinRange  = 0;
        beanRange  = 0;
        goodRange  = 0;
        badRange   = 0;
        exBadRange = 0;
        questRange = 0;
    }

    private void RegulateSpawnScaling()
    {
        emptyScaling         . startRange = 0                                   ; emptyScaling         .endRange = (int)(emptyScaling         . startRange + emptyRange) ;
        coinScaling          . startRange = emptyScaling         . endRange + 1 ; coinScaling          .endRange = (int)(coinScaling          . startRange + coinRange ) ;
        beanScaling          . startRange = coinScaling          . endRange + 1 ; beanScaling          .endRange = (int)(beanScaling          . startRange + beanRange ) ;
        goodMutationScaling  . startRange = beanScaling          . endRange + 1 ; goodMutationScaling  .endRange = (int)(goodMutationScaling  . startRange + goodRange ) ;
        badMutationScaling   . startRange = goodMutationScaling  . endRange + 1 ; badMutationScaling   .endRange = (int)(badMutationScaling   . startRange + badRange  ) ;
        exBadMutationScaling . startRange = badMutationScaling   . endRange + 1 ; exBadMutationScaling .endRange = (int)(exBadMutationScaling . startRange + exBadRange) ;
        questScaling         . startRange = exBadMutationScaling . endRange + 1 ; questScaling         .endRange = (int)(questScaling         . startRange + questRange) ;
    }

    private void RegulateSpawnRate()
    {
        if (Time.time >= beginTime + 3f && emptyRange >= 300)
        {
            emptyRange -= Time.deltaTime * 60;
        }

        if (Time.time >= beginTime + 5f && coinRange <= 400)
        {
            coinRange += Time.deltaTime * 40;
        }

        if (Time.time >= beginTime + 4f && beanRange <= 50)
        {
            beanRange += Time.deltaTime * 20;
        }

        if (Time.time >= beginTime + 30f && badRange <= 50)
        {
            badRange += Time.deltaTime * 10;
        }

        if (Time.time >= beginTime + 45f && goodRange <= 50)
        {
            goodRange += Time.deltaTime * 10;
        }

        if (Time.time >= beginTime + 90f && exBadRange <= 50)
        {
            exBadRange += Time.deltaTime * 10;
        }
    }
}

